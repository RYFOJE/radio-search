using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Radio_Search.Importer.Canada.Data.Enums;
using Radio_Search.Importer.Canada.Data.Models;
using Radio_Search.Importer.Canada.Data.Models.ImportInfo;
using Radio_Search.Importer.Canada.Data.Repositories.Interfaces;
using Radio_Search.Importer.Canada.Data_Contracts.V1;
using Radio_Search.Importer.Canada.Services.Configuration;
using Radio_Search.Importer.Canada.Services.Data;
using Radio_Search.Importer.Canada.Services.Interfaces;
using Radio_Search.Importer.Canada.Services.Interfaces.TAFLDefinitionImport;
using Radio_Search.Importer.Canada.Services.Interfaces.TAFLImport;
using Radio_Search.Importer.Canada.Services.Responses;
using Radio_Search.Utils.BlobStorage.Interfaces;
using Radio_Search.Utils.MessageBroker.Factories.Interfaces;
using System.Diagnostics;

namespace Radio_Search.Importer.Canada.Services.Implementations
{
    public class ImportManagerService : IImportManagerService
    {
        private readonly IImportJobRepo _importJobRepo;
        private readonly ILogger<ImportManagerService> _logger;
        private readonly IBlobStorageService _blobService;
        private readonly FileLocations _fileLocations;
        private readonly ITAFLDefinitionImportService _definitionImportService;
        private readonly IPreprocessingService _preprocessingService;
        private readonly IConfiguration _config;
        private readonly IMessageBrokerWriteFactory _sbWriteFactory;
        private readonly IDownloadFileService _downloadFileService;
        private readonly IProcessingService _processingService;
        private readonly ServiceBusDefinitions _sbDefinitions;

        public ImportManagerService(
            IImportJobRepo importJobRepo,
            ILogger<ImportManagerService> logger,
            IBlobStorageService blobService,
            IOptions<FileLocations> fileLocations,
            ITAFLDefinitionImportService definitionImportService,
            IPreprocessingService preprocessingService,
            IConfiguration config,
            IMessageBrokerWriteFactory sbWriterFactory,
            IDownloadFileService downloadService,
            IProcessingService processingService,
            IOptions<ServiceBusDefinitions> sbDefinitions) 
        { 
            _importJobRepo = importJobRepo;
            _logger = logger;
            _blobService = blobService;
            _fileLocations = fileLocations.Value;
            _definitionImportService = definitionImportService;
            _preprocessingService = preprocessingService;
            _config = config;
            _sbWriteFactory = sbWriterFactory;
            _downloadFileService = downloadService;
            _processingService = processingService;
            _sbDefinitions = sbDefinitions.Value;
        }

        /// <inheritdoc/>
        public async Task StartImportJobAsync()
        {
            // TODO: Add validation that there isn't currently an import going on
            var timer = Stopwatch.StartNew();
            try
            {
                _logger.LogInformation("Starting import job creation.");
                var job = await _importJobRepo.UpsertImportJobRecordAsync(new());
                _logger.LogInformation("Import job record created in {ElapsedMs} ms. JobID: {JobId}", timer.ElapsedMilliseconds, job.ImportJobID);

                timer.Restart();

                var definitionFileName = string.Format(_fileLocations.UnprocessedTAFLDefinition, job.ImportJobID);
                var taflFileName = string.Format(_fileLocations.UnprocessedTAFLRows, job.ImportJobID);

                _logger.LogInformation("Starting download of TAFL Definition and TAFL Rows files: {DefinitionFile}, {TaflFile}", definitionFileName, taflFileName);

                var definition = _downloadFileService.DownloadAndSaveRecentTAFLDefinition(definitionFileName);
                var tafl = _downloadFileService.DownloadAndSaveRecentTAFL(taflFileName);

                // Wait for both downloads to finish
                await Task.WhenAll(definition, tafl);

                _logger.LogInformation("Finished downloading TAFL files in {ElapsedMs} ms.", timer.ElapsedMilliseconds);

                timer.Restart();

                await WriteMessageToSB(new DownloadCompleteMessage { ImportJobID = job.ImportJobID }, _sbDefinitions.TopicName, _sbDefinitions.DownloadComplete_SubscriptionName);

                _logger.LogInformation("Download complete message sent in {ElapsedMs} ms.", timer.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start import job.");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task HandleDownloadCompleteAsync(int importJobID)
        {
            var job = await _importJobRepo.GetImportJobRecordAsync(importJobID);
            var timer = Stopwatch.StartNew();
            try
            {
                if(!IsProcessable(job, TimeSpan.FromMinutes(5))) // TODO: Make this configurable
                {
                    _logger.LogInformation("HandleDownloadCompleteAsync has already been processed or is currently being processed. Job: {@Job}", job);
                    return;
                }
                else if(job.Status == ImportStatus.Failure)
                {
                    _logger.LogInformation("Job is in a failed status. Stopping any further processing.");
                    return;
                }

                job.CurrentStep = ImportStep.Chunking;
                await _importJobRepo.UpsertImportJobRecordAsync(job);

                // DEFINITION STEP
                _logger.LogInformation("Starting to process the TAFL Definition File.");

                using (var definitionFileStream = await _blobService.DownloadAsync(string.Format(_fileLocations.UnprocessedTAFLDefinition, importJobID)))
                {
                    var definitionRows = _definitionImportService.ProcessTAFLDefinition(definitionFileStream);
                    await _definitionImportService.SaveTAFLDefinitionToDBAsync(definitionRows.Tables);
                }

                _logger.LogInformation("Finished processing the TAFL Definition File within {ElapsedMs} ms.", timer.ElapsedMilliseconds);

                timer.Restart();

                // TAFL CSV STEP
                List<TaflEntryRawRow> rows;

                using(Stream rawFullStream = await _blobService.DownloadAsync(string.Format(_fileLocations.UnprocessedTAFLRows, importJobID)))
                {
                    rows = _preprocessingService.DeduplicateFullFile(rawFullStream);
                }

#warning ADD APP CONFIG VALUE
                int chunkSize = _config.GetValue("ChunkSize", 2_000); // TODO: Add the actual size in appconfig and remove default

                var chunkedRows = rows
                    .Chunk(chunkSize)
                    .Select(c => c.ToList())
                    .ToList();


                var chunkTimer = Stopwatch.StartNew();
                var chunkIndex = 0;
                foreach (var rowsChunk in chunkedRows) 
                {
                    string fileLocation = string.Format(_fileLocations.ChunkFile, importJobID, chunkIndex);

                    if (!await _blobService.ExistsAsync(fileLocation))
                    {
                        using (Stream chunkStream = _preprocessingService.GenerateChunkFile(rowsChunk))
                        {
                            await _blobService.UploadAsync(fileLocation, chunkStream);
                        }

                        await CreateAndUploadChunkRecord(importJobID, chunkIndex); // DANGLING RECORD IF SB WRITE FAILS
                        await WriteMessageToSB(
                            new ProcessChunkMessage
                            {
                                ImportJobID = importJobID,
                                FileID = chunkIndex,
                                FileLocation = fileLocation
                            }, _sbDefinitions.TopicName, _sbDefinitions.ChunkReady_SubscriptionName);

                        _logger.LogInformation("Finished creating chunk file at location: {Location} within {ElapsedMs} ms.", fileLocation, chunkTimer.ElapsedMilliseconds);
                    }
                    else
                    {
                        _logger.LogWarning("Chunk file already existed. This is probably due to an Handle Downloaded that failed mid download. File: {File}", fileLocation);
                    }


                    chunkIndex++;
                    chunkTimer.Restart();
                }

                job.CurrentStep = ImportStep.ProcessingChunks;
                await _importJobRepo.UpsertImportJobRecordAsync(job);
            }
            catch (Exception ex)
            {
                job.CurrentStep = ImportStep.DownloadingFiles;
                await _importJobRepo.UpsertImportJobRecordAsync(job); // What if this throws??
                _logger.LogError(ex, "Failed to HandleDownloadCompleteAsync");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task MarkImportAsFailedAsync(int importJobID)
        {
            try
            {
                var job = await _importJobRepo.GetImportJobRecordAsync(importJobID);
                job.Status = ImportStatus.Failure;

                await _importJobRepo.UpsertImportJobRecordAsync(job);

#warning NOT VALID
                await WriteMessageToSB(new ImportJobFailedMessage { ImportJobID = importJobID }, "ServiceBus:Names:ImportFailed", ""); // TODO: Switch to strongly typed
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to set the import as failed for ImportJobID: {ImportJobId}", importJobID);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task ProcessChunkAsync(int importJobID, int chunkID, Func<Task> renewLockFunc)
        {
            var chunkRecord = await _importJobRepo.GetImportJobChunkFileAsync(importJobID, chunkID);
            var importJobRecord = await _importJobRepo.GetImportJobRecordAsync(importJobID);

            try
            {
                if(!IsProcessable(chunkRecord, TimeSpan.FromMinutes(5)))
                {
                    _logger.LogWarning("Concurrency issue, the file is currently being processed or has been completed by another instance.");
                }
                else if(importJobRecord.Status == ImportStatus.Failure)
                {
                    _logger.LogInformation("Import job failed in another instance. No longer processing.");
                    return;
                }

                // Mark record as being worked on
                chunkRecord.Status = FileStatus.Processing;
                chunkRecord.StartTime = DateTime.UtcNow;
                await _importJobRepo.UpsertImportJobChunkFileRecordAsync(chunkRecord);

                GetValidRawRowsResponse preprocessResponse;

                using (Stream chunkStream = await _blobService.DownloadAsync(string.Format(_fileLocations.ChunkFile, importJobID, chunkID)))
                    preprocessResponse = await _preprocessingService.GetValidRawRows(chunkStream);

                _logger.LogInformation("Finished getting valid rows. {InvalidRecords} invalid records.", preprocessResponse.InvalidRowCount);

                var insertAndUpdated = await _processingService.GetInsertsAndUpdates(preprocessResponse.ValidRows);

                await renewLockFunc();
                await _processingService.InsertNewFromRawRecords(insertAndUpdated.InsertRows, importJobID);

                await renewLockFunc();
                await _processingService.InsertUpdatedFromRawRecords(insertAndUpdated.UpdateRows, importJobID);

                _logger.LogInformation("Increasing stats field, New Count: {RowsAddedCount} Updated Count: {RowsUpdatedCount}.",
                    insertAndUpdated.InsertRows.Count, insertAndUpdated.UpdateRows.Count);

                await _importJobRepo.IncrementStatsFieldAsync(importJobID, e => e.NewRecordCount, insertAndUpdated.InsertRows.Count);
                await _importJobRepo.IncrementStatsFieldAsync(importJobID, e => e.UpdatedRecordCount, insertAndUpdated.UpdateRows.Count);

                await HandleChunkDoneAsync(chunkID, importJobID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process chunk for JobID: {JobID} ChunkID: {ChunkID}", importJobID, chunkID);
                chunkRecord.StartTime = null;
                await _importJobRepo.UpsertImportJobChunkFileRecordAsync(chunkRecord);

                throw;
            }
        }

        /// <inheritdoc/>
        public Task RevertFailedImportAsync(int importJobID)
        {
            throw new NotImplementedException();
        }

        private async Task CreateAndUploadChunkRecord(int importJobID, int ChunkID)
        {
            var chunkRecord = new ImportJobChunkFile
            {
                ImportJobID = importJobID,
                FileID = ChunkID
            };

            await _importJobRepo.UpsertImportJobChunkFileRecordAsync(chunkRecord);
        }

        ///<inheritdoc/>
        public async Task HandleChunkProcessingCompleteAsync(int importJobID)
        {
            var timer = Stopwatch.StartNew();
            
            _logger.LogInformation("Getting all records not included in import.");
            var deletedRecords = await _processingService.GetDeletedRecords(importJobID);
                
            _logger.LogInformation("Fetched {DeletedCount} records in {ElapsedMs} ms.", deletedRecords.Count, timer.ElapsedMilliseconds);
            timer.Restart();

            _logger.LogInformation("Starting to invalidate {RecordCount} records.", deletedRecords.Count);
            await _processingService.InvalidateRecordsFromDB(deletedRecords, importJobID);
            _logger.LogInformation("Finished invalidating deleted records in {ElapsedMs} ms.", timer.ElapsedMilliseconds);

            var importJobRecord = await _importJobRepo.GetImportJobRecordAsync(importJobID);
            importJobRecord.CurrentStep = ImportStep.Complete;
            importJobRecord.EndTime = DateTime.UtcNow;

            await _importJobRepo.IncrementStatsFieldAsync(importJobID, e => e.DeletedRecordCount, deletedRecords.Count);

            await _importJobRepo.UpsertImportJobRecordAsync(importJobRecord);
        }

        private async Task WriteMessageToSB(object message, string topicName, string targetName)
        {
            try
            {
                var writer = _sbWriteFactory.GetMessageBrokerWriter(topicName);

                await writer.WriteMessageAsync(message, targetName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while writing download complete message to Service Bus.");
                throw;
            }
        }

        public async Task HandleChunkDoneAsync(int chunkID, int importJobID)
        {
            var chunkRecord = await _importJobRepo.GetImportJobChunkFileAsync(importJobID, chunkID);
            chunkRecord.Status = FileStatus.Processed;
            chunkRecord.EndTime = DateTime.UtcNow;
            await _importJobRepo.UpsertImportJobChunkFileRecordAsync(chunkRecord);


            if (!await _importJobRepo.IsChunkProcessingDoneAsync(importJobID))
                return;

            _logger.LogInformation("Chunk processing is done. Inserting Fan In message to service bus.");
            var writer = _sbWriteFactory.GetMessageBrokerWriter(_sbDefinitions.TopicName);

            await writer.WriteMessageAsync(new ChunkProcessingComplete());
        }

        private static bool IsProcessable(TimeTrackedEntry record, TimeSpan expiryTime)
        {
            if (record.StartTime == null)
                return true;

            else if (record.EndTime != null)
                return false;

            return record.StartTime + expiryTime < DateTime.UtcNow;
        }
    }
}
