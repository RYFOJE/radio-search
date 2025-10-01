namespace Radio_Search.Querying.Canada.Data_Contracts.V1.Responses
{
    public class LicenseQueryResponse
    {
        /// <summary>
        /// Total result count for the query, regardless of paging.
        /// </summary>
        public int TotalResults { get; set; } = 0;

        /// <summary>
        /// Cursor value for the next page, will be null if no more results.
        /// </summary>
        public int? NextCursor { get; set; }

        /// <summary>
        /// Query Result, containing License DTOs
        /// </summary>
        public List<LicenseDataContract> Licenses { get; set; } = new List<LicenseDataContract>();
    }
}
