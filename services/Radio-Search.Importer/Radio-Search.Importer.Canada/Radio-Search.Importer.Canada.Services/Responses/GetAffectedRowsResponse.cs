using Radio_Search.Importer.Canada.Data.Models.License;
using Radio_Search.Importer.Canada.Services.Data;

namespace Radio_Search.Importer.Canada.Services.Responses
{
    public class GetAffectedRowsResponse
    {

        /// <summary>
        /// List of TAFLEntryRows that differ from their corresponding DB Row
        /// </summary>
        public List<TAFLEntryRawRow> UpdatedRows { get; set; } = new();

        /// <summary>
        /// List of DBRows that are no longer in the TAFL File
        /// </summary>
        public List<LicenseRecord> DeletedRows { get; set; } = new();

        /// <summary>
        /// List of LicenseIDs that have not been modified
        /// </summary>
        public HashSet<string> UnaffectedRows { get; set; } = new();

    }
}
