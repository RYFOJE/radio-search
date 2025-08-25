using CsvHelper;
using CsvHelper.Configuration;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Radio_Search.Importer.Canada.Data.Models.License;
using Radio_Search.Importer.Canada.Data.Repositories.Interfaces;
using Radio_Search.Importer.Canada.Services.Data;
using Radio_Search.Importer.Canada.Services.Interfaces.TAFLImport;
using Radio_Search.Importer.Canada.Services.Responses;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace Radio_Search.Importer.Canada.Services.Implementations.TAFLImport
{
    public class PreprocessingService : IPreprocessingService
    {
        private readonly ILogger<PreprocessingService> _logger;
        private readonly IValidator<TaflEntryRawRow> _taflRawRowValidator;
        private readonly ITAFLDefinitionRepo _definitionRepo;

        public PreprocessingService(
            ILogger<PreprocessingService> logger,
            IValidator<TaflEntryRawRow> taflRawRowValidator,
            ITAFLDefinitionRepo definitionRepo) 
        { 
            _logger = logger;
            _taflRawRowValidator = taflRawRowValidator;
            _definitionRepo = definitionRepo;
        }

        public List<TaflEntryRawRow> DeduplicateFullFile(Stream fullTAFLStream)
        {
            int initialCount = 0;

            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Starting deduplicate full tafl full file");
            var unprocessedRows = ExtractTAFLRawRowsFromCSV(fullTAFLStream);

            initialCount = unprocessedRows.Count;
            _logger.LogInformation("Finished extracting TAFL Raw Rows. There were {RowCount} rows. Processed in {ElapsedMs} ms.", initialCount, timer.ElapsedMilliseconds);
            timer.Restart();

            unprocessedRows = DeduplicateRows(unprocessedRows);
            _logger.LogInformation("Finished deduplicating TAFL Raw Rows. There were {DuplicateRowCount} duplicate rows. Processed in {ElapsedMs} ms.", initialCount - unprocessedRows.Count, timer.ElapsedMilliseconds);

            return unprocessedRows;
        }


        /// <inheritdoc/>
        public Stream GenerateChunkFile(List<TaflEntryRawRow> rows)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            var csv = new CsvWriter(sw, CultureInfo.InvariantCulture);

            csv.WriteRecords(rows);
            sw.Flush();
            ms.Position = 0;

            return ms;
        }

        public async Task<GetValidRawRowsResponse> GetValidRawRows(Stream fileStream)
        {
            var timer = Stopwatch.StartNew();
            int initialCount = 0;

            _logger.LogInformation("Starting to ExtractTAFLRawRowsFromCSV");
            var unprocessedRows = ExtractTAFLRawRowsFromCSV(fileStream);
            initialCount = unprocessedRows.Count;

            _logger.LogInformation("Finished ExtractTAFLRawRowsFromCSV with a RAW count of {RawRowCount}. Processed in {ElapsedMs} ms.", initialCount, timer.ElapsedMilliseconds);
            timer.Restart();

            _logger.LogInformation("Starting to deduplicate records.");
            unprocessedRows = DeduplicateRows(unprocessedRows);
            _logger.LogInformation("Finished deduplicating record. There are currently {RowCount} records after deduplicating. With {DuplicateCount} duplicates."
                , unprocessedRows.Count, initialCount - unprocessedRows.Count);

            int beforeValidationCount = unprocessedRows.Count;
            var validRows = await FluentValidateRecords(unprocessedRows);
            
            _logger.LogInformation("Finished validation. Valid row count: {ValidRowCount}, Invalid row count: {InvalidRowCount}",
                validRows.Count, beforeValidationCount - validRows.Count);

            return new GetValidRawRowsResponse
            {
                ValidRows = validRows,
                InvalidRowCount = unprocessedRows.Count - validRows.Count
            };
        }

        /// <summary>
        /// Removes duplicate rows from the provided list based on the LicenseRecordID, retaining the row with the most
        /// recent InServiceDate.
        /// </summary>
        /// <remarks>If the InServiceDate cannot be parsed for a row, that row is ignored unless it is the
        /// only entry for its LicenseRecordID.</remarks>
        /// <param name="rows">A list of <see cref="TaflEntryRawRow"/> objects to be deduplicated.</param>
        /// <returns>A list of <see cref="TaflEntryRawRow"/> objects with duplicates removed, keeping the row with the latest
        /// InServiceDate for each LicenseRecordID.</returns>
        public static List<TaflEntryRawRow> DeduplicateRows(List<TaflEntryRawRow> rows)
        {
            Dictionary<int, TaflEntryRawRow> uniqueRawRows = new();

            foreach (var row in rows)
            {
                if (!uniqueRawRows.ContainsKey(row.LicenseRecordID))
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
        /// Extracts a list of raw TAFL entry rows from a CSV stream.
        /// </summary>
        /// <remarks>This method reads the CSV data from the provided stream and attempts to parse each
        /// row into a <see cref="TaflEntryRawRow"/> object. Rows with a column count that does not match the expected
        /// number of columns for <see cref="TaflEntryRawRow"/> are skipped. If an error occurs while processing a row,
        /// the row is skipped, and the error is logged.</remarks>
        /// <param name="stream">The input stream containing the CSV data. The stream must be readable and positioned at the beginning of the
        /// CSV content.</param>
        /// <returns>A list of <see cref="TaflEntryRawRow"/> objects parsed from the CSV data.  The list will be empty if no
        /// valid rows are found.</returns>
        public List<TaflEntryRawRow> ExtractTAFLRawRowsFromCSV(Stream stream)
        {
            var stopwatch = Stopwatch.StartNew();

            List<TaflEntryRawRow> records = [];
            int skippedRecords = 0;
            int columnMismatchCount = 0;

            int expectedColumnCount = GetPropCount<TaflEntryRawRow>();

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

                        var currentRecord = csv.GetRecord<TaflEntryRawRow>();
                        if (currentRecord != null)
                        {
                            records.Add(currentRecord);
                        }
                    }
                    catch
                    {
                        skippedRecords++;
                        _logger.LogDebug("Error on row {Row}", csv.Context.Parser?.Row);
                    }
                }
            }

             _logger.LogInformation("ExtractTAFLRawRowsFromCSV took {ElapsedMs} ms", stopwatch.ElapsedMilliseconds);

            return records;
        }

        /// <summary>
        /// Filters and validates a list of raw TAFL entry rows, returning only the valid rows.
        /// </summary>
        /// <remarks>This method performs the following operations: <list type="bullet">
        /// <item><description>Deduplicates the input rows to ensure no duplicate entries are
        /// processed.</description></item> <item><description>Validates each row using a FluentValidation validator.
        /// Rows that fail validation are removed from the list, and detailed validation errors are logged for debugging
        /// purposes.</description></item> </list> Note that additional validation logic, such as checking against
        /// database definitions, may be implemented in the future.</remarks>
        /// <param name="rows">The list of raw TAFL entry rows to validate.</param>
        /// <returns>A list of <see cref="TaflEntryRawRow"/> objects that have passed all validation checks.</returns>
        public async Task<List<TaflEntryRawRow>> FluentValidateRecords(List<TaflEntryRawRow> rows)
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Starting to fetch all TAFL Definition Rows");
            var allDefinitions = await GetAllDefinitionRows();
            _logger.LogInformation("Finished fetching all TAFL Definition Rows in {ElapsedMs} ms.", timer.ElapsedMilliseconds);


            // Check with FluentValidation
            for (int i = 0; i < rows.Count; i++)
            {
                var context = new ValidationContext<TaflEntryRawRow>(rows[i]);
                context.RootContextData["AllDefinitions"] = allDefinitions;
                var result = _taflRawRowValidator.Validate(context);

                if (result.IsValid)
                    continue;

                var errors = string.Join("; ", result.Errors.Select(e => $"Property: {e.PropertyName}, Error: {e.ErrorMessage}"));
                _logger.LogDebug("TAFL row invalid. LicenseRecordID: {LicenseRecordID}, Errors: {Errors}, Row: {@Row}",
                    rows[i].LicenseRecordID, errors, rows[i]);

                rows.RemoveAt(i);
                i--;
            }

            // Check to see if all Definitions exist

            // TODO: Implement checking against what the DB has like rows
#warning ADD LOGIC FOR TYPE VALIDATION

            return rows;
        }

        /// <summary>
        /// Creates a CSV Reader Object from the given stream
        /// </summary>
        /// <param name="stream">Stream to parse</param>
        /// <returns>CSVReader for the given stream</returns>
        private CsvReader CreateCSVReader(StreamReader stream)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                BadDataFound = context =>
                {
                    _logger.LogWarning("Bad data found while processing CSV Entry={Entry}", context.RawRecord);
                },
                ReadingExceptionOccurred = context =>
                {
                    _logger.LogDebug("Error on row {RowNumber}", context.Exception.Data["CsvHelper.RowNumber"]);
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


        public async Task<TAFLDefinitionAllDbRows> GetAllDefinitionRows()
        {
            var definitions = new TAFLDefinitionAllDbRows
            {
                StandbyTransmitterInfos = (await _definitionRepo.GetAllRowsNoTracking<StandbyTransmitterInformation>()).Select(x => x.StandbyTransmitterInformationID).ToHashSet(),
                RegulatoryServices = (await _definitionRepo.GetAllRowsNoTracking<RegulatoryService>()).Select(x => x.RegulatoryServiceID).ToHashSet(),
                CommunicationTypes = (await _definitionRepo.GetAllRowsNoTracking<CommunicationType>()).Select(x => x.CommunicationTypeID).ToHashSet(),
                ConformityToFrequencyPlans = (await _definitionRepo.GetAllRowsNoTracking<ConformityFrequencyPlan>()).Select(x => x.ConformityFrequencyPlanID).ToHashSet(),
                OperationalStatuses = (await _definitionRepo.GetAllRowsNoTracking<OperationalStatus>()).Select(x => x.OperationalStatusID).ToHashSet(),
                StationClasses = (await _definitionRepo.GetAllRowsNoTracking<StationClass>()).Select(x => x.StationClassID).ToHashSet(),
                Services = (await _definitionRepo.GetAllRowsNoTracking<ServiceType>()).Select(x => x.ServiceTypeID).ToHashSet(),
                Subservices = (await _definitionRepo.GetAllRowsNoTracking<SubserviceType>()).Select(x => x.SubserviceTypeID).ToHashSet(),
                LicenseTypes = (await _definitionRepo.GetAllRowsNoTracking<LicenseType>()).Select(x => x.LicenseTypeID).ToHashSet(),
                AuthorizationStatuses = (await _definitionRepo.GetAllRowsNoTracking<AuthorizationStatus>()).Select(x => x.AuthorizationStatusID).ToHashSet(),
                CongestionZones = (await _definitionRepo.GetAllRowsNoTracking<CongestionZoneType>()).Select(x => x.CongestionZoneTypeID).ToHashSet(),
                StationTypes = (await _definitionRepo.GetAllRowsNoTracking<StationType>()).Select(x => x.StationTypeID).ToHashSet(),
                ITUClassOfStations = (await _definitionRepo.GetAllRowsNoTracking<ITUClassType>()).Select(x => x.ITUClassTypeID).ToHashSet(),
                StationCostCategories = (await _definitionRepo.GetAllRowsNoTracking<StationCostCategory>()).Select(x => x.StationCostCategoryID).ToHashSet(),
                Provinces = (await _definitionRepo.GetAllRowsNoTracking<Province>()).Select(x => x.ProvinceID).ToHashSet(),
                Polarizations = (await _definitionRepo.GetAllRowsNoTracking<PolarizationType>()).Select(x => x.PolarizationTypeID).ToHashSet(),
                AntennaPatterns = (await _definitionRepo.GetAllRowsNoTracking<AntennaPattern>()).Select(x => x.AntennaPatternID).ToHashSet(),
                ModulationTypes = (await _definitionRepo.GetAllRowsNoTracking<ModulationType>()).Select(x => x.ModulationTypeID).ToHashSet(),
                FiltrationInstalledTypes = (await _definitionRepo.GetAllRowsNoTracking<FiltrationInstalledType>()).Select(x => x.FiltrationInstalledTypeID).ToHashSet(),
                AnalogDigitals = (await _definitionRepo.GetAllRowsNoTracking<AnalogDigital>()).Select(x => x.AnalogDigitalID).ToHashSet(),
                StationFunctions = (await _definitionRepo.GetAllRowsNoTracking<StationFunctionType>()).Select(x => x.StationFunctionTypeID).ToHashSet(),
            };
            return definitions;
        }
    }
}
