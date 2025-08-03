using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Radio_Search.Importer.Canada.Data.Models.License;
using Radio_Search.Importer.Canada.Data.Repositories.Interfaces;

namespace Radio_Search.Importer.Canada.Data.Repositories
{
    /// <inheritdoc/>
    public class TAFLRepo : ITAFLRepo
    {
        private const string RECORD_FETCH_LIMIT_CONFIG_KEY = "Import:RecordFetchLimit";
        private const string BULK_UPDATE_BATCH_SIZE_CONFIG_KEY = "Import:BulkUpdateBatchSize";

        private readonly CanadaImporterContext _context;
        private readonly IConfiguration _config;

        public TAFLRepo(
            CanadaImporterContext context,
            IConfiguration config) 
        {
            _context = context;
            _config = config;
        }

        /// <inheritdoc/>
        public async Task BulkAddLicenseRecordsAsync(List<LicenseRecord> records)
        {
            await _context.BulkInsertAsync(records, opt =>{
                
            });
        }

        /// <inheritdoc/>
        public async Task BulkInvalidateRecords(List<string> licenseIDs)
        {
            int batchSize;
            var configValue = _config[BULK_UPDATE_BATCH_SIZE_CONFIG_KEY];
            if (string.IsNullOrWhiteSpace(configValue) || !int.TryParse(configValue, out batchSize))
                throw new ArgumentException($"{BULK_UPDATE_BATCH_SIZE_CONFIG_KEY} must be a valid integer.");

            var idBatches = licenseIDs.Chunk(batchSize);

            foreach (var batch in idBatches)
            {
                await _context.LicenseRecords
                    .Where(b => batch.Contains(b.CanadaLicenseRecordID) && b.IsValid == true)
                    .ExecuteUpdateAsync(setters =>
                        setters.SetProperty(b => b.IsValid, false));
            }
        }

        /// <inheritdoc/>
        public async Task<List<LicenseRecord>> GetRecordsNoTrackingAsync(int skipCount, int takeCount, bool isValidOnly = true)
        {
            int fetchLimit;
            var configValue = _config[RECORD_FETCH_LIMIT_CONFIG_KEY];
            if (string.IsNullOrWhiteSpace(configValue) || !int.TryParse(configValue, out fetchLimit))
                throw new ArgumentException($"{RECORD_FETCH_LIMIT_CONFIG_KEY} must be a valid integer.");

            if (takeCount > fetchLimit)
                throw new InvalidOperationException($"Attempted to fetch {takeCount} when the limit is {fetchLimit}.");

            var query = _context.LicenseRecords
                .OrderBy(x => x.CanadaLicenseRecordID)
                .AsNoTracking();

            if (isValidOnly)
                query = query.Where(x => x.IsValid);

            query = query.Skip(skipCount).Take(takeCount);

            return await query.ToListAsync();
        }


    }
}
