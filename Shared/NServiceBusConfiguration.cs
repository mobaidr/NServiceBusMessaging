using Microsoft.Extensions.Configuration;
using NServiceBus;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System;

namespace Shared.Core
{
    public class NServiceBusConfiguration
    {
        public static IEndpointInstance Setup()
        {
            var endpointConfiguration = new EndpointConfiguration("Samples.ASPNETCore.Endpoint");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            RabbitMQSetup(endpointConfiguration);

            var endpointInstance = Endpoint.Start(endpointConfiguration)
                                               .GetAwaiter()
                                               .GetResult();

            return endpointInstance;
        }

        private static void RabbitMQSetup(EndpointConfiguration endpointConfiguration)
        {
            Console.WriteLine("Creating ConfigurationBuilder");
            var builder = new ConfigurationBuilder().AddCloudFoundry();

            Console.WriteLine("Building the configuration");
            var config = builder.Build();

            string hostname = config["vcap:services:p-rabbitmq:0:credentials:hostname"];
            string vhost = config["vcap:services:p-rabbitmq:0:credentials:vhost"];
            string username = config["vcap:services:p-rabbitmq:0:credentials:username"];
            string password = config["vcap:services:p-rabbitmq:0:credentials:password"];

            string connectionStr = $"virtualhost={vhost};host={hostname};username={username};password={password}";

            Console.WriteLine("CONNECTIONSTRING");
            Console.WriteLine(connectionStr);

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.ConnectionString(connectionStr);
            transport.UseConventionalRoutingTopology();

            var routing = transport.Routing();

            routing.RouteToEndpoint(
                assembly: typeof(MyMessage).Assembly,
                destination: "Samples.ASPNETCore.Endpoint");
        }
    }
}
