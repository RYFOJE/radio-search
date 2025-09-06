namespace Radio_Search.Importer.Canada.Data.Models
{
    public abstract class TimeTrackedEntry : DatabaseEntry
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
