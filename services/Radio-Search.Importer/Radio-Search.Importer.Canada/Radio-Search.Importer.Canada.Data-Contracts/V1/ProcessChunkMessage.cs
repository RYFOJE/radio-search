namespace Radio_Search.Importer.Canada.Data_Contracts.V1
{
    /// <summary>
    /// A message that represents a chunked CSV that is ready to be processed.
    /// </summary>
    public class ProcessChunkMessage
    {
        /// <summary>
        /// The associated ImportJobID.
        /// </summary>
        public int ImportJobID { get; set; }

        /// <summary>
        /// The chunks unique ID for that import.
        /// </summary>
        public int FileID { get; set; } 

        /// <summary>
        /// The location of the file chunk.
        /// </summary>
        public required Uri FileLocation { get; set; }
    }
}
