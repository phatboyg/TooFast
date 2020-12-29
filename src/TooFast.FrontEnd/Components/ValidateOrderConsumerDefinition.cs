namespace TooFast.FrontEnd.Components
{
    using MassTransit.Definition;


    public class SubmitOrderConsumerDefinition :
        ConsumerDefinition<SubmitOrderConsumer>
    {
        public SubmitOrderConsumerDefinition()
        {
            Endpoint(x => x.PrefetchCount = 100);
        }
    }
}