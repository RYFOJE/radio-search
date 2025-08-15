using Radio_Search.Importer.Canada.Services.Data;

namespace Radio_Search.Importer.Canada.Services.Responses
{
    public class GetValidRawRowsResponse
    {
        public List<TAFLEntryRawRow> ValidRows { get; set; } = [];
        public int InvalidRowCount { get; set; } = 0;
    }
}
