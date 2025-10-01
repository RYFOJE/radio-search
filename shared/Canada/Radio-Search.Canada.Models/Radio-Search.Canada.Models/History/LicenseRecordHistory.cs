using Radio_Search.Canada.Models.Enums;
using Radio_Search.Canada.Models.ImportInfo;
using Radio_Search.Canada.Models.License;

namespace Radio_Search.Canada.Models.History
{
    public class LicenseRecordHistory : DatabaseEntry
    {
        public int LicenseRecordHistoryId { get; set; }

        // FK, Composite
        public int CanadaLicenseRecordID { get; set; }
        public int Version { get; set; } = 1;


        public LicenseRecord? LicenseRecord { get; set; }
        public ChangeType ChangeType { get; set; }
        public ImportJob? EditedByImportJob { get; set; }
        public int? EditedByImportJobID { get; set; }
        public int? EditedByUserID { get; set; }
    }
}