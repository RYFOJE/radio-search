using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Radio_Search.Importer.Canada.Data;
using Radio_Search.Importer.Canada.Data.Models.License;
using Radio_Search.Importer.Canada.Data.Repositories.Interfaces;
using Radio_Search.Importer.Canada.Services.Configuration;
using Radio_Search.Importer.Canada.Services.Data;
using Radio_Search.Importer.Canada.Services.Data.Enums;
using Radio_Search.Importer.Canada.Services.Exceptions;
using Radio_Search.Importer.Canada.Services.Exceptions.TAFLDefinitionExceptions;
using Radio_Search.Importer.Canada.Services.Interfaces;
using Radio_Search.Importer.Canada.Services.Responses;
using Spire.Pdf;
using Spire.Pdf.Utilities;
using System.Globalization;
using System.Reflection;

namespace Radio_Search.Importer.Canada.Services.Implementations
{
    public class ImportService : IImportService
    {
        private readonly ILogger<ImportService> _logger;
        private readonly IConfiguration _config;
        private readonly TAFLDefinitionTablesOrder _taflDefinitionOrder;
        private readonly CanadaImporterContext _context;
        private readonly ITAFLDefinitionRepo _definitionRepo;
        private readonly IMapper _mapper;
        private readonly IPDFProcessingServices _pdfService;

        public ImportService(
                ILogger<ImportService> logger,
                IConfiguration config,
                IOptions<TAFLDefinitionTablesOrder> taflDefinitionOrder,
                CanadaImporterContext context,
                ITAFLDefinitionRepo definitionRepo,
                IMapper mapper,
                IPDFProcessingServices pdfService)
        {
            _logger = logger;
            _config = config;
            _taflDefinitionOrder = taflDefinitionOrder.Value;
            _context = context;
            _definitionRepo = definitionRepo;
            _mapper = mapper;
            _pdfService = pdfService;
        }

        /// <inheritdoc/>
        public ProcessTAFLCsvResponse ProcessTAFLCsv(Stream stream)
        {
            List<BroadcastAuthorizationRecord> records = new();
            int skippedRecords = 0;
            int columnMismatchCount = 0;

            int expectedColumnCount = GetPropCount<BroadcastAuthorizationRecord>();

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

                        var currentRecord = csv.GetRecord<BroadcastAuthorizationRecord>();
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

            return new ProcessTAFLCsvResponse
            {
                Success = true,
                ColumnMismatchCount = columnMismatchCount,
                BadDataCount = skippedRecords,
                Data = records
            };
        }

        /// <inheritdoc/>
        public ProcessTAFLDefinitionResponse ProcessTAFLDefinition(Stream multiPageStream)
        {
            Dictionary<TableDefinitions, HashSet<TableDefinitionRow>> parsedTables = new();

            try
            {
                var singlePageStream = _pdfService.MergePDFToSinglePage(multiPageStream);

                PdfDocument pdf = new PdfDocument();
                pdf.LoadFromStream(singlePageStream);
                var extractor = new PdfTableExtractor(pdf);

                List<PdfTable> rawTables = new();

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

                    if (currTableEnum == TableDefinitions.Skip)
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
        public async Task<SaveTAFLToDBResponse> SaveTAFLToDBAsync(List<BroadcastAuthorizationRecord> records)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<SaveTAFLDefinitionToDBResponse> SaveTAFLDefinitionToDBAsync(Dictionary<TableDefinitions, HashSet<TableDefinitionRow>> tables)
        {
            int currIncrease = 0;
            int totalModifiedRowCount = 0;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try {


                totalModifiedRowCount += await ProcessTableDefinitionAsync<TableDefinitions, TableDefinitionRow, StationFunctionType>(
                    tables,
                    TableDefinitions.StationFunction,
                    _definitionRepo.StationFunctionAddUpdate,
                    nameof(TableDefinitions.StationFunction)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TableDefinitions, TableDefinitionRow, RegulatoryService>(
                    tables,
                    TableDefinitions.RegulatoryService,
                    _definitionRepo.RegulatoryServiceAddUpdate,
                    nameof(TableDefinitions.RegulatoryService)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TableDefinitions, TableDefinitionRow, CommunicationType>(
                    tables,
                    TableDefinitions.CommunicationType,
                    _definitionRepo.CommunicationTypeAddUpdate,
                    nameof(TableDefinitions.CommunicationType)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TableDefinitions, TableDefinitionRow, ConformityFrequencyPlan>(
                    tables,
                    TableDefinitions.ConformityToFrequencyPlan,
                    _definitionRepo.ConformityToFrequencyPlanAddUpdate,
                    nameof(TableDefinitions.ConformityToFrequencyPlan)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TableDefinitions, TableDefinitionRow, AnalogDigital>(
                    tables,
                    TableDefinitions.AnalogDigital,
                    _definitionRepo.AnalogDigitalAddUpdate,
                    nameof(TableDefinitions.AnalogDigital)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TableDefinitions, TableDefinitionRow, ModulationType>(
                    tables,
                    TableDefinitions.ModulationType,
                    _definitionRepo.ModulationTypeAddUpdate,
                    nameof(TableDefinitions.ModulationType)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TableDefinitions, TableDefinitionRow, FiltrationInstalledType>(
                    tables,
                    TableDefinitions.FiltrationType,
                    _definitionRepo.FiltrationTypeAddUpdate,
                    nameof(TableDefinitions.FiltrationType)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TableDefinitions, TableDefinitionRow, AntennaPattern>(
                    tables,
                    TableDefinitions.AntennaPattern,
                    _definitionRepo.AntennaPatternAddUpdate,
                    nameof(TableDefinitions.AntennaPattern)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TableDefinitions, TableDefinitionRow, PolarizationType>(
                    tables,
                    TableDefinitions.Polarization,
                    _definitionRepo.PolarizationAddUpdate,
                    nameof(TableDefinitions.Polarization)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TableDefinitions, TableDefinitionRow, StationType>(
                    tables,
                    TableDefinitions.TypeOfStation,
                    _definitionRepo.TypeOfStationAddUpdate,
                    nameof(TableDefinitions.TypeOfStation)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TableDefinitions, TableDefinitionRow, ITUClassType>(
                    tables,
                    TableDefinitions.ITUClass,
                    _definitionRepo.ITUClassAddUpdate,
                    nameof(TableDefinitions.ITUClass)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TableDefinitions, TableDefinitionRow, StationCostCategory>(
                    tables,
                    TableDefinitions.StationCostCategory,
                    _definitionRepo.StationCostCategoryAddUpdate,
                    nameof(TableDefinitions.StationCostCategory)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TableDefinitions, TableDefinitionRow, Province>(
                    tables,
                    TableDefinitions.Provinces,
                    _definitionRepo.ProvincesAddUpdate,
                    nameof(TableDefinitions.Provinces)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TableDefinitions, TableDefinitionRow, CongestionZoneType>(
                    tables,
                    TableDefinitions.CongestionZone,
                    _definitionRepo.CongestionZoneAddUpdate,
                    nameof(TableDefinitions.CongestionZone)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TableDefinitions, TableDefinitionRow, ServiceType>(
                    tables,
                    TableDefinitions.Service,
                    _definitionRepo.ServiceAddUpdate,
                    nameof(TableDefinitions.Service)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TableDefinitions, TableDefinitionRow, SubserviceType>(
                    tables,
                    TableDefinitions.Subservice,
                    _definitionRepo.SubserviceAddUpdate,
                    nameof(TableDefinitions.Subservice)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TableDefinitions, TableDefinitionRow, LicenseType>(
                    tables,
                    TableDefinitions.LicenseType,
                    _definitionRepo.LicenseTypeAddUpdate,
                    nameof(TableDefinitions.LicenseType)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TableDefinitions, TableDefinitionRow, AuthorizationStatus>(
                    tables,
                    TableDefinitions.AuthorizationStatus,
                    _definitionRepo.AuthorizationStatusAddUpdate,
                    nameof(TableDefinitions.AuthorizationStatus)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TableDefinitions, TableDefinitionRow, OperationalStatus>(
                    tables,
                    TableDefinitions.OperationalStatus,
                    _definitionRepo.OperationalStatusAddUpdate,
                    nameof(TableDefinitions.OperationalStatus)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TableDefinitions, TableDefinitionRow, StationClass>(
                    tables,
                    TableDefinitions.StationClass,
                    _definitionRepo.StationClassAddUpdate,
                    nameof(TableDefinitions.StationClass)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TableDefinitions, TableDefinitionRow, StandbyTransmitterInformation>(
                    tables,
                    TableDefinitions.StandbyTransmitterInformation,
                    _definitionRepo.StandbyTransmitterInformationAddUpdate,
                    nameof(TableDefinitions.StandbyTransmitterInformation)
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

            return csv;
        }

        private void ThrowIfTableInvalid(PdfTable table)
        {
            int rows = table.GetRowCount();
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

        private List<TableDefinitionRow> GetAllRows(PdfTable table, bool ignoreHeader = true)
        {
            int curRow = ignoreHeader ? 1 : 0;
            int totalRows = table.GetRowCount();

            List<TableDefinitionRow> parsedRows = new(totalRows - curRow);

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

                TableDefinitionRow parseRow = new()
                {
                    Code = code,
                    DescriptionEN = englishDefinition,
                    DescriptionFR = frenchDefinition,
                };
                parsedRows.Add(parseRow);
            }

            return parsedRows;
        }

        private int GetPropCount<T>()
        {
            var type = typeof(T);

            // Get all public instance properties
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            return properties.Length;
        }

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

            return rows.Count;
        }
    }
}
