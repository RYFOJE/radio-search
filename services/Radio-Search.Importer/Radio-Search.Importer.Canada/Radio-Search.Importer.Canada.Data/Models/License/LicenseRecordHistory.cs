using Radio_Search.Importer.Canada.Data.Enums;
using Radio_Search.Importer.Canada.Data.Models.History;

namespace Radio_Search.Importer.Canada.Data.Models.License
{
    public class LicenseRecordHistory
    {
        public int LicenseRecordHistoryID { get; set; }
        public LicenseRecord? LicenseRecord { get; set; }
        public ImportHistory? ImportHistoryRecord { get; set; }
        public ChangeType ChangeType { get; set; }

        /// <summary>
        /// The representation of the object previous state
        /// </summary>
        public string? JSONRepresentation { get; set; }
    }
}