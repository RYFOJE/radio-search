using System.ComponentModel.DataAnnotations;

namespace Radio_Search.Importer.Canada.Data.Models.License
{
    public class LicenseType : MultiLanguageEntry
    {

        [MaxLength(7)]
        public string LicenseTypeID { get; set; } = string.Empty;
    }
}
