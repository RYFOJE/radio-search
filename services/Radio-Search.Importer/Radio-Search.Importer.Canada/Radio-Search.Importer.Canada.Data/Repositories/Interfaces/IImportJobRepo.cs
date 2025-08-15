using Radio_Search.Importer.Canada.Data.Models.History;
using Radio_Search.Importer.Canada.Data.Models.ImportInfo;
using System.Linq.Expressions;

namespace Radio_Search.Importer.Canada.Data.Repositories.Interfaces
{
    public interface IImportJobRepo
    {
        /// <summary>
        /// Inserts a new import history record or updates an existing one based on the provided data.
        /// </summary>
        /// <remarks>If the record already exists, it will be updated with the provided data. Otherwise, a
        /// new record will be created.</remarks>
        /// <param name="importHistory">The import history record to insert or update. Must contain valid data, including a unique identifier to
        /// determine whether the record should be created or updated.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated or newly created
        /// <see cref="ImportJob"/> object.</returns>
        Task<ImportJob> UpsertImportJobRecord(ImportJob importHistory);

        /// <summary>
        /// Bulk inserts License Record History rows
        /// </summary>
        /// <param name="licenseRecordHistories"></param>
        /// <returns></returns>
        Task BulkInsertLicenseRecordHistory(List<LicenseRecordHistory> licenseRecordHistories);

        /// <summary>
        /// Gets an import record from ID.
        /// </summary>
        /// <param name="id">The ID used for the search</param>
        /// <returns></returns>
        Task<ImportJob> GetImportJobRecord(int id);

        /// <summary>
        /// Gets an Import Job Chunk File
        /// </summary>
        /// <param name="ImportJobID">The Job ID associated to the record</param>
        /// <param name="FileID">The File ID Associated to the Chunk File</param>
        /// <returns>The Chunk file if found</returns>
        Task<ImportJobChunkFile> GetImportJobChunkFile(int ImportJobID, int FileID);

        /// <summary>
        /// Inserts or updates a Job Chunk File Record
        /// </summary>
        /// <param name="fileRecord">The record to insert or update</param>
        /// <returns></returns>
        Task<ImportJobChunkFile> UpsertImportJobChunkFileRecord(ImportJobChunkFile fileRecord);

        /// <summary>
        /// Gets import job stats for an import job
        /// </summary>
        /// <param name="ImportJobID">Import Job ID to get stats for</param>
        /// <returns>The import job stats</returns>
        Task<ImportJobStats> GetImportJobStats(int ImportJobID);

        /// <summary>
        /// Creates and retrieves statistics for the specified import job.
        /// </summary>
        /// <remarks>This method is asynchronous and does not block the calling thread. Ensure that the
        /// <paramref name="importJobID"/> corresponds to a valid and existing import job.</remarks>
        /// <param name="importJobID">The unique identifier of the import job for which statistics are to be created. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ImportJobStats"/>
        /// object with the statistics for the specified import job.</returns>
        Task<ImportJobStats> CreateImportJobStats(int importJobID);

        /// <summary>
        /// Increments a specified integer field in the statistics of an import job by a given amount.
        /// </summary>
        /// <param name="importJobID">The unique identifier of the import job whose statistics field is to be updated.</param>
        /// <param name="fieldSelector">An expression that specifies the integer field in the <see cref="ImportJobStats"/> object to be incremented.</param>
        /// <param name="increaseAmount">The amount by which to increment the specified field. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task completes when the field has been successfully
        /// incremented.</returns>
        Task IncrementStatsField(int importJobID, Expression<Func<ImportJobStats, int>> fieldSelector, int increaseAmount);

        /// <summary>
        /// Retrieves all license IDs associated with the specified import job.
        /// </summary>
        /// <param name="importJobID">The unique identifier of the import job for which to retrieve license IDs.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of
        /// license IDs. If no license IDs are found, the collection will be empty.</returns>
        Task<IEnumerable<string>> GetAllLicenseIDsFromImport(int importJobID);
    }
}
