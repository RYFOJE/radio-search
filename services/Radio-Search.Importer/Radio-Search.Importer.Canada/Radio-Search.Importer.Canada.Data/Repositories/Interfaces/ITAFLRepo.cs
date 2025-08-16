using Radio_Search.Importer.Canada.Data.Models.License;
using EFCore.BulkExtensions;

namespace Radio_Search.Importer.Canada.Data.Repositories.Interfaces
{

    /// <summary>
    /// Repo to interact with the TAFL License Records
    /// </summary>
    public interface ITAFLRepo
    {

        /// <summary>
        /// Gets a range of TAFL Records
        /// <remarks>The total amount of records capable of being returned is limited
        /// to the count set by key Import:RecordFetchLimit</remarks>
        /// </summary>
        /// The order in which these records are being returned are based on the primary key (License ID)
        /// <param name="skipCount">The amount of records to skip</param>
        /// <param name="takeCount">The amount of records to return</param>
        /// <returns></returns>
        Task<List<LicenseRecord>> GetRecordsNoTrackingAsync(int skipCount, int takeCount, bool isValidOnly = true);

        /// <summary>
        /// Bulk set the IsValid field to false for all records.
        /// </summary>
        /// <param name="licenseIDs">A list of Canadian Licenses to invalidate.</param>
        /// <returns></returns>
        Task BulkInvalidateRecords(List<string> licenseIDs);

        /// <summary>
        /// Adds multiple license records to the system asynchronously.
        /// </summary>
        /// <remarks>This method processes the provided list of license records in bulk. Ensure that the
        /// list is not null or empty, and that each <see cref="LicenseRecord"/> in the list contains valid data. If any
        /// record is invalid, the operation may fail or partially complete.</remarks>
        /// <param name="records">A list of <see cref="LicenseRecord"/> objects to be added. Each record must be valid and non-null.</param>
        /// <returns>A task that represents the asynchronous operation. The task completes when all records have been processed.</returns>
        Task BulkAddLicenseRecordsAsync(List<LicenseRecord> records);

        /// <summary>
        /// Retrieves a collection of license records corresponding to the specified license IDs without tracking
        /// changes in the database context.
        /// </summary>
        /// <remarks>This method performs a no-tracking query, meaning the returned entities are not
        /// tracked by the database context. This is useful for read-only operations where tracking is unnecessary and
        /// can improve performance.</remarks>
        /// <param name="licenseIDs">A list of license IDs for which the license records should be fetched. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see
        /// cref="LicenseRecord"/> objects corresponding to the provided license IDs. If no records are found, the
        /// result will be an empty collection.</returns>
        Task<List<LicenseRecord>> BulkFetchLicenseRecordsNoTrackingAsync(List<string> licenseIDs);


        /// <summary>
        /// Retrieves the version numbers associated with the specified license records.
        /// </summary>
        /// <remarks>The returned dictionary will only include entries for license records that exist and
        /// have associated version numbers. If a record ID does not exist or has no version, it will not appear in the
        /// dictionary.</remarks>
        /// <param name="recordIds">A list of license record identifiers for which to retrieve version numbers. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a dictionary where the keys are
        /// license record identifiers and the values are their corresponding version numbers.</returns>
        Task<Dictionary<string, int>> GetVersionsForLicenseRecords(List<string> recordIds);
    }
}
