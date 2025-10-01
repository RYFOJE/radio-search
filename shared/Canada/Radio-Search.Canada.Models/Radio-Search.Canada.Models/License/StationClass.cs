using System.ComponentModel.DataAnnotations;

namespace Radio_Search.Canada.Models.License
{
    public class StationClass : MultiLanguageEntry
    {
        [MaxLength(5)]
        public string StationClassID { get; set; } = string.Empty;
    }
}
