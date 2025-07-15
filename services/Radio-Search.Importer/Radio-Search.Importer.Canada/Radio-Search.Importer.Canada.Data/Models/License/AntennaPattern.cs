using System.ComponentModel.DataAnnotations;

namespace Radio_Search.Importer.Canada.Data.Models.License
{
    public class AntennaPattern : MultiLanguageEntry
    {
        [MaxLength(15)]
        public string AntennaPatternID { get; set; } = string.Empty;

    }
}
