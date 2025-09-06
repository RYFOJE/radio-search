using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Radio_Search.Importer.Canada.Data;
using Radio_Search.Importer.Canada.Data.Models.License;
using Radio_Search.Importer.Canada.Data.Repositories.Interfaces;
using Radio_Search.Importer.Canada.Services.Configuration;
using Radio_Search.Importer.Canada.Services.Data;
using Radio_Search.Importer.Canada.Services.Data.Enums;
using Radio_Search.Importer.Canada.Services.Exceptions.TAFLDefinitionExceptions;
using Radio_Search.Importer.Canada.Services.Interfaces.TAFLDefinitionImport;
using Radio_Search.Importer.Canada.Services.Responses;
using Spire.Pdf;
using Spire.Pdf.Utilities;
using System.Reflection;

namespace Radio_Search.Importer.Canada.Services.Implementations.TAFLDefinitionImport
{


    public class TaflDefinitionImportService : ITAFLDefinitionImportService
    {
        private readonly ILogger<TaflDefinitionImportService> _logger;
        private readonly TAFLDefinitionTablesOrder _taflDefinitionOrder;
        private readonly CanadaImporterContext _context;
        private readonly ITAFLDefinitionRepo _definitionRepo;
        private readonly IMapper _mapper;
        private readonly IPDFProcessingServices _pdfService;
        public readonly IValidator<TaflEntryRawRow> _taflRawRowValidator;

        public TaflDefinitionImportService(
                ILogger<TaflDefinitionImportService> logger,
                IOptions<TAFLDefinitionTablesOrder> taflDefinitionOrder,
                CanadaImporterContext context,
                ITAFLDefinitionRepo definitionRepo,
                ITAFLRepo taflRepo,
                IImportJobRepo historyRepo,
                IMapper mapper,
                IPDFProcessingServices pdfService,
                IValidator<TaflEntryRawRow> taflRawRowValidator)
        {
            _logger = logger;
            _taflDefinitionOrder = taflDefinitionOrder.Value;
            _context = context;
            _definitionRepo = definitionRepo;
            _mapper = mapper;
            _pdfService = pdfService;
            _taflRawRowValidator = taflRawRowValidator;
        }

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
        public async Task<int> SaveTAFLDefinitionToDBAsync(Dictionary<TAFLDefinitionTableEnum, HashSet<TAFLDefinitionRawRow>> tables)
        {
            int totalModifiedRowCount = 0;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {


                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, StationFunctionType>(
                    tables,
                    TAFLDefinitionTableEnum.StationFunction,
                    _definitionRepo.StationFunctionAddUpdateAsync,
                    nameof(TAFLDefinitionTableEnum.StationFunction)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, RegulatoryService>(
                    tables,
                    TAFLDefinitionTableEnum.RegulatoryService,
                    _definitionRepo.RegulatoryServiceAddUpdateAsync,
                    nameof(TAFLDefinitionTableEnum.RegulatoryService)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, CommunicationType>(
                    tables,
                    TAFLDefinitionTableEnum.CommunicationType,
                    _definitionRepo.CommunicationTypeAddUpdateAsync,
                    nameof(TAFLDefinitionTableEnum.CommunicationType)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, ConformityFrequencyPlan>(
                    tables,
                    TAFLDefinitionTableEnum.ConformityToFrequencyPlan,
                    _definitionRepo.ConformityToFrequencyPlanAddUpdateAsync,
                    nameof(TAFLDefinitionTableEnum.ConformityToFrequencyPlan)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, AnalogDigital>(
                    tables,
                    TAFLDefinitionTableEnum.AnalogDigital,
                    _definitionRepo.AnalogDigitalAddUpdateAsync,
                    nameof(TAFLDefinitionTableEnum.AnalogDigital)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, ModulationType>(
                    tables,
                    TAFLDefinitionTableEnum.ModulationType,
                    _definitionRepo.ModulationTypeAddUpdateAsync,
                    nameof(TAFLDefinitionTableEnum.ModulationType)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, FiltrationInstalledType>(
                    tables,
                    TAFLDefinitionTableEnum.FiltrationInstalledType,
                    _definitionRepo.FiltrationTypeAddUpdateAsync,
                    nameof(TAFLDefinitionTableEnum.FiltrationInstalledType)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, AntennaPattern>(
                    tables,
                    TAFLDefinitionTableEnum.AntennaPattern,
                    _definitionRepo.AntennaPatternAddUpdateAsync,
                    nameof(TAFLDefinitionTableEnum.AntennaPattern)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, PolarizationType>(
                    tables,
                    TAFLDefinitionTableEnum.Polarization,
                    _definitionRepo.PolarizationAddUpdateAsync,
                    nameof(TAFLDefinitionTableEnum.Polarization)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, StationType>(
                    tables,
                    TAFLDefinitionTableEnum.TypeOfStation,
                    _definitionRepo.TypeOfStationAddUpdateAsync,
                    nameof(TAFLDefinitionTableEnum.TypeOfStation)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, ITUClassType>(
                    tables,
                    TAFLDefinitionTableEnum.ITUClass,
                    _definitionRepo.ITUClassAddUpdateAsync,
                    nameof(TAFLDefinitionTableEnum.ITUClass)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, StationCostCategory>(
                    tables,
                    TAFLDefinitionTableEnum.StationCostCategory,
                    _definitionRepo.StationCostCategoryAddUpdateAsync,
                    nameof(TAFLDefinitionTableEnum.StationCostCategory)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, Province>(
                    tables,
                    TAFLDefinitionTableEnum.Provinces,
                    _definitionRepo.ProvincesAddUpdateAsync,
                    nameof(TAFLDefinitionTableEnum.Provinces)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, CongestionZoneType>(
                    tables,
                    TAFLDefinitionTableEnum.CongestionZone,
                    _definitionRepo.CongestionZoneAddUpdateAsync,
                    nameof(TAFLDefinitionTableEnum.CongestionZone)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, ServiceType>(
                    tables,
                    TAFLDefinitionTableEnum.Service,
                    _definitionRepo.ServiceAddUpdateAsync,
                    nameof(TAFLDefinitionTableEnum.Service)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, SubserviceType>(
                    tables,
                    TAFLDefinitionTableEnum.Subservice,
                    _definitionRepo.SubserviceAddUpdateAsync,
                    nameof(TAFLDefinitionTableEnum.Subservice)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, LicenseType>(
                    tables,
                    TAFLDefinitionTableEnum.LicenseType,
                    _definitionRepo.LicenseTypeAddUpdateAsync,
                    nameof(TAFLDefinitionTableEnum.LicenseType)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, AuthorizationStatus>(
                    tables,
                    TAFLDefinitionTableEnum.AuthorizationStatus,
                    _definitionRepo.AuthorizationStatusAddUpdateAsync,
                    nameof(TAFLDefinitionTableEnum.AuthorizationStatus)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, OperationalStatus>(
                    tables,
                    TAFLDefinitionTableEnum.OperationalStatus,
                    _definitionRepo.OperationalStatusAddUpdateAsync,
                    nameof(TAFLDefinitionTableEnum.OperationalStatus)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, StationClass>(
                    tables,
                    TAFLDefinitionTableEnum.StationClass,
                    _definitionRepo.StationClassAddUpdateAsync,
                    nameof(TAFLDefinitionTableEnum.StationClass)
                );

                totalModifiedRowCount += await ProcessTableDefinitionAsync<TAFLDefinitionTableEnum, TAFLDefinitionRawRow, StandbyTransmitterInformation>(
                    tables,
                    TAFLDefinitionTableEnum.StandbyTransmitterInformation,
                    _definitionRepo.StandbyTransmitterInformationAddUpdateAsync,
                    nameof(TAFLDefinitionTableEnum.StandbyTransmitterInformation)
                );

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Failed while processing SaveTAFLDefinitionToDBAsync");
                throw;
            }

            await transaction.CommitAsync();
            return totalModifiedRowCount;
        }

        #endregion

        #region private

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

    }

}