namespace TooFast.FrontEnd
{
    using System;
    using Components;
    using Contracts;
    using MassTransit;
    using MassTransit.Clients;
    using MassTransit.ExtensionsDependencyInjectionIntegration;
    using MassTransit.RabbitMqTransport.Transport;
    using MassTransit.Scoping;
    using MassTransit.Util;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;


    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<IValidateOrderService, ValidateOrderService>(client =>
            {
                client.BaseAddress = new Uri("http://backend:80/");
            });

            services.AddMassTransit(x =>
            {
                x.AddConsumersFromNamespaceContaining<SubmitOrderConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.PrefetchCount = 256;

                    cfg.Host("rabbitmq");

                    cfg.ConfigureEndpoints(context);
                });
            });
            services.AddMassTransitHostedService();

            services.AddReplyToClientFactory();
            services.AddReplyToRequestClient<ValidateOrder>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TooFast.FrontEnd",
                    Version = "v1"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TooFast.FrontEnd v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }


    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddReplyToClientFactory(this IServiceCollection services, RequestTimeout timeout = default)
        {
            return services.AddSingleton(x =>
                Bind<RabbitMqHost>.Create(TaskUtil.Await(() => x.GetRequiredService<IBus>().CreateReplyToClientFactory(timeout))));
        }

        public static IServiceCollection AddReplyToRequestClient<T>(this IServiceCollection services, RequestTimeout timeout = default)
            where T : class
        {
            return services.AddScoped(provider =>
            {
                var clientFactory = provider.GetRequiredService<Bind<RabbitMqHost, IClientFactory>>().Value;
                var consumeContext = provider.GetRequiredService<ScopedConsumeContextProvider>().GetContext();

                if (consumeContext != null)
                    return clientFactory.CreateRequestClient<T>(consumeContext, timeout);

                return new ClientFactory(new ScopedClientFactoryContext<IServiceProvider>(clientFactory, provider))
                    .CreateRequestClient<T>(timeout);
            });
        }
    }
}