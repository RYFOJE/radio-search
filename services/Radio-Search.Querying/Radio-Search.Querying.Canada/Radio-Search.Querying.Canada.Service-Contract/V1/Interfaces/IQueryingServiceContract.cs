using Radio_Search.Querying.Canada.Data_Contracts.V1.Responses;
using Radio_Search.Querying.Generic.Data_Contracts.V1;

namespace Radio_Search.Querying.Canada.Service_Contract.V1.Interfaces
{
    public interface IQueryingServiceContract
    {
        /// <summary>
        /// Fetches license information based on the specified query.
        /// </summary>
        /// <param name="query">The query containing the criteria for retrieving license information. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="LicenseQueryResponse"/>
        /// object with the license information matching the query criteria.</returns>
        Task<LicenseQueryResponse> FetchLicensesForQuery(LicenseQueryOptions query);
    }
}
