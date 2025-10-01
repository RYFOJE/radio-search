namespace Radio_Search.Canada.Models
{
    public abstract class TimeTrackedEntry : DatabaseEntry
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
