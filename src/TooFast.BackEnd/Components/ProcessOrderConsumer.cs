namespace TooFast.BackEnd.Components
{
    using System;
    using System.Threading.Tasks;
    using Contracts;
    using MassTransit;


    public class ProcessOrderConsumer :
        IConsumer<ProcessOrder>
    {
        public Task Consume(ConsumeContext<ProcessOrder> context)
        {
            throw new InvalidOperationException("We are going to fail, because the road is closed.");
        }
    }
}