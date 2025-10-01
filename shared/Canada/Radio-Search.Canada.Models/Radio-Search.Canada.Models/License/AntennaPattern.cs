using System.ComponentModel.DataAnnotations;

namespace Radio_Search.Canada.Models.License
{
    public class AntennaPattern : MultiLanguageEntry
    {
        [MaxLength(15)]
        public string AntennaPatternID { get; set; } = string.Empty;

    }
}
