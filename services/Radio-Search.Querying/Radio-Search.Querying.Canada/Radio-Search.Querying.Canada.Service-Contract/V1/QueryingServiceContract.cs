using Radio_Search.Querying.Canada.Data_Contracts.V1.Responses;
using Radio_Search.Querying.Canada.Service_Contract.V1.Interfaces;
using Radio_Search.Querying.Generic.Data_Contracts.V1;

namespace Radio_Search.Querying.Canada.Service_Contract.V1
{
    public class QueryingServiceContract : IQueryingServiceContract
    {
        private string _serviceName;
        private IHttpClientFactory _clientFactory;

        public QueryingServiceContract(
            IHttpClientFactory clientFactory,
            string serviceName = Constants.HTTP_FACTORY_NAME)
        {
            _clientFactory = clientFactory;
            _serviceName = serviceName;
        }

        ///<inheritdoc/>
        public Task<LicenseQueryResponse> FetchLicensesForQuery(LicenseQueryOptions query)
        {
            var client = _clientFactory.CreateClient(_serviceName);
        }

    }
}
