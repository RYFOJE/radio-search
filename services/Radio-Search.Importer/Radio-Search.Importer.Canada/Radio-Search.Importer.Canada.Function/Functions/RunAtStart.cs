using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Radio_Search.Importer.Canada.Services.Configuration;
using Radio_Search.Utils.MessageBroker.Implementations;
using Radio_Search.Utils.MessageBroker.Interfaces;

namespace Radio_Search.Importer.Canada.Services.Implementations
{
    public class RunAtStart : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RunAtStart> _logger;
        private readonly ServiceBusDefinitions _serviceBusDefinitions;

        public RunAtStart(
            IServiceProvider serviceProvider,
            ILogger<RunAtStart> logger,
            IOptions<ServiceBusDefinitions> serviceBusDefinitions)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _serviceBusDefinitions = serviceBusDefinitions.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var messageBrokerClient = scope.ServiceProvider.GetRequiredService<IMessageBrokerClient>();

                var filters = new[]
                {
                    (_serviceBusDefinitions.TopicName, new MessageFilter
                    {
                        FilterName = _serviceBusDefinitions.ImportStart_SubscriptionName,
                        FilterOnName = _serviceBusDefinitions.ImportStart_SubscriptionName
                    }),
                    (_serviceBusDefinitions.TopicName, new MessageFilter
                    {
                        FilterName = _serviceBusDefinitions.DownloadComplete_SubscriptionName,
                        FilterOnName = _serviceBusDefinitions.DownloadComplete_SubscriptionName
                    }),
                    (_serviceBusDefinitions.TopicName, new MessageFilter
                    {
                        FilterName = _serviceBusDefinitions.ChunkReady_SubscriptionName,
                        FilterOnName = _serviceBusDefinitions.ChunkReady_SubscriptionName
                    }),
                    (_serviceBusDefinitions.TopicName, new MessageFilter
                    {
                        FilterName = _serviceBusDefinitions.ChunkProcessingComplete_SubscriptionName,
                        FilterOnName = _serviceBusDefinitions.ChunkProcessingComplete_SubscriptionName
                    })
                };

                foreach (var (topicName, filter) in filters)
                {
                    _logger.LogInformation("Creating subscription {SubscriptionName} with filter {FilterOnName}",
                        filter.FilterName, filter.FilterOnName);

                    await messageBrokerClient.AddMessageFilter(topicName, filter);

                    _logger.LogInformation("Successfully created subscription {SubscriptionName}", filter.FilterName);
                }

                _logger.LogInformation("Service Bus initialization completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize Service Bus subscriptions");
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}