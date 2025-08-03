using Radio_Search.Importer.Canada.Data.Enums;
using Radio_Search.Importer.Canada.Data.Models.License;

namespace Radio_Search.Importer.Canada.Data.Models.History
{
    public class LicenseRecordHistory
    {
        public int LicenseRecordHistoryID { get; set; }
        public Guid InternalLicenseRecordID { get; set; }
        public LicenseRecord? LicenseRecord { get; set; }
        public ChangeType ChangeType { get; set; }
        public ImportHistory? EditedByImportHistoryRecord { get; set; }
        public int? EditedByImportHistoryRecordID { get; set; }
        public int? EditedByUserID { get; set; }
    }
}