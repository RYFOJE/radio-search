using System.ComponentModel.DataAnnotations;

namespace Radio_Search.Importer.Canada.Data.Models.License
{
    public class AuthorizationStatus : MultiLanguageEntry
    {
        [MaxLength(3)]
        public string AuthorizationStatusID { get; set; } = string.Empty;
    }
}
