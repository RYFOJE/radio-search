namespace Radio_Search.Importer.Canada.Services.Configuration
{
    /// <summary>
    /// Configuration containing all Fonts required for importer to function.
    /// </summary>
    public class RequiredFonts
    {
        public List<Font> Fonts { get; set; } = [];
    }

    /// <summary>
    /// Font download configuration
    /// </summary>
    public class Font
    {

        /// <summary>
        /// FileName once the file is downloaded
        /// </summary>
        public required string FileName { get; set; }

        /// <summary>
        /// The URL Containing the font resource to fetch
        /// </summary>
        public required string DownloadUrl { get; set; }
    }
}
