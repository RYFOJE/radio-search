namespace Radio_Search.Importer.Canada.Services.Responses
{
    /// <summary>
    /// Base of any Response. Can be successful or failed
    /// </summary>
    public class ResponseBase
    {
        /// <summary>
        /// Status of the response
        /// </summary>
        public bool Success { get; set; } = false;

        /// <summary>
        /// Message regarding the download
        /// </summary>
        public string? Message { get; set; }
    }
}
