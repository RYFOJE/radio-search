using Radio_Search.Importer.Canada.Services.Data;

namespace Radio_Search.Importer.Canada.Services.Responses
{
    public class GetInsertsAndUpdatesResponse
    {

        public List<TAFLEntryRawRow> InsertRows { get; set; } = [];
        public List<TAFLEntryRawRow> UpdateRows { get; set; } = [];

    }
}
