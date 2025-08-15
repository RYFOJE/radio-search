using Radio_Search.Importer.Canada.Data.Enums;

namespace Radio_Search.Importer.Canada.Data.Models.ImportInfo
{
    public class ImportJobChunkFile : TimeTrackedEntry
    {
        public int ImportJobID { get; set; } // Composite key
        public int FileID { get; set; } // Composite Key
        public FileStatus Status { get; set; } = FileStatus.Unprocessed;
        public ImportJob? ImportJob { get; set; }
    }
}
