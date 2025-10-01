using AutoMapper;
using Azure;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Radio_Search.Importer.Canada.Data;
using Radio_Search.Canada.Models.ImportInfo;
using Radio_Search.Canada.Models.License;
using Radio_Search.Importer.Canada.Data.Repositories.Interfaces;
using Radio_Search.Importer.Canada.Services.Configuration;
using Radio_Search.Importer.Canada.Services.Data;
using Radio_Search.Importer.Canada.Services.Implementations;
using Radio_Search.Importer.Canada.Services.Interfaces.TAFLDefinition;
using Radio_Search.Importer.Canada.Services.Responses;
using Radio_Search.Importer.Canada.Services.Validators;
using Xunit.Sdk;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Radio_Search.Importer.Canada.UnitTests.Services
{
    public class ImportServiceTests
    {
        private readonly Mock<ILogger<ImportService>> _logger = new();
        private readonly Mock<IConfiguration> _config = new();
        private readonly Mock<IOptions<TAFLDefinitionTablesOrder>> _taflDefinitionOrderOptions = new();
        private readonly Mock<CanadaImporterContext> _context;
        private readonly Mock<ITAFLDefinitionRepo> _definitionRepo = new();
        private readonly Mock<ITAFLRepo> _taflRepo = new();
        private readonly Mock<IImportJobRepo> _historyRepo = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IPDFProcessingServices> _pdfService = new();

        private readonly ImportService _service;

        public ImportServiceTests()
        {
            var options = new DbContextOptionsBuilder<CanadaImporterContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new Mock<CanadaImporterContext>(options);

            _service = new ImportService(
                _logger.Object,
                _config.Object,
                _taflDefinitionOrderOptions.Object,
                _context.Object,
                _definitionRepo.Object,
                _taflRepo.Object,
                _historyRepo.Object,
                _mapper.Object,
                _pdfService.Object,
                new TAFLEntryRawRowValidator() // Check to see if we want this or not
            );
        }

        #region ExtractTAFLRawRowsFromCSV

        // TODO Implement these tests

        #endregion

        #region GetActionsPerRawRow

        // TODO Implement these tests too :(

        #endregion

        #region CreateImportHistoryRecord

        [Fact]
        public async Task CreateImportHistoryRecord_CreatesRecord()
        {
            // Arrange
            byte[] data = new byte[] { 1, 2, 3 };
            _historyRepo.Setup(x => x.CreateImportHistoryRecord(It.IsAny<ImportJob>())).ReturnsAsync((ImportJob history) => { return history; }); // Return the object passed in

            // Act
            var response = await _service.CreateImportHistoryRecord(data);

            // Assert
            _historyRepo.Verify(x => x.CreateImportHistoryRecord(It.IsAny<ImportJob>()), Times.Once);
            Assert.NotEqual(Guid.Empty, response.ImportHistoryID);
            Assert.Equal(data, response.FileHash);
            Assert.Equal(ImportStatus.Pending, response.Status);
        }

        [Fact]
        public async Task CreateImportHistoryRecord_FailedCreate()
        {
            // Arrange
            byte[] data = new byte[] { 1, 2, 3 };
            _historyRepo.Setup(x => x.CreateImportHistoryRecord(It.IsAny<ImportJob>())).Throws<InvalidOperationException>();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateImportHistoryRecord(data));
            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed while CreateImportHistoryRecord")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }

        #endregion

        #region TAFLListToDictionary

        [Fact]
        public void TAFLListToDictionary_EmptyList()
        {
            // Arrange
            List<TaflEntryRawRow> rawRows = new();

            // Act
            var response = _service.TAFLListToDictionary(rawRows);

            // Assert
            Assert.Empty(response);
        }

        [Fact]
        public void TAFLListToDictionary_Single()
        {
            // Arrange
            List<TaflEntryRawRow> rawRows = [
                new() {LicenseRecordID = "record1"}];

            // Act
            var response = _service.TAFLListToDictionary(rawRows);

            // Assert
            Assert.Single(response);
        }

        [Fact]
        public void TAFLListToDictionary_MultipleUnique()
        {
            // Arrange
            List<TaflEntryRawRow> rawRows = [
                new() {LicenseRecordID = "record1"},
                new() {LicenseRecordID = "record2"}];

            // Act
            var response = _service.TAFLListToDictionary(rawRows);

            // Assert
            Assert.Equal(2, response.Count());
        }

        [Fact]
        public void TAFLListToDictionary_MultipleDuplicates()
        {
            // Arrange
            List<TaflEntryRawRow> rawRows = [
                new() {LicenseRecordID = "record1", AnalogDigitalID = "203"},
                new() {LicenseRecordID = "record1", AnalogDigitalID = "12"}];

            // Act
            Assert.Throws<ArgumentException>(() => _service.TAFLListToDictionary(rawRows));
        }

        #endregion

        #region GetUpdatedRowsFromRange

        [Fact]
        public void GetUpdatedRowsFromRange_NoUpdates()
        {
            // Arrange
            Dictionary<string, TaflEntryRawRow> inputRows = new(){
                { "ABC", GetTAFLRawRow("ABC", 12.1m) },
                { "123", GetTAFLRawRow("123", 12.1m) },
                { "a1b2", GetTAFLRawRow("a1b2", 12.1m) },
                { "wowza", GetTAFLRawRow("wowza", 12.1m) }
            };

            List<LicenseRecord> dbRows = [
                GetTAFLDbRow("ABC", 12.1m),
                GetTAFLDbRow("123", 12.1m)
            ];

            // Act
            var response = _service.GetUpdatedRowsFromRange(inputRows, dbRows);

            // Assert
            Assert.Empty(response);
        }

        [Fact]
        public void GetUpdatedRowsFromRange_SingleUpdate()
        {
            // Arrange
            Dictionary<string, TaflEntryRawRow> inputRows = new(){
                { "ABC", GetTAFLRawRow("ABC", 12.1m) },
                { "123", GetTAFLRawRow("123", 12.1m) }, // Updated Row
                { "a1b2", GetTAFLRawRow("a1b2", 12.1m) },
                { "wowza", GetTAFLRawRow("wowza", 12.1m) }
            };

            List<LicenseRecord> dbRows = [
                GetTAFLDbRow("ABC", 12.1m), 
                GetTAFLDbRow("123", 12.3m)// Updated Row
            ];

            // Act
            var response = _service.GetUpdatedRowsFromRange(inputRows, dbRows);

            // Assert
            Assert.Single(response);
            Assert.Equal(inputRows["123"], response.First());
        }

        [Fact]
        public void GetUpdatedRowsFromRange_MultipleUpdate()
        {
            // Arrange
            Dictionary<string, TaflEntryRawRow> inputRows = new(){
                { "ABC", GetTAFLRawRow("ABC", 12.1m) }, // Updated Row
                { "123", GetTAFLRawRow("123", 12.1m) }, // Updated Row
                { "a1b2", GetTAFLRawRow("a1b2", 12.1m) },
                { "wowza", GetTAFLRawRow("wowza", 12.1m) }
            };

            List<LicenseRecord> dbRows = [
                GetTAFLDbRow("ABC", 12.3m), // Updated Row
                GetTAFLDbRow("123", 12.45m)// Updated Row
            ];

            // Act
            var response = _service.GetUpdatedRowsFromRange(inputRows, dbRows);

            // Assert
            Assert.Equal(2, response.Count());
            Assert.Contains(inputRows["ABC"], response);
            Assert.Contains(inputRows["123"], response);
        }

        #endregion

        #region GetDeletedRowsFromRange

        [Fact]
        public void GetDeletedRowsFromRange_NoDeletes()
        {
            // Arrange
            Dictionary<string, TaflEntryRawRow> inputRows = new(){
                { "ABC", GetTAFLRawRow("ABC", 12.1m) },
                { "123", GetTAFLRawRow("123", 12.1m) },
                { "asda", GetTAFLRawRow("asda", 12.1m) }
            };

            List<LicenseRecord> dbRows = [
                GetTAFLDbRow("ABC", 12.1m),
                GetTAFLDbRow("123", 12.4m)
            ];

            // Act
            var response = _service.GetDeletedRowsFromRange(inputRows, dbRows);

            // Assert
            Assert.Empty(response);
        }


        [Fact]
        public void GetDeletedRowsFromRange_OneDelete()
        {
            // Arrange
            Dictionary<string, TaflEntryRawRow> inputRows = new(){
                { "123", GetTAFLRawRow("123", 12.1m) },
                { "asda", GetTAFLRawRow("asda", 12.1m) }
            };

            List<LicenseRecord> dbRows = [
                GetTAFLDbRow("ABC", 12.5m),
                GetTAFLDbRow("123", 12.4m)
            ];

            // Act
            var response = _service.GetDeletedRowsFromRange(inputRows, dbRows);

            // Assert
            Assert.Single(response);
            Assert.Equal("ABC", response.First().CanadaLicenseRecordID);
        }


        [Fact]
        public void GetDeletedRowsFromRange_MultipleDelete()
        {
            // Arrange
            Dictionary<string, TaflEntryRawRow> inputRows = new(){
                { "asda", GetTAFLRawRow("asda", 12.1m) }
            };

            List<LicenseRecord> dbRows = [
                GetTAFLDbRow("ABC", 12.5m),
                GetTAFLDbRow("123", 12.4m)
            ];

            // Act
            var response = _service.GetDeletedRowsFromRange(inputRows, dbRows);

            // Assert
            Assert.NotEmpty(response);
            Assert.Equal(2, response.Count());
        }

        #endregion

        #region GetUnaffectedRowsFromRange

        [Fact]
        public void GetUnaffectedRowsFromRange_NoResults()
        {
            // Arrange
            Dictionary<string, TaflEntryRawRow> inputRows = new(){
                { "ABC", GetTAFLRawRow("ABC", 12.2m) },
                { "123", GetTAFLRawRow("123", 12.9m) },
                { "asda", GetTAFLRawRow("asda", 12.1m) }
            };

            List<LicenseRecord> dbRows = [
                GetTAFLDbRow("ABC", 12.1m),
                GetTAFLDbRow("123", 12.4m)
            ];

            // Act
            var response = _service.GetUnaffectedRowsFromRange(inputRows, dbRows);

            // Assert
            Assert.Empty(response);
        }

        [Fact]
        public void GetUnaffectedRowsFromRange_SingleResult()
        {
            // Arrange
            Dictionary<string, TaflEntryRawRow> inputRows = new(){
                { "ABC", GetTAFLRawRow("ABC", 12.1m) },
                { "123", GetTAFLRawRow("123", 12.9m) },
                { "asda", GetTAFLRawRow("asda", 12.1m) }
            };

            List<LicenseRecord> dbRows = [
                GetTAFLDbRow("ABC", 12.1m),
                GetTAFLDbRow("123", 12.4m)
            ];

            // Act
            var response = _service.GetUnaffectedRowsFromRange(inputRows, dbRows);

            // Assert
            Assert.Single(response);
            Assert.Contains("ABC", response);
        }


        [Fact]
        public void GetUnaffectedRowsFromRange_MultipleResult()
        {
            // Arrange
            Dictionary<string, TaflEntryRawRow> inputRows = new(){
                { "ABC", GetTAFLRawRow("ABC", 12.1m) },
                { "123", GetTAFLRawRow("123", 12.4m) },
                { "asda", GetTAFLRawRow("asda", 12.1m) }
            };

            List<LicenseRecord> dbRows = [
                GetTAFLDbRow("ABC", 12.1m),
                GetTAFLDbRow("123", 12.4m)
            ];

            // Act
            var response = _service.GetUnaffectedRowsFromRange(inputRows, dbRows);

            // Assert
            Assert.Equal(2, response.Count());
            Assert.Contains("ABC", response);
            Assert.Contains("123", response);
        }

        #endregion

        #region GetCreatedRows

        [Fact]
        public void GetCreatedRows_NoResults()
        {
            // Arrange
            Dictionary<string, TaflEntryRawRow> inputRows = new(){
                { "ABC", GetTAFLRawRow("ABC", 12.2m) },
                { "123", GetTAFLRawRow("123", 12.9m) },
                { "asda", GetTAFLRawRow("asda", 12.1m) }
            };

            GetAffectedRowsResponse affectedRows = new()
            {
                UpdatedRows = [GetTAFLRawRow("ABC", 12.1m)],
                DeletedRows = [GetTAFLDbRow("123", 12.1m)],
                UnaffectedRows = ["asda"]
            };

            // Act
            var response = _service.GetCreatedRows(inputRows, affectedRows);

            // Assert
            Assert.Empty(response);
        }

        [Fact]
        public void GetCreatedRows_SingleResult()
        {
            // Arrange
            Dictionary<string, TaflEntryRawRow> inputRows = new(){
                { "ABC", GetTAFLRawRow("ABC", 12.2m) },
                { "123", GetTAFLRawRow("123", 12.9m) },
                { "asda", GetTAFLRawRow("asda", 12.1m) }
            };

            GetAffectedRowsResponse affectedRows = new()
            {
                UpdatedRows = [GetTAFLRawRow("ABC", 12.1m)],
                UnaffectedRows = ["asda"]
            };

            // Act
            var response = _service.GetCreatedRows(inputRows, affectedRows);

            // Assert
            Assert.Single(response);
            Assert.Contains(inputRows["123"], response);
        }

        [Fact]
        public void GetCreatedRows_MultipleResult()
        {
            // Arrange
            Dictionary<string, TaflEntryRawRow> inputRows = new(){
                { "ABC", GetTAFLRawRow("ABC", 12.2m) },
                { "123", GetTAFLRawRow("123", 12.9m) },
                { "asda", GetTAFLRawRow("asda", 12.1m) }
            };

            GetAffectedRowsResponse affectedRows = new()
            {
                UnaffectedRows = ["asda"]
            };

            // Act
            var response = _service.GetCreatedRows(inputRows, affectedRows);

            // Assert
            Assert.Equal(2, response.Count());
            Assert.Contains(inputRows["ABC"], response);
            Assert.Contains(inputRows["123"], response);
        }

        #endregion

        #region MarkImportAsFailed

        [Fact]
        public async Task MarkImportAsFailed_Success()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var importHistory = new ImportJob
            {
                ImportJobID = guid,
                Status = ImportStatus.Pending,
                EndTime = null
            };

            _historyRepo.Setup(x => x.GetImportHistoryRecord(guid)).ReturnsAsync(importHistory);
            _historyRepo.Setup(x => x.UpdateImportHistoryRecord(It.IsAny<ImportJob>()))
                .ReturnsAsync((ImportJob h) => h);

            // Act
            await _service.MarkImportAsFailed(guid);

            // Assert
            _historyRepo.Verify(x => x.GetImportHistoryRecord(guid), Times.Once);
            _historyRepo.Verify(x => x.UpdateImportHistoryRecord(It.Is<ImportJob>(
                h => h.Status == ImportStatus.Failure && h.EndTime != null)), Times.Once);
        }

        [Fact]
        public async Task MarkImportAsFailed_Failure()
        {
            // Arrange
            var guid = Guid.NewGuid();
            _historyRepo.Setup(x => x.GetImportHistoryRecord(guid)).ThrowsAsync(new Exception("DB error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.MarkImportAsFailed(guid));
            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed while trying to MarkImportAsFailedAsync")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.AtLeastOnce);
        }

        #endregion

        #region DeduplicateRows

        [Fact]
        public void DeduplicateRows_NoDuplicates()
        {
            // Arrange
            List<TaflEntryRawRow> rows = [
                new()
                    { 
                      LicenseRecordID = "00001234",
                      InServiceDate = "2024-10-10"
                    },
                new()
                    {
                        LicenseRecordID = "000012233",
                        InServiceDate = "2024-10-10"
                    }
                ];

            // Act
            var resp = _service.DeduplicateRows(rows);

            // Assert
            Assert.Equal(2, rows.Count());
            Assert.Contains(rows[0], resp);
            Assert.Contains(rows[1], resp);
        }

        [Fact]
        public void DeduplicateRows_Duplicates()
        {
            // Arrange
            List<TaflEntryRawRow> rows = [
                new()
                    {
                      LicenseRecordID = "000012233",
                      InServiceDate = "2024-10-11"
                    },
                new() // Should be ignored
                    {
                        LicenseRecordID = "000012233",
                        InServiceDate = "2022-10-10"
                    }
                ];

            // Act
            var resp = _service.DeduplicateRows(rows);

            // Assert
            Assert.Single(resp);
            Assert.Contains(rows[0], resp);
        }

        [Fact]
        public void DeduplicateRows_BadData()
        {
            // Arrange
            List<TaflEntryRawRow> rows = [
                new()
                    {
                      LicenseRecordID = "000012233",
                      InServiceDate = "2024-101-11"
                    },
                new()
                    {
                      LicenseRecordID = "000012233",
                      InServiceDate = "2024-121-11"
                    },
                new()
                    {
                      LicenseRecordID = "000012233",
                    },
                new() // Should not be ignored
                    {
                        LicenseRecordID = "000012233",
                        InServiceDate = "2022-10-10"
                    }
                ];

            // Act
            var resp = _service.DeduplicateRows(rows);

            // Assert
            Assert.Single(resp);
            Assert.Contains(rows[3], resp);
        }

        #endregion

        #region HELPERS

        private TaflEntryRawRow GetTAFLRawRow(string LicenseID, decimal Frequency)
        {
            return new()
            { 
                LicenseRecordID = LicenseID,
                AccountNumber = "123",
                AnalogCapacityChannels = 1,
                AnalogDigitalID = "a",
                CallSign = "YSI123",
                FrequencyMHz = Frequency,
            };
        }

        private LicenseRecord GetTAFLDbRow(string LicenseID, decimal Frequency)
        {
            return new()
            {
                InternalLicenseRecordID = Guid.NewGuid(),
                CanadaLicenseRecordID = LicenseID,
                AccountNumber = "123",
                AnalogCapacityChannels = 1,
                AnalogDigitalID = 'a',
                CallSign = "YSI123",
                FrequencyMHz = Frequency,
            };
        }

        #endregion
    }
}
