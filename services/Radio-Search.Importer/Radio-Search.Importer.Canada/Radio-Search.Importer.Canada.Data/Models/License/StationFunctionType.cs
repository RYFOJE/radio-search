using System.ComponentModel.DataAnnotations;

namespace Radio_Search.Importer.Canada.Data.Models.License
{
    public class StationFunctionType : MultiLanguageEntry
    {
        [MaxLength(3)]
        public string StationFunctionTypeID { get; set; } = string.Empty;
    }
}
