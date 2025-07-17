namespace Radio_Search.Importer.Canada.Services.Configuration
{
    /// <summary>
    /// Represents a collection of URLs used for downloading TAFL-related files and verifying update dates.
    /// </summary>
    public class DownloaderURLs
    {
        /// <summary>
        /// Location of TAFL Full CSV File.
        /// </summary>
        public string TAFLUrl { get; set; } = string.Empty;

        /// <summary>
        /// Location of TAFL Definition PDF File.
        /// </summary>
        public string TAFLDescriptionUrl { get; set; } = string.Empty;

        /// <summary>
        /// Location of the TAFL Download page, for last update date verification.
        /// </summary>
        public string DownloadPageUrl { get; set; } = string.Empty;
    }
}
