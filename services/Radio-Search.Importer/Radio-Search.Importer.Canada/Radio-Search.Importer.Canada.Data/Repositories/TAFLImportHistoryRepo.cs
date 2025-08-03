using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Radio_Search.Importer.Canada.Data.Models.History;
using Radio_Search.Importer.Canada.Data.Repositories.Interfaces;

namespace Radio_Search.Importer.Canada.Data.Repositories
{
    public class TAFLImportHistoryRepo : ITAFLImportHistoryRepo
    {
        private readonly CanadaImporterContext _context;
        private readonly ILogger<TAFLImportHistoryRepo> _logger;

        public TAFLImportHistoryRepo(
            CanadaImporterContext context,
            ILogger<TAFLImportHistoryRepo> _logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = _logger ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public async Task BulkInsertLicenseRecordHistory(List<LicenseRecordHistory> licenseRecordHistories)
        {
            await _context.BulkInsertAsync(licenseRecordHistories);
        }

        /// <inheritdoc/>
        public async Task<ImportHistory> CreateImportHistoryRecord(ImportHistory record)
        {
            _context.ImportHistories.Add(record);
            await _context.SaveChangesAsync();
            await _context.Entry(record).ReloadAsync();
            return record;
        }

        /// <inheritdoc/>
        public async Task<ImportHistory> UpdateImportHistoryRecord(ImportHistory record)
        {
            _context.ImportHistories.Update(record);
            await _context.SaveChangesAsync();
            await _context.Entry(record).ReloadAsync();
            return record;
        }

        /// <inheritdoc/>
        public async Task<ImportHistory> GetImportHistoryRecord(int id)
        {
            var record = await _context.ImportHistories.FirstOrDefaultAsync(x => x.ImportHistoryID == id);

            if(record is null)
            {
                _logger.LogError("Failed to find a ImportHistoryRecord for Guid: {id}", id);
                throw new KeyNotFoundException($"No ImportHistory record found for id: {id}");
            }

            return record;
        }

    }
}
