using Radio_Search.Importer.Canada.Services.Data;

namespace Radio_Search.Importer.Canada.Services.Responses
{
    public class GetValidRawRowsResponse
    {
        public List<TaflEntryRawRow> ValidRows { get; set; } = [];
        public int InvalidRowCount { get; set; } = 0;
    }
}
