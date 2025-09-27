namespace Radio_Search.Querying.Gateway.Services.Configurations
{


    /// <summary>
    /// File locations for any files in the Blob storage. This is meant to be a strongly typed
    /// configuration to load these locations in.
    /// </summary>
    public class BlobFileLocations
    {
        /// <summary>
        /// File location containing uncompressed Natural Earth files
        /// </summary>
        public string UnzippedNaturalEarth { get; set; } = string.Empty;

    }
}
