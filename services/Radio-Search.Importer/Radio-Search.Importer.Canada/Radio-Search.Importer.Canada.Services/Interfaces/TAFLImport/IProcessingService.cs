using Radio_Search.Importer.Canada.Services.Data;
using Radio_Search.Importer.Canada.Services.Responses;

namespace Radio_Search.Importer.Canada.Services.Interfaces.TAFLImport
{
    public interface IProcessingService
    {

        /// <summary>
        /// Returns the rows from the TAFL Raw file that need to be inserted and updated
        /// </summary>
        /// <returns>A task containing which rows from the CSV will need to be inserted and updated into the DB.</returns>
        Task<GetInsertsAndUpdatesResponse> GetInsertsAndUpdates(List<TAFLEntryRawRow> rows);

        /// <summary>
        /// Inserts new records into the database based on the provided raw data rows.
        /// </summary>
        /// <remarks>This method processes the raw data rows to extract and validate the necessary
        /// information before inserting them into the database. Ensure that the provided rows meet the required format
        /// and constraints to avoid errors during processing.</remarks>
        /// <param name="rows">A list of raw data rows to be processed and inserted. Each row must contain valid data required for
        /// insertion. The list cannot be null or empty.</param>
        /// <param name="importID">The import job associated to the Import</param>
        /// <returns>A task that represents the asynchronous operation. The task completes when all valid rows have been
        /// processed and inserted.</returns>
        Task InsertNewFromRawRecords(List<TAFLEntryRawRow> rows, int importID);

        /// <summary>
        /// Runs once Inserts and updates have been performed. Use this to see all IDs that are no longer in the Import File
        /// </summary>
        /// <param name="importID">The import Job ID</param>
        /// <returns>List of License Record IDs that are not in the Import</returns>
        Task<List<string>> GetDeletedRecords(int importID);

        /// <summary>
        /// Inserts or updates records in the database based on the provided raw data rows.
        /// </summary>
        /// <remarks>This method processes the provided raw data rows and determines whether each record
        /// should be inserted as new or updated if it already exists. Ensure that the input list is not null and
        /// contains valid data rows.</remarks>
        /// <param name="rows">A list of raw data rows to be processed. Each row represents a record to be inserted or updated.</param>
        /// <param name="importID">The import job associated to the Import</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task InsertUpdatedFromRawRecords(List<(int version, TAFLEntryRawRow row)> rows, int importID);

        Task InvalidateRecordsFromDB(List<string> recordIDs, int importId);


    }
}
