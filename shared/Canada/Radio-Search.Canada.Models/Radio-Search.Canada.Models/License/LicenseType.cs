using System.ComponentModel.DataAnnotations;

namespace Radio_Search.Canada.Models.License
{
    public class LicenseType : MultiLanguageEntry
    {

        [MaxLength(7)]
        public string LicenseTypeID { get; set; } = string.Empty;
    }
}
