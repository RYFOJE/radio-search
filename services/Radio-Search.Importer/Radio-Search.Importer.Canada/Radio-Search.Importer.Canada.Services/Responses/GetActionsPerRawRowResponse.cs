using Radio_Search.Importer.Canada.Data.Models.License;
using Radio_Search.Importer.Canada.Services.Data;

namespace Radio_Search.Importer.Canada.Services.Responses
{
    public class GetActionsPerRawRowResponse
    {
        public List<LicenseRecord> DeletedDBLicense { get; set; } = [];
        public List<TAFLEntryRawRow> UpdatedRawRows { get; set; } = [];
        public List<TAFLEntryRawRow> CreatedRawRows { get; set; } = [];
    }
}
