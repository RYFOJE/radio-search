using Microsoft.AspNetCore.Mvc;
using Radio_Search.Querying.Canada.Data_Contracts.V1.Responses;
using Radio_Search.Querying.Generic.Data_Contracts.V1;

namespace Radio_Search.Querying.Canada.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueryingController : ControllerBase
    {
        private readonly ILogger<QueryingController> _logger;

        public QueryingController(ILogger<QueryingController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(LicenseQueryResponse), StatusCodes.Status200OK)]
        public Task<IActionResult> Get(LicenseQueryOptions query)
        {
            throw new NotImplementedException();
        }
    }
}
