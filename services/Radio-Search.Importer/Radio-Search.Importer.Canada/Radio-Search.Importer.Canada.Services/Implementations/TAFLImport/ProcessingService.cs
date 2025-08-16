using AutoMapper;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Radio_Search.Importer.Canada.Data;
using Radio_Search.Importer.Canada.Data.Enums;
using Radio_Search.Importer.Canada.Data.Models.History;
using Radio_Search.Importer.Canada.Data.Models.License;
using Radio_Search.Importer.Canada.Data.Repositories.Interfaces;
using Radio_Search.Importer.Canada.Services.Data;
using Radio_Search.Importer.Canada.Services.Interfaces.TAFLImport;
using Radio_Search.Importer.Canada.Services.Responses;
using System.Diagnostics;

namespace Radio_Search.Importer.Canada.Services.Implementations.TAFLImport
{
    public class ProcessingService : IProcessingService
    {
        private readonly ILogger<ProcessingService> _logger;
        private readonly ITAFLRepo _taflRepo;
        private readonly CanadaImporterContext _context;
        private readonly IMapper _mapper;
        private readonly IImportJobRepo _importJobRepo;

        public ProcessingService(
            ILogger<ProcessingService> logger,
            ITAFLRepo taflRepo,
            CanadaImporterContext context,
            IMapper mapper,
            IImportJobRepo importJobRepo)
        {
            _logger = logger;
            _taflRepo = taflRepo;
            _context = context;
            _mapper = mapper;
            _importJobRepo = importJobRepo;
        }

        /// <inheritdoc/>
        public async Task<GetInsertsAndUpdatesResponse> GetInsertsAndUpdates(List<TAFLEntryRawRow> rows)
        {
            var timer = Stopwatch.StartNew();
            try
            {
                var response = new GetInsertsAndUpdatesResponse();

                _logger.LogInformation("Starting for fetch all DB License Records for the License Records.");
                var matchedDbRecords = (await _taflRepo.BulkFetchLicenseRecordsNoTrackingAsync(rows.Select(x => x.LicenseRecordID).ToList())).ToDictionary(t => t.CanadaLicenseRecordID, t => t);

                _logger.LogInformation("Finished fetching all DB License Records for the License Records within {elapsedMs} ms.", timer.ElapsedMilliseconds);

                response.InsertRows = rows.Where(x => !matchedDbRecords.ContainsKey(x.LicenseRecordID)).ToList();

                // Get ones that need updating

                response.UpdateRows = rows.Where(x =>
                    {
                        if (!matchedDbRecords.TryGetValue(x.LicenseRecordID, out var dbRecord))
                            return false;

                        return !x.Equals(dbRecord);

                    })
                    .Select(x => (matchedDbRecords[x.LicenseRecordID].Version + 1, x)) // Append the version number that will be the new. THIS IS WHERE THE LOGIC RESIDES TO CHANGE VERSION NUMBER
                    .ToList();

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while trying to GetInsertsAndUpdates.");
                throw;
            }
        }

        public async Task InsertNewFromRawRecords(List<TAFLEntryRawRow> rows, int importID)
        {
            var dbNewLicenses = _mapper.Map<List<LicenseRecord>>(rows);


            var timer = Stopwatch.StartNew();
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                List<LicenseRecordHistory> licenseHistoryRecord = [];

                licenseHistoryRecord.AddRange(dbNewLicenses.Select(
                    x => CreateLicenseRecordHistory(x, importID, ChangeType.Created))
                );

                _logger.LogInformation("Inserting {NewLicenseCount} new Licenses into the DB.", dbNewLicenses.Count());

                await _taflRepo.BulkAddLicenseRecordsAsync(dbNewLicenses);

                _logger.LogInformation("Finished inserting licenses into DB after {timeElapsedMS} ms", timer.ElapsedMilliseconds);

                timer.Restart();
                _logger.LogInformation("Inserting {LicenseHistoryCount} License History Records", licenseHistoryRecord.Count());

                await _importJobRepo.BulkInsertLicenseRecordHistory(licenseHistoryRecord);
                _logger.LogInformation("Finished inserting License Record Histories into DB after {timeElapsedMS} ms", timer.ElapsedMilliseconds);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Failed while trying to InsertNewFromRawRecords");
                throw;
            }
        }


        public async Task InsertUpdatedFromRawRecords(List<(int version, TAFLEntryRawRow row)> rows, int importID)
        {
            var dbUpdatedLicenses = CreateUpdatedLicenseRecords(rows);
            var timer = Stopwatch.StartNew();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Invalidating {UpdatedLicenseCount} outdated Licenses into the DB.", dbUpdatedLicenses.Count());
                await _taflRepo.BulkInvalidateRecords(dbUpdatedLicenses.Select(x => x.CanadaLicenseRecordID).ToList());
                _logger.LogInformation("Finished invalidating records in {elapsedMs} ms.", timer.ElapsedMilliseconds);
                timer.Restart();

                List<LicenseRecordHistory> licenseHistoryRecord = [];
                licenseHistoryRecord.AddRange(dbUpdatedLicenses.Select(
                    x => CreateLicenseRecordHistory(x, importID, ChangeType.Updated))
                );

                _logger.LogInformation("Inserting {UpdatedLicenseCount} updated Licenses into the DB.", dbUpdatedLicenses.Count());

                await _taflRepo.BulkAddLicenseRecordsAsync(dbUpdatedLicenses);

                _logger.LogInformation("Finished inserting licenses into DB after {timeElapsedMS} ms", timer.ElapsedMilliseconds);

                timer.Restart();
                _logger.LogInformation("Inserting {LicenseHistoryCount} License History Records", licenseHistoryRecord.Count());

                await _importJobRepo.BulkInsertLicenseRecordHistory(licenseHistoryRecord);
                _logger.LogInformation("Finished inserting License Record Histories into DB after {timeElapsedMS} ms", timer.ElapsedMilliseconds);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Failed while trying to InsertNewFromRawRecords");
                throw;
            }
        }

        public async Task<List<string>> GetDeletedRecords(int importID)
        {
            var timer = Stopwatch.StartNew();

            _logger.LogInformation("Starting to fetch Records from Import");
            var recordIdsFromImport = await _importJobRepo.GetAllLicenseIDsFromImport(importID);

            _logger.LogInformation("Fetched {importRecordsCount} records in {elapsedMs} ms.", recordIdsFromImport.Count(), timer.ElapsedMilliseconds);
            timer.Restart();

            _logger.LogInformation("Starting to fetch Excluded Records from Import");
            var recordIdsNotFromImport = await _importJobRepo.GetActiveLicensesNotFromImport(importID);

            _logger.LogInformation("Fetched {importRecordsCount} excluded records in {elapsedMs} ms.", recordIdsNotFromImport.Count(), timer.ElapsedMilliseconds);

            return recordIdsNotFromImport.Where(x => !recordIdsFromImport.Contains(x)).ToList();
        }

        public async Task InvalidateRecordsFromDB(List<string> recordIDs, int importId)
        {
            var timer = Stopwatch.StartNew();
            var versionInformation = await _taflRepo.GetVersionsForLicenseRecords(recordIDs);

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var licenseHistoryRecord = versionInformation.Select(x => CreateLicenseRecordHistory(x.Key, x.Value, importId, ChangeType.Removed)).ToList();

                _logger.LogInformation("Starting to Bulk Invalidate {bulkInvalidateCount} records.", versionInformation.Count());
                await _taflRepo.BulkInvalidateRecords(versionInformation.Keys.ToList());
                _logger.LogInformation("Finished bulk invalidating in {elapsedMS} ms.", timer.ElapsedMilliseconds);


                timer.Restart();
                _logger.LogInformation("Starting to Bulk insert Record History.");
                await _importJobRepo.BulkInsertLicenseRecordHistory(licenseHistoryRecord);
                _logger.LogInformation("Finished bulk insert Record History in {elapsedMS} ms.", timer.ElapsedMilliseconds);

                await transaction.CommitAsync();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private List<LicenseRecord> CreateUpdatedLicenseRecords(List<(int version, TAFLEntryRawRow row)> rows)
        {
            List<LicenseRecord> dbRecords = _mapper.Map<List<LicenseRecord>>(rows.Select(x => x.row));
            var lookupVersion = rows.ToDictionary(x => x.row.LicenseRecordID, x => x.version);

            foreach (var row in dbRecords)
            {
                row.Version = lookupVersion[row.CanadaLicenseRecordID];
            }

            return dbRecords;
        }

        private static LicenseRecordHistory CreateLicenseRecordHistory(string recordId, int recordVersion, int ImportHistoryID, ChangeType changeType)
        {
            return new()
            {
                CanadaLicenseRecordID = recordId,
                Version = recordVersion,
                EditedByImportJobID = ImportHistoryID,
                ChangeType = changeType
            };
        }
        private static LicenseRecordHistory CreateLicenseRecordHistory(LicenseRecord record, int ImportHistoryID, ChangeType changeType)
        {
            return CreateLicenseRecordHistory(record.CanadaLicenseRecordID, record.Version, ImportHistoryID, changeType);
        }
    }
}
