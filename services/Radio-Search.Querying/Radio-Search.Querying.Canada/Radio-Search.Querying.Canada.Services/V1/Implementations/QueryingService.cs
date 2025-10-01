using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Radio_Search.Canada.Models.License;
using Radio_Search.Querying.Canada.Data.Repositories.Interfaces;
using Radio_Search.Querying.Canada.Data_Contracts.V1.Responses;
using Radio_Search.Querying.Canada.Services.V1.Interfaces;
using Radio_Search.Querying.Canada.Services.Validators;
using Radio_Search.Querying.Generic.Data_Contracts.V1;

namespace Radio_Search.Querying.Canada.Services.V1.Implementations
{
    public class QueryingService : IQueryingService
    {
        private ILicenseQueryingRepo _licenseQueryRepo;
        private ILogger _logger;
        private IValidator<QueryOptions> _queryValidator;
        private IMapper _mapper;

        public QueryingService(
            ILogger logger,
            ILicenseQueryingRepo licenseQueryRepo,
            IValidator<QueryOptions> queryValidator,
            IMapper mapper)
        {
            _licenseQueryRepo = licenseQueryRepo;
            _logger = logger;
            _queryValidator = queryValidator;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public Task<LicenseQueryResponse> QueryLicenses(LicenseQueryOptions fullQuery)
        {
            if(!_queryValidator.Validate(fullQuery).IsValid)
            {
                _logger.LogError("Invalid query options provided.");
                throw new ArgumentException("Invalid query options provided.", nameof(fullQuery));
            }

            var results = _licenseQueryRepo.QueryLicenses(fullQuery);

            var response = new LicenseQueryResponse
            {
                Licenses = _mapper.Map<List<LicenseDataContract>>(results.Result)
            };


            throw new NotImplementedException();
        }
    }
}
