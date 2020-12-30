namespace TooFast.Client
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;
    using MassTransit;


    class Program
    {
        static async Task Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
            });

            try
            {
                await bus.StartAsync(TimeSpan.FromSeconds(30));
                try
                {
                    //                    await LoadProcessOrderQueue(bus);

                    await BenchmarkRpc(bus);
                }
                finally
                {
                    await bus.StopAsync(TimeSpan.FromSeconds(30));
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        static async Task LoadProcessOrderQueue(IBusControl bus)
        {
            const int messageCount = 100;

            await Task.WhenAll(Enumerable.Range(0, messageCount).Select(x => bus.Publish(new ProcessOrder {OrderId = NewId.NextGuid()})));
        }

        static async Task BenchmarkRpc(IBusControl bus)
        {
            await using var clientFactory = await bus.CreateReplyToClientFactory();

            IRequestClient<SubmitOrder> client = clientFactory.CreateRequestClient<SubmitOrder>();

            var messageCount = 10000;
            var concurrentMessageCount = 40;
            var loopLimit = messageCount / concurrentMessageCount;

            // warmup

            await Task.WhenAll(Enumerable.Range(0, concurrentMessageCount).Select(x => ProcessOrder(client)));

            // do it

            var timer = Stopwatch.StartNew();

            for (var i = 0; i < loopLimit; i++)
                await Task.WhenAll(Enumerable.Range(0, concurrentMessageCount).Select(x => ProcessOrder(client)));

            timer.Stop();

            Console.WriteLine("Message Count: {0}", messageCount);

            Console.WriteLine("Total duration: {0:g}", timer.Elapsed);
            Console.WriteLine("Request rate: {0:F2} (req/s)", messageCount * 1000 / timer.ElapsedMilliseconds);
            Console.WriteLine("Message rate: {0:F2} (msg/s)", messageCount * 4 * 1000 / timer.ElapsedMilliseconds);
        }

        static async Task ProcessOrder(IRequestClient<SubmitOrder> client)
        {
            Response<OrderSubmitted, OrderRejected> response = await client.GetResponse<OrderSubmitted, OrderRejected>(new SubmitOrder
            {
                Order = new OrderModel
                {
                    OrderId = NewId.NextGuid(),
                    Total = 1234.56m
                }
            });
        }
    }
}