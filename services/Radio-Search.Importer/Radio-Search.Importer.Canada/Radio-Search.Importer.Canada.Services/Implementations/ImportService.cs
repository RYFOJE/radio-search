
using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Radio_Search.Importer.Canada.Data;
using Radio_Search.Importer.Canada.Data.Enums;
using Radio_Search.Importer.Canada.Data.Models.History;
using Radio_Search.Importer.Canada.Data.Models.License;
using Radio_Search.Importer.Canada.Data.Repositories.Interfaces;
using Radio_Search.Importer.Canada.Services.Configuration;
using Radio_Search.Importer.Canada.Services.Data;
using Radio_Search.Importer.Canada.Services.Data.Enums;
using Radio_Search.Importer.Canada.Services.Exceptions.TAFLDefinitionExceptions;
using Radio_Search.Importer.Canada.Services.Interfaces;
using Radio_Search.Importer.Canada.Services.Responses;
using Spire.Pdf;
using Spire.Pdf.Utilities;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;

namespace Radio_Search.Importer.Canada.Services.Implementations
{
    public class ImportService : IImportService
    {
        private const int MAX_FETCH_COUNT = 1000;
        private const int MAX_FETCH_ITERATIONS = 1000000;
        private const string TAFL_RAW_DATE_FORMAT = "yyyy-MM-dd";

        private readonly ILogger<ImportService> _logger;
        private readonly IConfiguration _config;
        private readonly TAFLDefinitionTablesOrder _taflDefinitionOrder;
        private readonly CanadaImporterContext _context;
        private readonly ITAFLDefinitionRepo _definitionRepo;
        private readonly ITAFLRepo _taflRepo;
        private readonly ITAFLImportHistoryRepo _historyRepo;
        private readonly IMapper _mapper;
        private readonly IPDFProcessingServices _pdfService;
        public readonly IValidator<TAFLEntryRawRow> _taflRawRowValidator;

        public ImportService(
                ILogger<ImportService> logger,
                IConfiguration config,
                IOptions<TAFLDefinitionTablesOrder> taflDefinitionOrder,
                CanadaImporterContext context,
                ITAFLDefinitionRepo definitionRepo,
                ITAFLRepo taflRepo,
                ITAFLImportHistoryRepo historyRepo,
                IMapper mapper,
                IPDFProcessingServices pdfService,
                IValidator<TAFLEntryRawRow> taflRawRowValidator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger is null");
            _config = config ?? throw new ArgumentNullException(nameof(config), "Config is null");
            _taflDefinitionOrder = taflDefinitionOrder.Value;
            _context = context ?? throw new ArgumentNullException(nameof(context), "Context is null");
            _definitionRepo = definitionRepo ?? throw new ArgumentNullException(nameof(definitionRepo), "definitionRepo is null");
            _taflRepo = taflRepo ?? throw new ArgumentNullException(nameof(taflRepo), "TAFLRepo is null");
            _historyRepo = historyRepo ?? throw new ArgumentNullException(nameof(historyRepo), "HistoryRepo is null");
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper), "Mapper is null");
            _pdfService = pdfService ?? throw new ArgumentNullException(nameof(pdfService), "pdfService is null");
            _taflRawRowValidator = taflRawRowValidator ?? throw new ArgumentNullException(nameof(taflRawRowValidator), "TaflRawRowValidator is null");
        }


        #region TAFL CSV IMPORT

        #region public
        
        /// <inheritdoc/>
        public async Task<BeginTAFLImportResponse> BeginTAFLImport(Stream stream)
        {
            ImportHistory? importRecord = null;
            try
            {
                var fileHash = GetStreamHash(stream);
                importRecord = await CreateImportHistoryRecord(fileHash);

                // CSV Parsing and extracting of raw rows
                var csvResult = ExtractTAFLRawRowsFromCSV(stream);
                int prevCount = csvResult.Data.Count;

                await stream.DisposeAsync();

                //Deduplicate
                csvResult.Data = DeduplicateRows(csvResult.Data);
                importRecord.SkippedRowCount += prevCount - csvResult.Data.Count();

                // Validate rows and skip invalid ones
                prevCount = csvResult.Data.Count;
                csvResult.Data = GetValidRows(csvResult.Data);
                importRecord.SkippedRowCount += prevCount - csvResult.Data.Count();

                importRecord.SkippedRowCount += csvResult.ColumnMismatchCount;
                importRecord.SkippedRowCount += csvResult.BadDataCount;
                

                importRecord = await _historyRepo.UpdateImportHistoryRecord(importRecord);

                // Check to see what to do with all records
                var actionResponse = await GetActionsPerRawRow(csvResult.Data);

                csvResult.Data = new();

                GC.Collect(); // TODO REMOVE THIS

                // Maybe add a sanity check for if action count total doesnt match total row count

                // Save records to DB
                var dbResponse = await SaveTAFLToDBAsync(actionResponse, importRecord.ImportHistoryID);
                

                // TODO: THROW HERE IF dbResponse fails!!!!!


                importRecord.Status = ImportStatus.Success;
                importRecord.TotalInsertedUpdatedRows = dbResponse.ModifiedRowCount;
                importRecord.EndTime = DateTime.UtcNow;
                await _historyRepo.UpdateImportHistoryRecord(importRecord);

                return new()
                {
                    Message = "Successfully imported the TAFL DB",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to BeginTAFLImport");

                if(importRecord != null)
                    await MarkImportAsFailed(importRecord.ImportHistoryID);

                return new()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        /// <inheritdoc/>
        public ProcessTAFLCsvResponse ExtractTAFLRawRowsFromCSV(Stream stream)
        {
            var stopwatch = Stopwatch.StartNew();

            List<TAFLEntryRawRow> records = [];
            int skippedRecords = 0;
            int columnMismatchCount = 0;

            int expectedColumnCount = GetPropCount<TAFLEntryRawRow>();

            using (var reader = new StreamReader(stream))
            using (var csv = CreateCSVReader(reader))
            {
                while (csv.Read())
                {
                    try
                    {
                        int fieldCount = csv.Parser.Count;
                        if (fieldCount != expectedColumnCount)
                        {
                            columnMismatchCount++;
                            _logger.LogWarning("Skipping row number {RowNumber}: Expected {ExpectedColumnCount} columns, found {ActualColumnCount}",
                                csv.Context.Parser?.Row, expectedColumnCount, fieldCount);
                            continue;
                        }

                        var currentRecord = csv.GetRecord<TAFLEntryRawRow>();
                        if (currentRecord != null)
                        {
                            records.Add(currentRecord);
                        }
                    }
                    catch (Exception ex)
                    {
                        skippedRecords++;
                        _logger.LogError(ex, "Error on row {row}", csv.Context.Parser?.Row);
                    }
                }
            }

            _logger.LogInformation("ExtractTAFLRawRowsFromCSV took {ElapsedTimeInMs} ms", stopwatch.ElapsedMilliseconds);

            return new ProcessTAFLCsvResponse
            {
                Success = true,
                ColumnMismatchCount = columnMismatchCount,
                BadDataCount = skippedRecords,
                Data = records
            };
        }

        /// <inheritdoc/>
        public async Task<GetActionsPerRawRowResponse> GetActionsPerRawRow(List<TAFLEntryRawRow> records)
        {
            var stopwatch = Stopwatch.StartNew();
            var dictionaryRecords = TAFLListToDictionary(records);

            var updatedDeletedResponse = await GetUpdatedDeletedRows(dictionaryRecords);
            var createdRows = GetCreatedRows(dictionaryRecords, updatedDeletedResponse);

            _logger.LogInformation("Finished GetActionsPerRawRow in {ElapsedTimeInMS} ms.", stopwatch.ElapsedMilliseconds);

            return new()
            {
                CreatedRawRows = createdRows,
                DeletedDBLicense = updatedDeletedResponse.DeletedRows,
                UpdatedRawRows = updatedDeletedResponse.UpdatedRows
            };
        }

        /// <inheritdoc/>
        public async Task<SaveTAFLToDBResponse> SaveTAFLToDBAsync(GetActionsPerRawRowResponse actionsPerRow, int importRecordID)
        {
            var stopwatch = Stopwatch.StartNew();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Create this here so we can collect all update records.
                List<LicenseRecordHistory> allLicenseRecords = [];

                // New Licenses
                var dbNewLicenses = _mapper.Map<List<LicenseRecord>>(actionsPerRow.CreatedRawRows,
                    opts => opts.Items["ImportHistoryID"] = importRecordID);

                actionsPerRow.CreatedRawRows = null;

                allLicenseRecords.AddRange(dbNewLicenses.Select(
                    x => CreateLicenseRecordHistory(x.InternalLicenseRecordID, importRecordID, ChangeType.Created))
                );
                _logger.LogInformation("Inserting {NewLicenseCount} new Licenses into the DB.", dbNewLicenses.Count());

                await _taflRepo.BulkAddLicenseRecordsAsync(dbNewLicenses);
                dbNewLicenses = null;


                // Updated Licenses
                _logger.LogInformation("Invalidating {UpdateLicenseCount} outdated Licenses", actionsPerRow.UpdatedRawRows.Count());
                await _taflRepo.BulkInvalidateRecords(actionsPerRow.UpdatedRawRows.Select(x => x.LicenseRecordID).ToList()); // This could be done better
                
                var dbUpdatedLicenses = _mapper.Map<List<LicenseRecord>>(actionsPerRow.UpdatedRawRows);
                allLicenseRecords.AddRange(dbUpdatedLicenses.Select(
                    x => CreateLicenseRecordHistory(x.InternalLicenseRecordID, importRecordID, ChangeType.Updated)));

                _logger.LogInformation("Inserting {UpdateLicenseCount} Updated Licenses", actionsPerRow.UpdatedRawRows.Count());
                await _taflRepo.BulkAddLicenseRecordsAsync(dbUpdatedLicenses);

                // Deleted Licenses
                _logger.LogInformation("Invalidating {UpdateLicenseCount} deleted Licenses", actionsPerRow.DeletedDBLicense.Count());

                await _taflRepo.BulkInvalidateRecords(actionsPerRow.DeletedDBLicense.Select(x => x.CanadaLicenseRecordID).ToList());
                allLicenseRecords.AddRange(actionsPerRow.DeletedDBLicense.Select(
                    x => CreateLicenseRecordHistory(x.InternalLicenseRecordID, importRecordID, ChangeType.Removed)));

                // License History
                _logger.LogInformation("Inserting {LicenseHistoryCount} License History Records", allLicenseRecords.Count());
                await _historyRepo.BulkInsertLicenseRecordHistory(allLicenseRecords);

                await transaction.CommitAsync();

                int modifiedCount = actionsPerRow.CreatedRawRows.Count();
                modifiedCount += actionsPerRow.UpdatedRawRows.Count();
                modifiedCount += actionsPerRow.DeletedDBLicense.Count();

                _logger.LogInformation("SaveTAFLToDBAsync took {ElapsedTimeInMs} ms", stopwatch.ElapsedMilliseconds);

                return new()
                {
                    Success = true,
                    Message = "Successfully saved changes to DB.",
                    ModifiedRowCount = modifiedCount // This wont include the LicenseRecordHistory coutn but I think this makes more sense.
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process SaveTAFLToDbAsync");
                
                await transaction.RollbackAsync();
                return new()
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ImportHistory> CreateImportHistoryRecord(byte[] fileHash)
        {
            var record = new ImportHistory()
            {
                StartTime = DateTime.UtcNow,
                Status = ImportStatus.Pending,
                FileHash = fileHash
            };

            try
            {
                return await _historyRepo.CreateImportHistoryRecord(record);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while CreateImportHistoryRecord");
                throw;
            }
        }

        /// <inheritdoc/>
        public Dictionary<string, TAFLEntryRawRow> TAFLListToDictionary(List<TAFLEntryRawRow> taflRows)
        {
            return taflRows.ToDictionary(row => row.LicenseRecordID);
        }

        /// <inheritdoc/>
        public List<TAFLEntryRawRow> GetUpdatedRowsFromRange(Dictionary<string, TAFLEntryRawRow> inputRows, List<LicenseRecord> dbRows)
        {
            List<TAFLEntryRawRow> toUpdate = [];

            foreach (var currDbRow in dbRows)
            {
                if (!inputRows.TryGetValue(currDbRow.CanadaLicenseRecordID, out var matchingRow))
                    continue;

                if (!matchingRow.Equals(currDbRow))
                    toUpdate.Add(matchingRow);
            }

            return toUpdate;
        }

        /// <inheritdoc/>
        public List<LicenseRecord> GetDeletedRowsFromRange(Dictionary<string, TAFLEntryRawRow> inputRows, List<LicenseRecord> dbRows)
        {
            List<TAFLEntryRawRow> toDelete = [];
            return dbRows.Where(x => !inputRows.ContainsKey(x.CanadaLicenseRecordID)).ToList();
        }

        /// <inheritdoc/>
        public HashSet<string> GetUnaffectedRowsFromRange(Dictionary<string, TAFLEntryRawRow> inputRows, List<LicenseRecord> dbRows)
        {
            HashSet<string> unaffectedRows = [];

            foreach (var currDbRow in dbRows)
            {
                if (!inputRows.TryGetValue(currDbRow.CanadaLicenseRecordID, out var matchingRow))
                    continue;

                if (matchingRow.Equals(currDbRow))
                    unaffectedRows.Add(matchingRow.LicenseRecordID);
            }

            return unaffectedRows;
        }


        /// <inheritdoc/>
        public List<TAFLEntryRawRow> GetCreatedRows(Dictionary<string, TAFLEntryRawRow> rawRows, GetAffectedRowsResponse affectedRowResponse)
        {
            HashSet<string> allSeenIDs = [];
            allSeenIDs.UnionWith(affectedRowResponse.UnaffectedRows);
            allSeenIDs.UnionWith(affectedRowResponse.UpdatedRows.Select(x => x.LicenseRecordID));
            allSeenIDs.UnionWith(affectedRowResponse.DeletedRows.Select(x => x.CanadaLicenseRecordID));

            return rawRows
                .Where(kvp => !allSeenIDs.Contains(kvp.Key))
                .Select(kvp => kvp.Value)
                .ToList();
        }

        /// <inheritdoc/>
        public async Task MarkImportAsFailed(int id)
        {
            try
            {
                var record = await _historyRepo.GetImportHistoryRecord(id);
                record.Status = ImportStatus.Failure;
                record.EndTime = DateTime.UtcNow;

                await _historyRepo.UpdateImportHistoryRecord(record);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while trying to MarkImportAsFailed for Record id: {id}", id);
            }
        }

        /// <summary>
        /// Removes duplicate rows from the provided list based on the LicenseRecordID, retaining the row with the most
        /// recent InServiceDate.
        /// </summary>
        /// <remarks>If the InServiceDate cannot be parsed for a row, that row is ignored unless it is the
        /// only entry for its LicenseRecordID.</remarks>
        /// <param name="rows">A list of <see cref="TAFLEntryRawRow"/> objects to be deduplicated.</param>
        /// <returns>A list of <see cref="TAFLEntryRawRow"/> objects with duplicates removed, keeping the row with the latest
        /// InServiceDate for each LicenseRecordID.</returns>
        public List<TAFLEntryRawRow> DeduplicateRows(List<TAFLEntryRawRow> rows)
        {
            Dictionary<string, TAFLEntryRawRow> uniqueRawRows = new();

            foreach(var row in rows)
            {
                if(!uniqueRawRows.ContainsKey(row.LicenseRecordID))
                {
                    uniqueRawRows[row.LicenseRecordID] = row;
                    continue;
                }

                var previousRow = uniqueRawRows[row.LicenseRecordID];

                if (previousRow.InServiceDate == null)
                    uniqueRawRows[row.LicenseRecordID] = row;

                if (row.InServiceDate == null)
                    continue;

                if (previousRow.InServiceDate > row.InServiceDate)
                    continue;

                uniqueRawRows[row.LicenseRecordID] = row;
            }

            return uniqueRawRows.Values.ToList();
        }

        /// <summary>
        /// Filters and returns a list of valid TAFLEntryRawRow objects from the provided collection.
        /// </summary>
        /// <remarks>This method uses an internal validator to determine the validity of each row. Only
        /// rows that pass the validation are included in the returned list.</remarks>
        /// <param name="rows">A list of <see cref="TAFLEntryRawRow"/> objects to be validated.</param>
        /// <returns>A list of <see cref="TAFLEntryRawRow"/> objects that are valid according to the validator.</returns>
        public List<TAFLEntryRawRow> GetValidRows(List<TAFLEntryRawRow> rows)
        {
            var validRows = new List<TAFLEntryRawRow>();
            foreach (var row in rows)
            {
                var result = _taflRawRowValidator.Validate(row);
                if (result.IsValid)
                {
                    validRows.Add(row);
                }
                else
                {
                    var errors = string.Join("; ", result.Errors.Select(e => $"Property: {e.PropertyName}, Error: {e.ErrorMessage}"));
                    _logger.LogDebug("TAFL row invalid. LicenseRecordID: {LicenseRecordID}, Errors: {Errors}, Row: {@Row}",
                        row.LicenseRecordID, errors, row);
                }
            }
            return validRows;
        }

        #endregion

        #region private

        private static byte[] GetStreamHash(Stream stream)
        {
            using var sha256 = SHA256.Create();

            if (stream.CanSeek)
                stream.Position = 0;

            byte[] hashBytes = sha256.ComputeHash(stream);

            stream.Position = 0;

            return hashBytes;
        }

        private async Task<GetAffectedRowsResponse> GetUpdatedDeletedRows(Dictionary<string, TAFLEntryRawRow> newRows)
        {
            var stopwatch = Stopwatch.StartNew();

            int currIteration = 0;
            List<LicenseRecord> rowsFetchedFromDB;

            GetAffectedRowsResponse response = new();

            do
            {
                int fetchOffset = MAX_FETCH_COUNT * currIteration;
                rowsFetchedFromDB = await _taflRepo.GetRecordsNoTrackingAsync(fetchOffset, MAX_FETCH_COUNT);

                response.UpdatedRows.AddRange(GetUpdatedRowsFromRange(newRows, rowsFetchedFromDB));
                response.DeletedRows.AddRange(GetDeletedRowsFromRange(newRows, rowsFetchedFromDB));
                response.UnaffectedRows.UnionWith(GetUnaffectedRowsFromRange(newRows, rowsFetchedFromDB));

                if (currIteration == MAX_FETCH_ITERATIONS)
                {
                    _logger.LogError("Failed on CheckUpdatedDeletedRows as the function surpassed the fetch iteration limit of {FetchIterationLimit}", MAX_FETCH_ITERATIONS);
                    throw new InvalidOperationException("Maximum fetch iterations reached while checking for updated or deleted rows. This may indicate an unexpectedly large data set or a logic error.");
                }
            } while (rowsFetchedFromDB.Count == MAX_FETCH_COUNT);

            stopwatch.Stop();
            _logger.LogInformation("CheckUpdatedDeletedRows executed in {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);

            return response;
        }

        private static LicenseRecordHistory CreateLicenseRecordHistory(Guid InternalLicenseID, int ImportHistoryID, ChangeType changeType)
        {
            return new()
            {
                InternalLicenseRecordID = InternalLicenseID,
                EditedByImportHistoryRecordID = ImportHistoryID,
                ChangeType = changeType
            };
        }

        #endregion

        #endregion

        #region TAFL DEFINITION IMPORT

        #region public

        /// <inheritdoc/>
        public ProcessTAFLDefinitionResponse ProcessTAFLDefinition(Stream multiPageStream)
        {
            Dictionary<TAFLDefinitionTableEnum, HashSet<TAFLDefinitionRawRow>> parsedTables = [];

            try
            {
                var singlePageStream = _pdfService.MergePDFToSinglePage(multiPageStream);

                PdfDocument pdf = new();
                pdf.LoadFromStream(singlePageStream);
                var extractor = new PdfTableExtractor(pdf);

                List<PdfTable> rawTables = [];

                // Extract all the Tables from the PDF, in correct order
                for (int pageIndex = 0; pageIndex < pdf.Pages.Count; pageIndex++)
                {
                    PdfTable[] tablesForPage = extractor.ExtractTable(pageIndex);
                    if (tablesForPage == null || tablesForPage.Length == 0)
                        continue;

                    rawTables.AddRange(tablesForPage);
                }

                // Check to see if all the tables have been found
                if (rawTables.Count != _taflDefinitionOrder.TableOrder.Count)
                    throw new InvalidOperationException("Table Order Count does not match total amount of tables found.");

                // Iterate through every table and parse it out
                for (int i = 0; i < _taflDefinitionOrder.TableEnumOrder.Count; i++)
                {
                    var currTableEnum = _taflDefinitionOrder.TableEnumOrder[i];

                    if (currTableEnum == TAFLDefinitionTableEnum.Skip)
                    {
                        _logger.LogInformation("Skipping table at index={tableIndex}", i);
                        continue;
                    }

                    ThrowIfTableInvalid(rawTables[i]);

                    // Get or create that tables HashSet for the given enum
                    if (!parsedTables.TryGetValue(currTableEnum, out var tableInOutputList))
                    {
                        tableInOutputList = new();
                        parsedTables[currTableEnum] = tableInOutputList;
                    }

                    var rows = GetAllRows(rawTables[i]);

                    tableInOutputList.UnionWith(rows);
                }
            }
            catch (TAFLDefinitionException tfEx)
            {
                _logger.LogError(tfEx, "One or more tables do not match expected format.");
                return new()
                {
                    Success = false,
                    Message = tfEx.Message
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ProcessTAFLDefinition failed");

                return new()
                {
                    Success = false,
                    Message = ex.Message
                };
            }

            return new()
            {
                Success = true,
                Message = "Successfully parsed TAFL Definition File",
                Tables = parsedTables
            };

        }

        /// <inheritdoc/>
        public async Task<SaveTAFLDefinitionToDBResponse> SaveTAFLDefinitionToDBAsync(Dictionary<TAFLDefinitionTableEnum, HashSet<TAFLDefinitionRawRow>> tables)
        {
            int totalModifiedRowCount = 0;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try {


                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, StationFunctionType>(
                    tables,
                    TAFLDefinitionTableEnum.StationFunction,
                    _definitionRepo.StationFunctionAddUpdate,
                    nameof(TAFLDefinitionTableEnum.StationFunction)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, RegulatoryService>(
                    tables,
                    TAFLDefinitionTableEnum.RegulatoryService,
                    _definitionRepo.RegulatoryServiceAddUpdate,
                    nameof(TAFLDefinitionTableEnum.RegulatoryService)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, CommunicationType>(
                    tables,
                    TAFLDefinitionTableEnum.CommunicationType,
                    _definitionRepo.CommunicationTypeAddUpdate,
                    nameof(TAFLDefinitionTableEnum.CommunicationType)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, ConformityFrequencyPlan>(
                    tables,
                    TAFLDefinitionTableEnum.ConformityToFrequencyPlan,
                    _definitionRepo.ConformityToFrequencyPlanAddUpdate,
                    nameof(TAFLDefinitionTableEnum.ConformityToFrequencyPlan)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, AnalogDigital>(
                    tables,
                    TAFLDefinitionTableEnum.AnalogDigital,
                    _definitionRepo.AnalogDigitalAddUpdate,
                    nameof(TAFLDefinitionTableEnum.AnalogDigital)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, ModulationType>(
                    tables,
                    TAFLDefinitionTableEnum.ModulationType,
                    _definitionRepo.ModulationTypeAddUpdate,
                    nameof(TAFLDefinitionTableEnum.ModulationType)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, FiltrationInstalledType>(
                    tables,
                    TAFLDefinitionTableEnum.FiltrationInstalledType,
                    _definitionRepo.FiltrationTypeAddUpdate,
                    nameof(TAFLDefinitionTableEnum.FiltrationInstalledType)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, AntennaPattern>(
                    tables,
                    TAFLDefinitionTableEnum.AntennaPattern,
                    _definitionRepo.AntennaPatternAddUpdate,
                    nameof(TAFLDefinitionTableEnum.AntennaPattern)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, PolarizationType>(
                    tables,
                    TAFLDefinitionTableEnum.Polarization,
                    _definitionRepo.PolarizationAddUpdate,
                    nameof(TAFLDefinitionTableEnum.Polarization)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, StationType>(
                    tables,
                    TAFLDefinitionTableEnum.TypeOfStation,
                    _definitionRepo.TypeOfStationAddUpdate,
                    nameof(TAFLDefinitionTableEnum.TypeOfStation)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, ITUClassType>(
                    tables,
                    TAFLDefinitionTableEnum.ITUClass,
                    _definitionRepo.ITUClassAddUpdate,
                    nameof(TAFLDefinitionTableEnum.ITUClass)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, StationCostCategory>(
                    tables,
                    TAFLDefinitionTableEnum.StationCostCategory,
                    _definitionRepo.StationCostCategoryAddUpdate,
                    nameof(TAFLDefinitionTableEnum.StationCostCategory)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, Province>(
                    tables,
                    TAFLDefinitionTableEnum.Provinces,
                    _definitionRepo.ProvincesAddUpdate,
                    nameof(TAFLDefinitionTableEnum.Provinces)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, CongestionZoneType>(
                    tables,
                    TAFLDefinitionTableEnum.CongestionZone,
                    _definitionRepo.CongestionZoneAddUpdate,
                    nameof(TAFLDefinitionTableEnum.CongestionZone)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, ServiceType>(
                    tables,
                    TAFLDefinitionTableEnum.Service,
                    _definitionRepo.ServiceAddUpdate,
                    nameof(TAFLDefinitionTableEnum.Service)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, SubserviceType>(
                    tables,
                    TAFLDefinitionTableEnum.Subservice,
                    _definitionRepo.SubserviceAddUpdate,
                    nameof(TAFLDefinitionTableEnum.Subservice)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, LicenseType>(
                    tables,
                    TAFLDefinitionTableEnum.LicenseType,
                    _definitionRepo.LicenseTypeAddUpdate,
                    nameof(TAFLDefinitionTableEnum.LicenseType)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, AuthorizationStatus>(
                    tables,
                    TAFLDefinitionTableEnum.AuthorizationStatus,
                    _definitionRepo.AuthorizationStatusAddUpdate,
                    nameof(TAFLDefinitionTableEnum.AuthorizationStatus)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, OperationalStatus>(
                    tables,
                    TAFLDefinitionTableEnum.OperationalStatus,
                    _definitionRepo.OperationalStatusAddUpdate,
                    nameof(TAFLDefinitionTableEnum.OperationalStatus)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, StationClass>(
                    tables,
                    TAFLDefinitionTableEnum.StationClass,
                    _definitionRepo.StationClassAddUpdate,
                    nameof(TAFLDefinitionTableEnum.StationClass)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, StandbyTransmitterInformation>(
                    tables,
                    TAFLDefinitionTableEnum.StandbyTransmitterInformation,
                    _definitionRepo.StandbyTransmitterInformationAddUpdate,
                    nameof(TAFLDefinitionTableEnum.StandbyTransmitterInformation)
                );

            }
            catch(Exception ex) {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Failed while processing SaveTAFLDefinitionToDBAsync");

                return new()
                {
                    Success = false,
                    Message = ex.Message
                };
            }

            await transaction.CommitAsync();

            return new SaveTAFLDefinitionToDBResponse
            {
                Success = true,
                ModifiedRowCount = totalModifiedRowCount,
                Message = "Successfully saved all TAFL definitions to the database."
            };
        }

        #endregion

        #region private
        /// <summary>
        /// Creates a CSV Reader Object from the given stream
        /// </summary>
        /// <param name="stream">Stream to parse</param>
        /// <returns>CSVReader for the given stream</returns>
        private CsvReader CreateCSVReader(StreamReader stream)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                BadDataFound = context =>
                {
                    _logger.LogWarning("Bad data found while processing CSV Entry={entry}", context.RawRecord);
                },
                ReadingExceptionOccurred = context =>
                {
                    _logger.LogError(context.Exception, "Error on row {rowNumber}", context.Exception.Data["CsvHelper.RowNumber"]);
                    return true;
                }
            };


            var csv = new CsvReader(stream, config);
            csv.Context.TypeConverterCache.AddConverter<decimal?>(new NullConverter<decimal?>(csv.Context.TypeConverterCache));
            csv.Context.TypeConverterCache.AddConverter<int?>(new NullConverter<int?>(csv.Context.TypeConverterCache));
            csv.Context.TypeConverterCache.AddConverter<double?>(new NullConverter<double?>(csv.Context.TypeConverterCache));
            csv.Context.TypeConverterCache.AddConverter<string?>(new NullConverter<string>(csv.Context.TypeConverterCache));

            return csv;
        }

        /// <summary>
        /// Checks to see if the Table is valid for further processing. Throws if invalid.
        /// </summary>
        /// <param name="table">PDFTable to be verified</param>
        /// <exception cref="TAFLDefinitionHeaderCountException">If the headers don't match the expected count of headers</exception>
        /// <exception cref="TAFLDefinitionHeaderMismatchException">If the headers don't match the expected headers</exception>
        private void ThrowIfTableInvalid(PdfTable table)
        {
            int cols = table.GetColumnCount();

            if (cols != _taflDefinitionOrder.Headers.Count)
                throw new TAFLDefinitionHeaderCountException(_taflDefinitionOrder.Headers.Count, cols);

            for (int i = 0; i < _taflDefinitionOrder.Headers.Count; i++)
            {
                var headerColumn = table.GetText(0, i).Trim();

                if (!string.Equals(headerColumn, _taflDefinitionOrder.Headers[i], StringComparison.InvariantCultureIgnoreCase))
                    throw new TAFLDefinitionHeaderMismatchException(headerColumn, _taflDefinitionOrder.Headers[i]);
            }
        }

        /// <summary>
        /// Gets all the rows for a given table.
        /// <remarks>Since <seealso cref="TAFLDefinitionRawRow"/> expects 3 rows, the PdfTable must only have 3 columns otherwise this will throw</remarks>
        /// </summary>
        /// <param name="table">The table to parse</param>
        /// <param name="ignoreHeader">Skips the first row.</param>
        /// <returns>List of TAFLDefinitionRawRow's</returns>
        /// <exception cref="InvalidDataException">If that data is empty, throw these exceptions</exception>
        private static List<TAFLDefinitionRawRow> GetAllRows(PdfTable table, bool ignoreHeader = true)
        {
            int curRow = ignoreHeader ? 1 : 0;
            int totalRows = table.GetRowCount();

            List<TAFLDefinitionRawRow> parsedRows = new(totalRows - curRow);

            for (; curRow < totalRows; curRow++)
            {
                string code = table.GetText(curRow, 0);
                string englishDefinition = table.GetText(curRow, 1);
                string frenchDefinition = table.GetText(curRow, 2);

                if (string.IsNullOrWhiteSpace(code))
                    throw new InvalidDataException($"Code column is empty at row {curRow}.");
                if (string.IsNullOrWhiteSpace(englishDefinition))
                    throw new InvalidDataException($"EnglishDefinition column is empty at row {curRow}.");
                if (string.IsNullOrWhiteSpace(frenchDefinition))
                    throw new InvalidDataException($"FrenchDefinition column is empty at row {curRow}.");

                TAFLDefinitionRawRow parseRow = new()
                {
                    Code = code,
                    DescriptionEN = englishDefinition,
                    DescriptionFR = frenchDefinition,
                };
                parsedRows.Add(parseRow);
            }

            return parsedRows;
        }

        /// <summary>
        /// Gets the total counts of properties in a given class
        /// </summary>
        /// <typeparam name="T">Type to check prop count</typeparam>
        /// <returns></returns>
        private static int GetPropCount<T>() where T : class
        {
            var type = typeof(T);

            // Get all public instance properties
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            return properties.Length;
        }

        /// <summary>
        /// Calls a repo function to add/update rows and returns the total amount of modifications/inserts
        /// </summary>
        /// <typeparam name="TTableEnum">Enum Type for representing tables</typeparam>
        /// <typeparam name="TRow">The data used for inserting into the table</typeparam>
        /// <typeparam name="TEntity">The database's expected model type</typeparam>
        /// <param name="tables">All the tables values, this is for every table</param>
        /// <param name="tableKey">The lookup key for the table Dictionary representing which table we're inserting for</param>
        /// <param name="repoAddUpdate">The repo method for inserting/updating records</param>
        /// <param name="logName">The name of the table in the given logs</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private async Task<int> ProcessTableDefinitionAsync<TTableEnum, TRow, TEntity>(
            Dictionary<TTableEnum, HashSet<TRow>> tables,
            TTableEnum tableKey,
            Func<List<TEntity>, Task<int>> repoAddUpdate,
            string logName)
            where TTableEnum : notnull
        {
            if (!tables.TryGetValue(tableKey, out var rows) || rows == null || rows.Count == 0)
            {
                throw new InvalidOperationException($"{logName} is null or empty");
            }

            var mappedRows = _mapper.Map<List<TEntity>>(rows);

            _logger.LogInformation("Starting to check {LogName} repo with {RowCount} rows", logName, rows.Count);
            var modifiedCount = await repoAddUpdate(mappedRows);
            _logger.LogInformation("Ended check {LogName} repo with {ModifiedCount} modified rows", logName, modifiedCount);

            return modifiedCount;
        }

        #endregion

        #endregion

    }
}
