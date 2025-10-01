using Radio_Search.Querying.Canada.Data_Contracts.V1.Responses;
using Radio_Search.Querying.Generic.Data_Contracts.V1;

namespace Radio_Search.Querying.Canada.Services.V1.Interfaces
{
    public interface IQueryingService
    {
        /// <summary>
        /// Queries licenses based on the specified options and returns the results.
        /// </summary>
        /// <param name="fullQuery">The options that define the criteria for the license query.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see
        /// cref="LicenseQueryResponse"/> object with the query results.</returns>
        Task<LicenseQueryResponse> QueryLicenses(LicenseQueryOptions fullQuery);
    }
}
