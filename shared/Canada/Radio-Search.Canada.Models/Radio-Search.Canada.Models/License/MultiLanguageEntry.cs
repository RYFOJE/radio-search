using Radio_Search.Canada.Models;

namespace Radio_Search.Canada.Models.License
{
    public class MultiLanguageEntry : DatabaseEntry
    {
        public string DescriptionFR { get; set; } = string.Empty;
        public string DescriptionEN { get; set; } = string.Empty;
    }
}
