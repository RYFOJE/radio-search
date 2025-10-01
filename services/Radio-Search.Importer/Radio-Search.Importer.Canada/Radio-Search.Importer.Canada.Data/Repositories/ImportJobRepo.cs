using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Radio_Search.Canada.Models.Enums;
using Radio_Search.Canada.Models.History;
using Radio_Search.Canada.Models.ImportInfo;
using Radio_Search.Importer.Canada.Data.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Radio_Search.Importer.Canada.Data.Repositories
{
    public class ImportJobRepo : IImportJobRepo
    {
        private readonly CanadaImporterContext _context;
        private readonly ILogger<ImportJobRepo> _logger;

        public ImportJobRepo(
            CanadaImporterContext context,
            ILogger<ImportJobRepo> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public async Task BulkInsertLicenseRecordHistoryAsync(List<LicenseRecordHistory> licenseRecordHistories)
        {
            await _context.BulkInsertOrUpdateAsync(licenseRecordHistories, opt => {
                opt.BulkCopyTimeout = 400;
                opt.BatchSize = 5000;
                opt.EnableStreaming = true;
                opt.TrackingEntities = false;
            });
        }

        ///<inheritdoc/>
        public async Task<ImportJobChunkFile> GetImportJobChunkFileAsync(int importJobID, int fileID)
        {
            var record = await _context.ImportJobChunkFiles.FirstOrDefaultAsync(x => x.ImportJobID == importJobID && x.FileID == fileID);

            if (record is null)
            {
                _logger.LogError("Failed to find a ImportJobChunkFile for ImportJobID: {JobId} and FileID {FileID}", importJobID, fileID);
                throw new KeyNotFoundException($"No ImportJobChunkFile record found for id: {importJobID},{fileID}");
            }

            return record;
        }

        /// <inheritdoc/>
        public async Task<ImportJob> GetImportJobRecordAsync(int id)
        {
            var record = await _context.ImportJobs.FirstOrDefaultAsync(x => x.ImportJobID == id);

            if (record is null)
            {
                _logger.LogError("Failed to find a ImportHistoryRecord for Guid: {Id}", id);
                throw new KeyNotFoundException($"No ImportHistory record found for id: {id}");
            }

            return record;
        }

        ///<inheritdoc/>
        public async Task<ImportJobStats> GetImportJobStatsAsync(int importJobID)
        {
            var record = await _context.ImportJobStats.FirstOrDefaultAsync(x => x.ImportJobID == importJobID);

            if (record is null)
            {
                _logger.LogError("Failed to find a ImportJobStats for Guid: {Id}", importJobID);
                throw new KeyNotFoundException($"No ImportJobStats record found for id: {importJobID}");
            }

            return record;
        }

        ///<inheritdoc/>
        public async Task<ImportJobChunkFile> UpsertImportJobChunkFileRecordAsync(ImportJobChunkFile fileRecord)
        {
            var exists = await _context.ImportJobChunkFiles.AnyAsync(x => x.ImportJobID == fileRecord.ImportJobID && x.FileID == fileRecord.FileID);

            if (exists)
            {
                _context.ImportJobChunkFiles.Update(fileRecord);
            }
            else
            {
                await _context.ImportJobChunkFiles.AddAsync(fileRecord);
            }

            await _context.SaveChangesAsync();
            await _context.Entry(fileRecord).ReloadAsync();
            return fileRecord;
        }

        /// <inheritdoc/>
        public async Task<ImportJob> UpsertImportJobRecordAsync(ImportJob importHistory)
        {
            var exists = await _context.ImportJobs.AnyAsync(x => x.ImportJobID == importHistory.ImportJobID);

            if (exists)
            {
                _context.ImportJobs.Update(importHistory);
            }
            else
            {
                await _context.ImportJobs.AddAsync(importHistory);
            }

            await _context.SaveChangesAsync();
            await _context.Entry(importHistory).ReloadAsync();
            return importHistory;
        }

        ///<inheritdoc/>
        public async Task<ImportJobStats> CreateImportJobStatsAsync(int importJobID)
        {
            var importStats = new ImportJobStats { ImportJobID = importJobID };
            try
            {
                await _context.ImportJobStats.AddAsync(importStats);
                await _context.SaveChangesAsync();
                return importStats;
            }
            catch (DbUpdateException)
            {
                return await _context.ImportJobStats.FirstAsync(x => x.ImportJobID == importStats.ImportJobID);
            }
        }

        ///<inheritdoc/>
        public async Task IncrementStatsFieldAsync(int importJobID, Expression<Func<ImportJobStats, int>> selector, int increaseAmount)
        {
            var stats = await _context.ImportJobStats.FirstOrDefaultAsync(x => x.ImportJobID == importJobID);
            if (stats == null)
                throw new KeyNotFoundException($"No ImportJobStats record found for id: {importJobID}");

            var memberExpr = selector.Body as MemberExpression;
            if (memberExpr?.Member is not System.Reflection.PropertyInfo propInfo)
                throw new ArgumentException("Selector must be a property access");

            var currentValue = (int)propInfo.GetValue(stats)!;
            propInfo.SetValue(stats, currentValue + increaseAmount);

            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<int>> GetAllLicenseIDsFromImportAsync(int importJobID)
        {
            return await _context.LicenseRecordsHistory
                .Where(x => x.EditedByImportJobID == importJobID)
                .Select(x => x.CanadaLicenseRecordID)
                .ToListAsync();
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<int>> GetActiveLicensesNotFromImportAsync(int importJobID)
        {
            return await _context.LicenseRecords
                .Where(x => x.IsValid && !x.HistoryRecords.Any(y => y.EditedByImportJobID == importJobID))
                .Select(x => x.CanadaLicenseRecordID)
                .ToListAsync();
        }

        ///<inheritdoc/>
        public async Task<bool> IsChunkProcessingDoneAsync(int importJobID)
        {
            return !await _context.ImportJobChunkFiles
                .Where(x => x.ImportJobID == importJobID && x.Status != FileStatus.Processed)
                .AnyAsync();
        }
    }
}
