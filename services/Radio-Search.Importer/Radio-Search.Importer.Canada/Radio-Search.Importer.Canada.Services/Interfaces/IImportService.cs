using Radio_Search.Importer.Canada.Data.Models.History;
using Radio_Search.Importer.Canada.Data.Models.License;
using Radio_Search.Importer.Canada.Services.Data;
using Radio_Search.Importer.Canada.Services.Data.Enums;
using Radio_Search.Importer.Canada.Services.Responses;

namespace Radio_Search.Importer.Canada.Services.Interfaces
{
    /// <summary>
    /// Provides methods for processing and saving TAFL data.
    /// </summary>
    /// <remarks>This service interface defines operations for handling TAFL data, including processing CSV
    /// input and saving records to a database. Implementations should ensure that the provided methods handle data
    /// efficiently and correctly.</remarks>
    public interface IImportService
    {
        /// <summary>
        /// Begins a full TAFL Import. This will trigger all corresponding functions and should be the only entry point for an import.
        /// </summary>
        /// <param name="stream">The input stream containing the TAFL CSV data. Must be readable and positioned at the start of the CSV
        /// content.</param>
        /// <returns>A response containing metadata pertaining to the import.</returns>
        Task<BeginTAFLImportResponse> BeginTAFLImport(Stream stream);

        /// <summary>
        /// Processes a TAFL CSV file from the provided stream and returns the result.
        /// </summary>
        /// <remarks>The method reads and processes the CSV data from the provided stream. Ensure that the
        /// stream is open and readable before calling this method.</remarks>
        /// <param name="stream">The input stream containing the TAFL CSV data. Must be readable and positioned at the start of the CSV
        /// content.</param>
        /// <returns>A <see cref="ProcessTAFLCsvResponse"/> object containing the results of the CSV processing, including any
        /// errors encountered.</returns>
        ProcessTAFLCsvResponse ExtractTAFLRawRowsFromCSV(Stream stream);

        /// <summary>
        /// Retrieves a collection of actions associated with each raw row entry.
        /// </summary>
        /// <param name="records">A list of raw row entries for which actions are to be retrieved. Cannot be null or empty.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a <see
        /// cref="GetActionsPerRawRowResponse"/> object with the actions for each raw row entry.</returns>
        Task<GetActionsPerRawRowResponse> GetActionsPerRawRow(List<TAFLEntryRawRow> records);

        /// <summary>
        /// Asynchronously saves the specified actions per row to the database associated with the given import record
        /// identifier.
        /// </summary>
        /// <param name="actionsPerRow">The actions per row data to be saved. This parameter cannot be null.</param>
        /// <param name="importRecordGuid">The unique identifier for the import record to which the actions are associated.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains a <see
        /// cref="SaveTAFLToDBResponse"/> indicating the outcome of the save operation.</returns>
        Task<SaveTAFLToDBResponse> SaveTAFLToDBAsync(GetActionsPerRawRowResponse actionsPerRow, int importRecordId);

        /// <summary>
        /// Processes a TAFL file definition from the provided PDF stream.
        /// </summary>
        /// <remarks>The method reads and processes the TAFL definition from the specified stream,
        /// returning a response that includes the outcome of the processing. Ensure the stream is positioned at the
        /// beginning of the TAFL definition before calling this method.</remarks>
        /// <param name="stream">The input stream containing the TAFL definition to be processed. Must not be null.</param>
        /// <returns>A <see cref="ProcessTAFLDefinitionResponse"/> object containing the results of the processing operation.</returns>
        ProcessTAFLDefinitionResponse ProcessTAFLDefinition(Stream stream);

        /// <summary>
        /// Saves the specified table definitions and their associated rows to the database.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to persist the provided table
        /// definitions and their rows into the database. Ensure that the <paramref name="Tables"/> parameter is not
        /// <see langword="null"/> and contains valid data before calling this method.</remarks>
        /// <param name="Tables">A dictionary where each key is a <see cref="TAFLDefinitionTableEnum"/> object representing a table definition, and
        /// each value is a <see cref="HashSet{T}"/> of <see cref="TAFLDefinitionRawRow"/> objects representing the rows
        /// to be saved.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains a <see
        /// cref="SaveTAFLDefinitionToDBResponse"/> indicating the outcome of the save operation.</returns>
        Task<SaveTAFLDefinitionToDBResponse> SaveTAFLDefinitionToDBAsync(Dictionary<TAFLDefinitionTableEnum, HashSet<TAFLDefinitionRawRow>> Tables);

        /// <summary>
        /// Creates a new import history record using the specified file hash.
        /// </summary>
        /// <param name="fileHash">The hash of the file to be recorded. Must not be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created <see
        /// cref="ImportHistory"/> record.</returns>
        Task<ImportHistory> CreateImportHistoryRecord(byte[] fileHash);

        /// <summary>
        /// Converts List of Raw Rows to dictionary with the lookup key being the license ID.
        /// This will allow for faster lookup.
        /// </summary>
        /// <param name="taflRows">Rows to be converted to dictionary entries</param>
        /// <returns></returns>
        Dictionary<string, TAFLEntryRawRow> TAFLListToDictionary(List<TAFLEntryRawRow> taflRows);

        /// <summary>
        /// Get the rows that are to be updated
        /// </summary>
        /// <param name="inputRows">The rows that are in</param>
        /// <param name="dbRows"></param>
        /// <returns></returns>
        List<TAFLEntryRawRow> GetUpdatedRowsFromRange(Dictionary<string, TAFLEntryRawRow> inputRows, List<LicenseRecord> dbRows);

        /// <summary>
        /// Get the rows that are to be deleted
        /// </summary>
        /// <param name="inputRows">The rows that are in</param>
        /// <param name="dbRows"></param>
        /// <returns></returns>
        List<LicenseRecord> GetDeletedRowsFromRange(Dictionary<string, TAFLEntryRawRow> inputRows, List<LicenseRecord> dbRows);


        /// <summary>
        /// Get the rows that are unaffected
        /// </summary>
        /// <param name="inputRows">The rows that are in the CSV</param>
        /// <param name="dbRows">The rows from the DB, this is only a small subset of the actual rows</param>
        /// <returns></returns>
        HashSet<string> GetUnaffectedRowsFromRange(Dictionary<string, TAFLEntryRawRow> inputRows, List<LicenseRecord> dbRows);

        /// <summary>
        /// Returns a list of rows that are not deleted or updated, we use this method to reduce the strain on the DB
        /// as we can infer what has been created from the Two other fields
        /// </summary>
        /// <param name="newRows">All the rows from the Current CSV Import</param>
        /// <param name="updatedRows">All the rows that are being Updated</param>
        /// <param name="DeletedRows">All the rows that are being Deleted</param>
        /// <returns></returns>
        List<TAFLEntryRawRow> GetCreatedRows(Dictionary<string, TAFLEntryRawRow> rawRows, GetAffectedRowsResponse affectedRowResponse);

        /// <summary>
        /// Marks the import operation identified by the specified GUID as failed.
        /// </summary>
        /// <remarks>This method updates the status of the import operation in the system to indicate
        /// failure. Ensure that the provided <paramref name="guid"/> corresponds to a valid import operation.</remarks>
        /// <param name="guid">The unique identifier of the import operation to mark as failed.</param>
        /// <returns></returns>
        Task MarkImportAsFailed(int Id);

        /// <summary>
        /// Removes duplicate entries from the provided list of raw rows.
        /// </summary>
        /// <remarks>This method compares entries based on their inherent properties to determine
        /// uniqueness. The order of the original list is preserved in the returned list.</remarks>
        /// <param name="rows">The list of raw rows to be processed for duplicates.</param>
        /// <returns>A list of <see cref="TAFLEntryRawRow"/> containing only unique entries from the input list.</returns>
        List<TAFLEntryRawRow> DeduplicateRows(List<TAFLEntryRawRow> rows);
    }
}
