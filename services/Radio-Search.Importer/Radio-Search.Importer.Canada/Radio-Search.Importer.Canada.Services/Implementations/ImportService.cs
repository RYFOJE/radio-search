using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Radio_Search.Importer.Canada.Services.Data;
using Radio_Search.Importer.Canada.Services.Interfaces;
using Radio_Search.Importer.Canada.Services.Responses;
using Radio_Search.Utils.BlobStorage.Interfaces;
using System.Globalization;
using System.Reflection;

namespace Radio_Search.Importer.Canada.Services.Implementations
{
    public class ImportService : IImportService
    {
        private readonly ILogger<ImportService> _logger;
        private readonly IConfiguration _config;

        public ImportService(
            ILogger<ImportService> logger,
            IConfiguration config,
            IBlobStorageService blobStorageService)
        {
            _logger = logger;
            _config = config;
        }

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

        public async Task SaveTAFLToDB(List<BroadcastAuthorizationRecord> records)
        {
            throw new NotImplementedException();
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

        private int GetPropCount<T>()
        {
            var type = typeof(T);

            // Get all public instance properties
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            return properties.Length;
        }
    }
}
