using Radio_Search.Importer.Canada.Services.Data;

namespace Radio_Search.Importer.Canada.Services.Responses
{
    public class ProcessTAFLCsvResponse : ResponseBase
    {
        public List<BroadcastAuthorizationRecord> Data { get; set; } = new();
        public int ColumnMismatchCount { get; set; }
        public int BadDataCount { get; set; }
    }
}
