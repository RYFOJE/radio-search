using Radio_Search.Importer.Canada.Data.Enums;
using Radio_Search.Importer.Canada.Data.Models.ImportInfo;
using Radio_Search.Importer.Canada.Data.Models.License;

namespace Radio_Search.Importer.Canada.Data.Models.History
{
    public class LicenseRecordHistory : DatabaseEntry
    {
        public int LicenseRecordHistoryId { get; set; }

        // FK, Composite
        public string CanadaLicenseRecordID { get; set; } = string.Empty;
        public int Version { get; set; } = 1;


        public LicenseRecord? LicenseRecord { get; set; }
        public ChangeType ChangeType { get; set; }
        public ImportJob? EditedByImportJob { get; set; }
        public int? EditedByImportJobID { get; set; }
        public int? EditedByUserID { get; set; }
    }
}