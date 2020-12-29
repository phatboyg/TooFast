namespace TooFast.FrontEnd.Components
{
    using System.Threading.Tasks;
    using Contracts;
    using MassTransit;


    public class SubmitOrderConsumer :
        IConsumer<SubmitOrder>
    {
        readonly IRequestClient<ValidateOrder> _requestClient;

        public SubmitOrderConsumer(IRequestClient<ValidateOrder> requestClient)
        {
            _requestClient = requestClient;
        }

        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {
            Response<OrderValidated, OrderInvalid> response =
                await _requestClient.GetResponse<OrderValidated, OrderInvalid>(new ValidateOrder {Order = context.Message.Order});

            if (response.Is(out Response<OrderValidated> validated))
                await context.RespondAsync(new OrderSubmitted {OrderId = context.Message.Order.OrderId});

            if (response.Is(out Response<OrderInvalid> invalid))
                await context.RespondAsync(new OrderRejected {OrderId = context.Message.Order.OrderId});
        }
    }
}