using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Radio_Search.Importer.Canada.Services.Configuration;
using Radio_Search.Importer.Canada.Services.Interfaces.EnvironmentManagement;
using Radio_Search.Utils.MessageBroker.Implementations;
using Radio_Search.Utils.MessageBroker.Interfaces;

namespace Radio_Search.Importer.Canada.Services.Implementations.EnvironmentManagement
{
    public class ServiceBusManagement(
        IMessageBrokerClient messageBrokerClient,
        ILogger<ServiceBusManagement> logger,
        IOptions<ServiceBusDefinitions> serviceBusDefinitions) : IServiceBusManagement
    {
        public async Task SetupFilters()
        {
            try
            {
                var filters = new[]
                {
                    (serviceBusDefinitions.Value.TopicName, new MessageFilter
                    {
                        FilterName = serviceBusDefinitions.Value.ImportStart_SubscriptionName,
                        FilterOnName = serviceBusDefinitions.Value.ImportStart_SubscriptionName
                    }),
                    (serviceBusDefinitions.Value.TopicName, new MessageFilter
                    {
                        FilterName = serviceBusDefinitions.Value.DownloadComplete_SubscriptionName,
                        FilterOnName = serviceBusDefinitions.Value.DownloadComplete_SubscriptionName
                    }),
                    (serviceBusDefinitions.Value.TopicName, new MessageFilter
                    {
                        FilterName = serviceBusDefinitions.Value.ChunkReady_SubscriptionName,
                        FilterOnName = serviceBusDefinitions.Value.ChunkReady_SubscriptionName
                    }),
                    (serviceBusDefinitions.Value.TopicName, new MessageFilter
                    {
                        FilterName = serviceBusDefinitions.Value.ChunkProcessingComplete_SubscriptionName,
                        FilterOnName = serviceBusDefinitions.Value.ChunkProcessingComplete_SubscriptionName
                    })
                };

                foreach (var (topicName, filter) in filters)
                {
                    logger.LogInformation("Creating subscription {SubscriptionName} with filter {FilterOnName}",
                        filter.FilterName, filter.FilterOnName);

                    await messageBrokerClient.AddMessageFilter(topicName, filter);

                    logger.LogInformation("Successfully created subscription {SubscriptionName}", filter.FilterName);
                }

                logger.LogInformation("Service Bus initialization completed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to initialize Service Bus subscriptions");
                throw;
            }
        }
    }
}
