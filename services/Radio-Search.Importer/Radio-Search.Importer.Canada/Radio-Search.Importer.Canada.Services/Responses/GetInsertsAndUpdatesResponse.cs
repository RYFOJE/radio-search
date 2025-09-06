using Radio_Search.Importer.Canada.Services.Data;

namespace Radio_Search.Importer.Canada.Services.Responses
{
    public class GetInsertsAndUpdatesResponse
    {

        public List<TaflEntryRawRow> InsertRows { get; set; } = [];
        public List<(int NewVersionNumber, TaflEntryRawRow Row)> UpdateRows { get; set; } = [];

    }
}
