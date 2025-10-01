using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Radio_Search.Importer.Canada.Data.Repositories.Interfaces;
using Npgsql;
using Radio_Search.Canada.Models.License;

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
            await _context.BulkInsertOrUpdateAsync(records, opt => {
                opt.BulkCopyTimeout = 400;
                opt.BatchSize = 5000;
                opt.EnableStreaming = true;
                opt.TrackingEntities = false;
            });
        }

        /// <inheritdoc/>
        public async Task BulkInvalidateRecordsAsync(List<int> licenseIDs)
        {
            int batchSize;
            var configValue = _config[BULK_UPDATE_BATCH_SIZE_CONFIG_KEY];
            if (string.IsNullOrWhiteSpace(configValue) || !int.TryParse(configValue, out batchSize))
                throw new ArgumentException($"{BULK_UPDATE_BATCH_SIZE_CONFIG_KEY} must be a valid integer.");

            var idBatches = licenseIDs.Chunk(batchSize);

            foreach (var batch in idBatches)
            {
                await _context.LicenseRecords
                    .Where(b => batch.Contains(b.CanadaLicenseRecordID) && b.IsValid)
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

        /// <inheritdoc/>
        public async Task<List<LicenseRecord>> BulkFetchLicenseRecordsNoTrackingAsync(List<int> licenseIDs)
        {
            int fetchLimit;
            var configValue = _config[RECORD_FETCH_LIMIT_CONFIG_KEY];
            if (string.IsNullOrWhiteSpace(configValue) || !int.TryParse(configValue, out fetchLimit))
                throw new ArgumentException($"{RECORD_FETCH_LIMIT_CONFIG_KEY} must be a valid integer.");

            var allRecords = new List<LicenseRecord>();

            for (int i = 0; i < licenseIDs.Count; i += fetchLimit)
            {
                var batchKeys = licenseIDs.Skip(i).Take(fetchLimit).ToList();

                var sql = $@"
                    SELECT lr.* 
                    FROM canada_importer.""LicenseRecords"" lr
                    WHERE lr.""CanadaLicenseRecordID"" = ANY(@ids)
                    AND lr.""IsValid"" = true";

                var batchRecords = await _context.LicenseRecords
                    .FromSqlRaw(sql, new NpgsqlParameter("@ids", batchKeys.ToArray()))
                    .AsNoTracking()
                    .ToListAsync();

                allRecords.AddRange(batchRecords);
            }

            return allRecords;
        }

        public async Task<Dictionary<int, int>> GetValidLicensesVersionIdsAsync(List<int> recordIds)
        {
            return await _context.LicenseRecords
                .Where(x => x.IsValid && recordIds.Contains(x.CanadaLicenseRecordID))
                .ToDictionaryAsync(
                    x => x.CanadaLicenseRecordID,
                    x => x.Version
                );
        }
    }
}
