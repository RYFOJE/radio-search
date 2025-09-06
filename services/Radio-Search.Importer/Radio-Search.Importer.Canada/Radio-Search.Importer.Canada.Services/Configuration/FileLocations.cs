namespace Radio_Search.Importer.Canada.Services.Configuration
{
    public class FileLocations
    {
        public string UnprocessedTAFLDefinition { get; set; } = string.Empty;
        public string UnprocessedTAFLRows { get; set; } = string.Empty;
        public string SeenIDs { get; set; } = string.Empty;

        /// <summary>
        /// Has two variables. 1st is the ImportJobID 2nd is the Chunk File ID
        /// </summary>
        public string ChunkFile { get; set; } = string.Empty;
    }
}
