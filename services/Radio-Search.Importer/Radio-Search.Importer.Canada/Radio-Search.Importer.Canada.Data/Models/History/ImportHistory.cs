using Radio_Search.Importer.Canada.Data.Enums;
using Radio_Search.Importer.Canada.Data.Models.License;

namespace Radio_Search.Importer.Canada.Data.Models.History
{
    public class ImportHistory
    {
        public Guid ImportHistoryID { get; set; } = Guid.NewGuid();
        public DateTime StartTime {  get; set; } = DateTime.UtcNow;
        public DateTime? EndTime { get; set; }
        public ImportStatus Status { get; set; } = ImportStatus.Pending;

        public int? ImportRowCount { get; set; }
        public int SkippedRowCount { get; set; }

        public List<LicenseRecordHistory> AssociatedRecords { get; set; } = new();
    }
}
