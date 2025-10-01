using System.ComponentModel.DataAnnotations;

namespace Radio_Search.Canada.Models.License
{
    public class ModulationType : MultiLanguageEntry
    {
        [MaxLength(25)]
        public string ModulationTypeID { get; set; } = string.Empty;
    }
}
