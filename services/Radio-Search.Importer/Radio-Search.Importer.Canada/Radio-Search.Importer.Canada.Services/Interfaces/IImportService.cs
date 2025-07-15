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
        /// Processes a TAFL CSV file from the provided stream and returns the result.
        /// </summary>
        /// <remarks>The method reads and processes the CSV data from the provided stream. Ensure that the
        /// stream is open and readable before calling this method.</remarks>
        /// <param name="stream">The input stream containing the TAFL CSV data. Must be readable and positioned at the start of the CSV
        /// content.</param>
        /// <returns>A <see cref="ProcessTAFLCsvResponse"/> object containing the results of the CSV processing, including any
        /// errors encountered.</returns>
        ProcessTAFLCsvResponse ProcessTAFLCsv(Stream stream);

        /// <summary>
        /// Saves a list of broadcast authorization records to the database asynchronously.
        /// </summary>
        /// <param name="records">The list of <see cref="BroadcastAuthorizationRecord"/> objects to be saved. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task<SaveTAFLToDBResponse> SaveTAFLToDBAsync(List<BroadcastAuthorizationRecord> records);

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
        /// <param name="Tables">A dictionary where each key is a <see cref="TableDefinitions"/> object representing a table definition, and
        /// each value is a <see cref="HashSet{T}"/> of <see cref="TableDefinitionRow"/> objects representing the rows
        /// to be saved.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains a <see
        /// cref="SaveTAFLDefinitionToDBResponse"/> indicating the outcome of the save operation.</returns>
        Task<SaveTAFLDefinitionToDBResponse> SaveTAFLDefinitionToDBAsync(Dictionary<TableDefinitions, HashSet<TableDefinitionRow>> Tables);
    }
}
