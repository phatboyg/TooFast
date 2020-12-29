namespace TooFast.BackEnd.Controllers
{
    using System.Threading.Tasks;
    using Contracts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;


    [ApiController]
    [Route("[controller]")]
    public class ValidateOrderController :
        ControllerBase
    {
        readonly ILogger<ValidateOrderController> _logger;

        public ValidateOrderController(ILogger<ValidateOrderController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post(OrderModel order)
        {
            return Ok();
        }
    }
}