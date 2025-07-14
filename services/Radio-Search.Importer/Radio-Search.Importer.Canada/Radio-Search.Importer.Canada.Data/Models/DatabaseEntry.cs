using System.ComponentModel.DataAnnotations;

namespace Radio_Search.Importer.Canada.Data.Models
{
    public class DatabaseEntry
    {
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;


        [Timestamp]
        public byte[]? ConcurrencyStamp { get; set; }
    }
}
