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
        /// <param name="licenseIDs">A list of Canadadian Licenses to invalidate.</param>
        /// <returns></returns>
        Task BulkInvalidateRecords(List<string> licenseIDs);

        Task BulkAddLicenseRecordsAsync(List<LicenseRecord> records);

    }
}
