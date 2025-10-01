using Radio_Search.Canada.Models.License;
using Radio_Search.Querying.Generic.Data_Contracts.V1;

namespace Radio_Search.Querying.Canada.Data.Repositories.Interfaces
{
    public interface ILicenseQueryingRepo
    {

        /// <summary>
        /// Queries the available licenses based on the specified options.
        /// </summary>
        /// <param name="query">The options used to filter and configure the license query. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The result is a tuple containing: <list type="bullet">
        /// <item> <description>A list of <see cref="LicenseRecord"/> objects that match the query
        /// criteria.</description> </item> <item> <description>A boolean value indicating whether there are more
        /// results available beyond the current query.</description> </item> </list></returns>
        public Task<(List<LicenseRecord>, bool)> QueryLicenses(LicenseQueryOptions query);
    }
}
