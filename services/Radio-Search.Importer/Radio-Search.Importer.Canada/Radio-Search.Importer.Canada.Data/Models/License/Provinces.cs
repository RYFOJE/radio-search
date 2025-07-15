using System.ComponentModel.DataAnnotations;

namespace Radio_Search.Importer.Canada.Data.Models.License
{
    public class Province : MultiLanguageEntry
    {
        [MaxLength(2)]
        public string ProvinceID { get; set; } = string.Empty;
    }
}
