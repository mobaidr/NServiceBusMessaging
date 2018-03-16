using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Shared.Core;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System;

public class Startup
{
    public Startup(IHostingEnvironment env)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddEnvironmentVariables();
        Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }


    public void ConfigureServices(IServiceCollection services)
    {
        //#region EndpointConfiguration

        //var builder = new ConfigurationBuilder().AddCloudFoundry();

        //Console.WriteLine("Building the configuration");
        //var config = builder.Build();

        //Console.WriteLine("Looking for the RabbitMQ URI");

        //string uri = config["vcap:services:p-rabbitmq:0:credentials:dashboard_url"];

        //Console.WriteLine(uri);
        //var endpointConfiguration = new EndpointConfiguration("Samples.ASPNETCore.Sender");
        //var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        //transport.UseConventionalRoutingTopology();
        //transport.ConnectionString(uri);


        ////endpointConfiguration.EnableInstallers();
        //endpointConfiguration.UsePersistence<InMemoryPersistence>();
        //endpointConfiguration.SendOnly();

        //#endregion

        //#region Routing

        //var routing = transport.Routing();
        //routing.RouteToEndpoint(
        //    assembly: typeof(MyMessage).Assembly,
        //    destination: "Samples.ASPNETCore.Endpoint");

        //#endregion

        //#region EndpointStart
        //Console.WriteLine("Starting endpoint");
        //var endpointInstance = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

        //Console.WriteLine("Started endpoint");

        //#endregion

        var endpointInstance = NServiceBusConfiguration.Setup();

        #region ServiceRegistration

        Console.WriteLine("Setting IMessageSession");
        services.AddSingleton<IMessageSession>(endpointInstance);

        #endregion

        services.AddMvc();
    }


    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        loggerFactory.AddDebug();
        app.UseMvc();
    }
}