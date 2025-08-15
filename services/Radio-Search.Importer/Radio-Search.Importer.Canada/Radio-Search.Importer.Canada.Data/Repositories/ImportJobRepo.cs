using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Radio_Search.Importer.Canada.Data.Models.History;
using Radio_Search.Importer.Canada.Data.Models.ImportInfo;
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
        public async Task BulkInsertLicenseRecordHistory(List<LicenseRecordHistory> licenseRecordHistories)
        {
            await _context.BulkInsertAsync(licenseRecordHistories);
        }

        public async Task<ImportJobChunkFile> GetImportJobChunkFile(int importJobID, int fileID)
        {
            var record = await _context.ImportJobChunkFiles.FirstOrDefaultAsync(x => x.ImportJobID == importJobID && x.FileID == fileID);

            if (record is null)
            {
                _logger.LogError("Failed to find a ImportJobChunkFile for ImportJobID: {jobId} and FileID {fileID}", importJobID, fileID);
                throw new KeyNotFoundException($"No ImportJobChunkFile record found for id: {importJobID},{fileID}");
            }

            return record;
        }

        /// <inheritdoc/>
        public async Task<ImportJob> GetImportJobRecord(int id)
        {
            var record = await _context.Importobs.FirstOrDefaultAsync(x => x.ImportJobID == id);

            if(record is null)
            {
                _logger.LogError("Failed to find a ImportHistoryRecord for Guid: {id}", id);
                throw new KeyNotFoundException($"No ImportHistory record found for id: {id}");
            }

            return record;
        }

        public async Task<ImportJobStats> GetImportJobStats(int importJobID)
        {
            var record = await _context.ImportJobStats.FirstOrDefaultAsync(x => x.ImportJobID == importJobID);

            if (record is null)
            {
                _logger.LogError("Failed to find a ImportJobStats for Guid: {id}", importJobID);
                throw new KeyNotFoundException($"No ImportJobStats record found for id: {importJobID}");
            }

            return record;
        }

        public async Task<ImportJobChunkFile> UpsertImportJobChunkFileRecord(ImportJobChunkFile fileRecord)
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
        public async Task<ImportJob> UpsertImportJobRecord(ImportJob importHistory)
        {
            var exists = await _context.Importobs.AnyAsync(x => x.ImportJobID == importHistory.ImportJobID);

            if (exists)
            {
                _context.Importobs.Update(importHistory);
            }
            else
            {
                await _context.Importobs.AddAsync(importHistory);
            }

            await _context.SaveChangesAsync();
            await _context.Entry(importHistory).ReloadAsync();
            return importHistory;
        }

        public async Task<ImportJobStats> CreateImportJobStats(int importJobID)
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

        public Task IncrementStatsField(int importJobID, Expression<Func<ImportJobStats, int>> fieldSelector, int increaseAmount)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<string>> GetAllLicenseIDsFromImport(int importJobID)
        {
            return await _context.LicenseRecordsHistory
                .Where(x => x.EditedByImportJobID == importJobID)
                .Select(x => x.CanadaLicenseRecordID)
                .ToListAsync();
        }
    }
}
