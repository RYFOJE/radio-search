using Radio_Search.Importer.Canada.Data.Enums;

namespace Radio_Search.Importer.Canada.Data.Models.History
{
    public class ImportHistory
    {
        public int ImportHistoryID { get; set; }
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public DateTime? EndTime { get; set; }
        public byte[] FileHash { get; set; } = [];
        public ImportStatus Status { get; set; } = ImportStatus.Pending;

        public int? TotalInsertedUpdatedRows { get; set; }
        public int SkippedRowCount { get; set; }

        public List<LicenseRecordHistory> AssociatedRecords { get; set; } = new();
    }
}
