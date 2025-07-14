using Radio_Search.Importer.Canada.Services.Data;
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

        Task SaveTAFLToDB(List<BroadcastAuthorizationRecord> records);
    }
}
