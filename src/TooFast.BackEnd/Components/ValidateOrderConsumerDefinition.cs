namespace TooFast.BackEnd.Components
{
    using MassTransit;
    using MassTransit.ConsumeConfigurators;
    using MassTransit.Definition;
    using MassTransit.RabbitMqTransport;


    public class ValidateOrderConsumerDefinition :
        ConsumerDefinition<ValidateOrderConsumer>
    {
        public ValidateOrderConsumerDefinition()
        {
            Endpoint(x => x.PrefetchCount = 100);
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<ValidateOrderConsumer> consumerConfigurator)
        {
            if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rabbit)
                rabbit.Durable = false;

            base.ConfigureConsumer(endpointConfigurator, consumerConfigurator);
        }
    }
}