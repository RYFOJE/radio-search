using Radio_Search.Importer.Canada.Services.Data;

namespace Radio_Search.Importer.Canada.Services.Responses
{
    /// <summary>
    /// Represents the response from processing a TAFL CSV file, including successfully processed records and counts of
    /// any errors encountered.
    /// </summary>
    public class ProcessTAFLCsvResponse : ResponseBase
    {
        /// <summary>
        /// List of processed records that were found inside of the TAFL File
        /// </summary>
        public List<BroadcastAuthorizationRecord> Data { get; set; } = new();

        /// <summary>
        /// Amount of skipped rows because they had the incorrect amount of rows
        /// </summary>
        public int ColumnMismatchCount { get; set; }

        /// <summary>
        /// Amount of skipped rows due to processing errors
        /// </summary>
        public int BadDataCount { get; set; }
    }
}
