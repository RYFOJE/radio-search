namespace Radio_Search.Canada.Models.ImportInfo
{
    public class ImportJobStats : DatabaseEntry
    {
        public int ImportJobID { get; set; }

        /// <summary>
        /// Rows skipped during pre-processing. These rows are skipped due to malformed data
        /// or any type of data that is not expected.
        /// </summary>
        public int PreprocessingSkippedRows { get; set; } = 0;
        
        public int NewRecordCount { get; set; } = 0;
        public int UpdatedRecordCount { get; set; } = 0;
        public int DeletedRecordCount { get; set; } = 0;
    }
}
