using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Radio_Search.Importer.Canada.Data_Contracts.V1;
using Radio_Search.Importer.Canada.Services.Interfaces;
using Radio_Search.Utils.MessageBroker.Formatters;
using System;
using System.Threading.Tasks;

namespace Radio_Search.Importer.Canada.Function.Functions;

public class ChunkProcessingCompleteFunction
{
    private readonly ILogger<ChunkProcessingCompleteFunction> _logger;
    private readonly IImportManagerService _importManager;

    public ChunkProcessingCompleteFunction(
        ILogger<ChunkProcessingCompleteFunction> logger,
        IImportManagerService importManager)
    {
        _logger = logger;
        _importManager = importManager;
    }

    [Function(nameof(ChunkProcessingCompleteFunction))]
    public async Task Run(
        [ServiceBusTrigger("canada", "ChunkProcessingComplete", Connection = "sb_importer", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        try
        {
            JSONFormatter formatter = new JSONFormatter();
            var deserializedMessage = formatter.Deserialize<ChunkProcessingComplete>(message.Body.ToArray());

            await _importManager.HandleChunkProcessingCompleteAsync(deserializedMessage.JobId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process Chunk Ready message. Attempt: {AttemptNumber}", message.DeliveryCount);
            await messageActions.AbandonMessageAsync(message);
        }

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}