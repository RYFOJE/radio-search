using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Radio_Search.Utils.MessageBroker.Configurations;
using Radio_Search.Utils.MessageBroker.Exceptions;
using Radio_Search.Utils.MessageBroker.Interfaces;

namespace Radio_Search.Utils.MessageBroker.Implementations.Azure
{
    public class AzureMessageBrokerClient : IMessageBrokerClient
    {
        private readonly AzureServiceBusClientConfig _config;
        private readonly ServiceBusClient _client;

        public AzureMessageBrokerClient(AzureServiceBusClientConfig config) 
        {
            _config = config;

            if (config == null)
                throw new InvalidMessageBrokerClientException("Azure Service Bus Client Config cannot be null when create Broker Client.");

            else if (string.IsNullOrWhiteSpace(config.ServiceBusUrl))
                throw new InvalidMessageBrokerClientException("Azure Service Bus Client Config Url cannot be null or whitespace.");

            try
            {
                _client = new ServiceBusClient(
                    _config.ServiceBusUrl,
                    new DefaultAzureCredential(),
                    _config.ClientOptions);
            }
            catch (Exception ex)
            {
                throw new InvalidMessageBrokerClientException("Exception thrown while creating Service Bus Client.", ex);
            }
        }

        /// <inheritdoc/>
        public IMessageBrokerWriter GetMessageWriter(string destinationName)
        {
            return new AzureServiceBusWriter(destinationName, _client);
        }
    }
}
