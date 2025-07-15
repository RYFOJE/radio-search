namespace Radio_Search.Importer.Canada.Services.Responses
{
    /// <summary>
    /// Represents the response received after a file download operation.
    /// </summary>
    /// <remarks>This class provides details about the downloaded file, including its name and full
    /// path.</remarks>
    public class DownloadFileResponse : ResponseBase
    {
        /// <summary>
        /// The name of the file downloaded
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// The full path of the file
        /// </summary>
        public Uri? FullPath { get; set; }
    }
}
