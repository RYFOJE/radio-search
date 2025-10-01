using System.ComponentModel.DataAnnotations;

namespace Radio_Search.Canada.Models.License
{
    public class CommunicationType : MultiLanguageEntry
    {
        [MaxLength(5)]
        public string CommunicationTypeID { get; set; } = string.Empty;
    }
}
