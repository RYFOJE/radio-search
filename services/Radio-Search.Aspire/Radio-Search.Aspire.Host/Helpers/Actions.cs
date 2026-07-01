using Aspire.Hosting.Azure;
using Azure.Messaging.ServiceBus;

namespace Radio_Search.Aspire.Host.Helpers;

public static class Actions
{
    // Subscriptions are matched on the "Target" application property (see ServiceBusManagement.SetupFilters).
    public static IResourceBuilder<AzureServiceBusResource> WithServiceBusDashboardCommands(
        this IResourceBuilder<AzureServiceBusResource> serviceBus,
        string topicName,
        params string[] subscriptionNames)
    {
        serviceBus.WithCommand(
            name: "publish-import-start",
            displayName: "Publish ImportStart",
            executeCommand: async context =>
            {
                var connectionString = await serviceBus.Resource.ConnectionStringExpression.GetValueAsync(context.CancellationToken);

                await using var client = new ServiceBusClient(connectionString);
                await using var sender = client.CreateSender(topicName);

                var message = new ServiceBusMessage
                {
                    ApplicationProperties = { ["Target"] = "ImportStart" }
                };

                await sender.SendMessageAsync(message, context.CancellationToken);

                return new ExecuteCommandResult { Success = true, Message = "ImportStart message published." };
            },
            commandOptions: new CommandOptions
            {
                IconName = "Send",
                IsHighlighted = true,
                Description = "Publish a message that starts the Canada import job.",
                ConfirmationMessage = $"Publish an ImportStart message to the {topicName} topic?"
            });

        serviceBus.WithCommand(
            name: "peek-queue-depths",
            displayName: "Peek Queue Depths",
            executeCommand: async context =>
            {
                var connectionString = await serviceBus.Resource.ConnectionStringExpression.GetValueAsync(context.CancellationToken);

                await using var client = new ServiceBusClient(connectionString);

                var summaries = new List<string>();

                foreach (var subscriptionName in subscriptionNames)
                {
                    await using var activeReceiver = client.CreateReceiver(topicName, subscriptionName);
                    await using var deadLetterReceiver = client.CreateReceiver(topicName, subscriptionName,
                        new ServiceBusReceiverOptions { SubQueue = SubQueue.DeadLetter });

                    var active = await activeReceiver.PeekMessagesAsync(maxMessages: 1000, cancellationToken: context.CancellationToken);
                    var deadLettered = await deadLetterReceiver.PeekMessagesAsync(maxMessages: 1000, cancellationToken: context.CancellationToken);

                    summaries.Add($"{subscriptionName}: active={FormatCount(active.Count)}, dead-letter={FormatCount(deadLettered.Count)}");
                }

                return new ExecuteCommandResult { Success = true, Message = string.Join(" | ", summaries) };

                static string FormatCount(int count) => count >= 1000 ? "1000+" : count.ToString();
            },
            commandOptions: new CommandOptions
            {
                IconName = "Eye",
                Description = "Peek approximate active and dead-letter message counts for each subscription."
            });

        return serviceBus;
    }
}
