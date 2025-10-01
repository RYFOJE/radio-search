using Radio_Search.Canada.Models.Enums;
using Radio_Search.Canada.Models.History;

namespace Radio_Search.Canada.Models.ImportInfo
{
    public class ImportJob : TimeTrackedEntry
    {
        public int ImportJobID { get; set; }
        
        /// <summary>
        /// The status of the import job
        /// </summary>
        public ImportStatus Status { get; set; } = ImportStatus.Pending;

        /// <summary>
        /// The current step of the import
        /// </summary>
        public ImportStep CurrentStep { get; set; } = ImportStep.DownloadingFiles;

        /// <summary>
        /// The import job's associated stats
        /// </summary>
        public ImportJobStats Stats { get; set; } = new();

        /// <summary>
        /// Associated License Record Histories
        /// </summary>
        public List<LicenseRecordHistory> AssociatedLicenseRecordHistories { get; set; } = [];

        /// <summary>
        /// All associated Unprocessed Chunk Files
        /// </summary>
        public List<ImportJobChunkFile> ImportJobChunkFiles { get; set; } = [];
    }
}
