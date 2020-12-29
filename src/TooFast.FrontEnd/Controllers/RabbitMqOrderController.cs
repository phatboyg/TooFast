namespace TooFast.FrontEnd.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Contracts;
    using MassTransit;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;


    [ApiController]
    [Route("[controller]")]
    public class RabbitMqOrderController :
        ControllerBase
    {
        readonly ILogger<OrderController> _logger;
        readonly IRequestClient<ValidateOrder> _requestClient;

        public RabbitMqOrderController(ILogger<OrderController> logger, IRequestClient<ValidateOrder> requestClient)
        {
            _logger = logger;
            _requestClient = requestClient;
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

                Response response = await _requestClient.GetResponse<OrderValidated, OrderInvalid>(new ValidateOrder {Order = order});

                return response switch
                {
                    (_, Response<OrderValidated>) => Ok(),
                    (_, Response<OrderInvalid>) => UnprocessableEntity(ModelState),
                    _ => Problem()
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unable to process order");

                return UnprocessableEntity(ModelState);
            }
        }
    }
}