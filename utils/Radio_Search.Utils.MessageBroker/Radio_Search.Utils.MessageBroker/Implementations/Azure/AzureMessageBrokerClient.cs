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
        private readonly bool _isEmulator;

        public AzureMessageBrokerClient(AzureServiceBusClientConfig config)
        {
            _config = config;

            if (config == null)
                throw new InvalidMessageBrokerClientException("Azure Service Bus Client Config cannot be null when create Broker Client.");

            else if (string.IsNullOrWhiteSpace(config.ServiceBusUrl))
                throw new InvalidMessageBrokerClientException("Azure Service Bus Client Config Url cannot be null or whitespace.");

            _isEmulator = IsConnectionString(config.ServiceBusUrl);

            try
            {
                if (_isEmulator)
                {
                    // Local/emulator use: SAS connection string, no Entra ID involved.
                    // The Service Bus emulator only supports the AMQP TCP transport, not AMQP Web Sockets,
                    // so the transport is forced regardless of the caller-supplied ClientOptions.
                    var emulatorOptions = _config.ClientOptions ?? new ServiceBusClientOptions();
                    emulatorOptions.TransportType = ServiceBusTransportType.AmqpTcp;

                    _client = new ServiceBusClient(_config.ServiceBusUrl, emulatorOptions);
                    _adminClient = new ServiceBusAdministrationClient(_config.ServiceBusUrl);
                }
                else
                {
                    // Real Azure use: fully qualified namespace + managed identity / developer credential.
                    _client = new ServiceBusClient(
                        _config.ServiceBusUrl,
                        new DefaultAzureCredential(),
                        _config.ClientOptions);

                    _adminClient = new ServiceBusAdministrationClient(_config.ServiceBusUrl, new DefaultAzureCredential());
                }
            }
            catch (Exception ex)
            {
                throw new InvalidMessageBrokerClientException("Exception thrown while creating Service Bus Client.", ex);
            }
        }

        /// <summary>
        /// The Service Bus emulator is only reachable via a SAS connection string
        /// (e.g. "Endpoint=sb://localhost;...;UseDevelopmentEmulator=true;"), since it doesn't
        /// support Microsoft Entra ID. A real namespace value is a bare URI/hostname with no SAS key.
        /// </summary>
        private static bool IsConnectionString(string serviceBusUrl) =>
            serviceBusUrl.Contains("Endpoint=sb://", StringComparison.OrdinalIgnoreCase) &&
            serviceBusUrl.Contains("SharedAccessKey", StringComparison.OrdinalIgnoreCase);

        /// <inheritdoc/>
        public IMessageBrokerWriter GetMessageWriter(string destinationName)
        {
            return new AzureServiceBusWriter(destinationName, _client);
        }

        public async Task<IMessageBrokerClient> AddMessageFilter(string destinationName, MessageFilter filter)
        {
            // The emulator's admin/management API listens on a separate port (5300) from the
            // AMQP data connection (5672), which isn't part of the connection string Aspire injects,
            // so ServiceBusAdministrationClient can't reach it here. Not needed locally anyway:
            // the Aspire AppHost already declares topics/subscriptions into the emulator's config.json
            // at container start (see AddServiceBusTopic/AddServiceBusSubscription in AppHost.cs).
            if (_isEmulator)
                return this;

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
