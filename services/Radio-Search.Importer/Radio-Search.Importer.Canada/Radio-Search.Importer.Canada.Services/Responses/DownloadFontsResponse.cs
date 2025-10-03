namespace Radio_Search.Importer.Canada.Services.Responses
{
    public class DownloadFontsResponse
    {
        public List<string> DownloadedFontPaths { get; set; } = [];
        public List<string> DownloadedFonts { get; set; } = [];
        public string DownloadedFontsRootDirectory { get; set; } = "";
        public int TotalFontsDownloaded { get; set; } = 0;
    }
}
