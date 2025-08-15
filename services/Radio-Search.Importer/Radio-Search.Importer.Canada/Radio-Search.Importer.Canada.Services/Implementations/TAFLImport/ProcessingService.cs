using Microsoft.Extensions.Logging;
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

        public ProcessingService(ILogger<ProcessingService> logger, ITAFLRepo taflRepo)
        {
            _logger = logger;
            _taflRepo = taflRepo;
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

                }).ToList();

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while trying to GetInsertsAndUpdates.");
                throw;
            }
        }

        public Task InsertNewFromRawRecords(List<TAFLEntryRawRow> rows)
        {
            throw new NotImplementedException();
        }

        public Task InsertUpdatedFromRawRecords(List<TAFLEntryRawRow> rows)
        {
            throw new NotImplementedException();
        }
    }
}
