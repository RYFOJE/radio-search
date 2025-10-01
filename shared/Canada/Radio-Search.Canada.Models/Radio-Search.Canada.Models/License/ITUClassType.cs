using System.ComponentModel.DataAnnotations;

namespace Radio_Search.Canada.Models.License
{
    public class ITUClassType : MultiLanguageEntry
    {
        [MaxLength(3)]
        public string ITUClassTypeID { get; set; } = string.Empty;
    }
}
