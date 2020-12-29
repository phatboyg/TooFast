namespace TooFast.BackEnd.Components
{
    using System.Threading.Tasks;
    using Contracts;
    using MassTransit;


    public class ValidateOrderConsumer :
        IConsumer<ValidateOrder>
    {
        public async Task Consume(ConsumeContext<ValidateOrder> context)
        {
            if (context.Message.Order.Total < 1000000.00m)
                await context.RespondAsync(new OrderValidated {OrderId = context.Message.Order.OrderId});
            else
                await context.RespondAsync(new OrderInvalid {OrderId = context.Message.Order.OrderId});
        }
    }
}