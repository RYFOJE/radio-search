namespace Radio_Search.Importer.Canada.Services.Responses
{
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
