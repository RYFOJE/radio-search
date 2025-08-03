using Radio_Search.Importer.Canada.Data.Models.History;

namespace Radio_Search.Importer.Canada.Data.Repositories.Interfaces
{
    public interface ITAFLImportHistoryRepo
    {

        /// <summary>
        /// Creates an import history record. This is used to track the start of an import.
        /// </summary>
        /// <param name="record">The record to be created</param>
        /// <returns></returns>
        Task<ImportHistory> CreateImportHistoryRecord(ImportHistory record);

        /// <summary>
        /// Updates an import history record.
        /// </summary>
        /// <param name="record">The record to update</param>
        Task<ImportHistory> UpdateImportHistoryRecord(ImportHistory record);

        /// <summary>
        /// Bulk inserts License Record History rows
        /// </summary>
        /// <param name="licenseRecordHistories"></param>
        /// <returns></returns>
        Task BulkInsertLicenseRecordHistory(List<LicenseRecordHistory> licenseRecordHistories);

        /// <summary>
        /// Gets an import record from GUID.
        /// </summary>
        /// <param name="recordGuid">The GUID used for the search</param>
        /// <returns></returns>
        Task<ImportHistory> GetImportHistoryRecord(int id);

    }
}
