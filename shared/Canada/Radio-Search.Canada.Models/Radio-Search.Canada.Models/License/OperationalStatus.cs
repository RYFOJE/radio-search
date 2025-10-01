using System.ComponentModel.DataAnnotations;

namespace Radio_Search.Canada.Models.License
{
    public class OperationalStatus : MultiLanguageEntry
    {
        [MaxLength(5)]
        public string OperationalStatusID { get; set; } = string.Empty;
    }
}
