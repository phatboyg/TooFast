namespace TooFast.BackEnd.Components
{
    using MassTransit;
    using MassTransit.ConsumeConfigurators;
    using MassTransit.Definition;


    public class ProcessOrderConsumerDefinition :
        ConsumerDefinition<ProcessOrderConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<ProcessOrderConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseKillSwitch(options =>
            {
                options.ActivationThreshold = 10;
                options.SetRestartTimeout(s: 10);
                options.SetTripThreshold(0.15);
            });
        }
    }
}