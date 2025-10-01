using Microsoft.AspNetCore.Mvc;
using Radio_Search.Querying.Canada.Data_Contracts.V1.Responses;
using Radio_Search.Querying.Generic.Data_Contracts.V1;

namespace Radio_Search.Querying.Canada.Api.Controllers
{
    public class DetailedController : Controller
    {
        private readonly ILogger<DetailedController> _logger;

        public DetailedController(ILogger<DetailedController> logger)
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
