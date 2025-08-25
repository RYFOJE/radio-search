using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Radio_Search.Utils.MessageBroker.Configurations;
using Radio_Search.Utils.MessageBroker.Exceptions;
using Radio_Search.Utils.MessageBroker.Interfaces;

namespace Radio_Search.Utils.MessageBroker.Implementations.Azure
{
    public class AzureMessageBrokerClient : IMessageBrokerClient
    {
        private readonly AzureServiceBusClientConfig _config;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusAdministrationClient _adminClient;

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

                _adminClient = new ServiceBusAdministrationClient(_config.ServiceBusUrl, new DefaultAzureCredential());
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

        public async Task<IMessageBrokerClient> AddMessageFilter(string destinationName, MessageFilter filter)
        {
            if (!await _adminClient.SubscriptionExistsAsync(destinationName, filter.FilterName))
            {
                var options = new CreateSubscriptionOptions(destinationName, filter.FilterName);

                options.LockDuration = TimeSpan.FromMinutes(5);
                options.MaxDeliveryCount = 5;

                // Add a rule that filters for messages with the subscription name
                var ruleOptions = new CreateRuleOptions(
                    filter.FilterName,
                    new SqlRuleFilter($"Target = '{filter.FilterOnName}'")
                );

                await _adminClient.CreateSubscriptionAsync(options, ruleOptions);

                if(await _adminClient.RuleExistsAsync(destinationName, filter.FilterName, "$Default"))
                    await _adminClient.DeleteRuleAsync(destinationName, filter.FilterName, "$Default");
            }

            return this;
        }
    }
}
