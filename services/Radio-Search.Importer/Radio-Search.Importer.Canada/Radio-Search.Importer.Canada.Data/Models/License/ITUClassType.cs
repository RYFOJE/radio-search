using System.ComponentModel.DataAnnotations;

namespace Radio_Search.Importer.Canada.Data.Models.License
{
    public class ITUClassType : MultiLanguageEntry
    {
        [MaxLength(3)]
        public string ITUClassTypeID { get; set; } = string.Empty;
    }
}
