namespace TooFast.FrontEnd.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Components;
    using Contracts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;


    [ApiController]
    [Route("[controller]")]
    public class OrderController :
        ControllerBase
    {
        readonly ILogger<OrderController> _logger;
        readonly IValidateOrderService _validate;

        public OrderController(ILogger<OrderController> logger, IValidateOrderService validate)
        {
            _logger = logger;
            _validate = validate;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post(OrderModel order)
        {
            try
            {
                if (!ModelState.IsValid)
                    return ValidationProblem(ModelState);

                await _validate.ValidateOrder(order);

                return Ok();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unable to process order");

                return UnprocessableEntity(ModelState);
            }
        }
    }
}