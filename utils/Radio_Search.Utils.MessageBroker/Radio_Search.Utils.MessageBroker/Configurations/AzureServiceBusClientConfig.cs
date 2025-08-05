using Azure.Messaging.ServiceBus;

namespace Radio_Search.Utils.MessageBroker.Configurations
{
    public class AzureServiceBusClientConfig
    {
        public required string ServiceBusUrl { get; set; }

        public ServiceBusClientOptions? ClientOptions { get; set; } = new ServiceBusClientOptions()
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        };
    }
}
