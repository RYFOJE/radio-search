using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Radio_Search.Importer.Canada.Data_Contracts.V1;
using Radio_Search.Importer.Canada.Services.Interfaces;
using Radio_Search.Utils.MessageBroker.Formatters;
using System.Diagnostics;

namespace Radio_Search.Importer.Canada.Function.Functions;

public class ChunkProcessingFunction
{
    private readonly ILogger<ChunkProcessingFunction> _logger;
    private readonly IImportManagerService _importManager;

    public ChunkProcessingFunction(
        ILogger<ChunkProcessingFunction> logger,
        IImportManagerService importManager)
    {
        _logger = logger;
        _importManager = importManager;
    }

    [Function(nameof(ChunkProcessingFunction))]
    public async Task Run(
        [ServiceBusTrigger("canada", "ChunkReady", Connection = "sb_importer", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        var timer = Stopwatch.StartNew();
        var messageHandled = false;

        try
        {
            JSONFormatter formatter = new JSONFormatter();
            var deserializedMessage = formatter.Deserialize<ProcessChunkMessage>(message.Body.ToArray());

            _logger.LogInformation("Starting to process ChunkID: {ChunkID} for ImportJobID: {ImportID}. Lock expires: {LockExpiry}, Delivery count: {DeliveryCount}",
                deserializedMessage.FileID, deserializedMessage.ImportJobID, message.LockedUntil, message.DeliveryCount);

            await _importManager.ProcessChunkAsync(deserializedMessage.ImportJobID, deserializedMessage.FileID, 
                async () => await messageActions.RenewMessageLockAsync(message)
            );

            _logger.LogInformation("Finished processing the ChunkID: {ChunkID} for ImportJobID: {ImportID} in {ElapsedMs} ms.",
                deserializedMessage.FileID, deserializedMessage.ImportJobID, timer.ElapsedMilliseconds);

            // Complete the message only on successful processing
            await messageActions.CompleteMessageAsync(message);
            messageHandled = true;

            _logger.LogInformation("Successfully completed message for ChunkID: {ChunkID}", deserializedMessage.FileID);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process Chunk Ready message. ChunkID: Unknown, Attempt: {AttemptNumber}, Elapsed: {ElapsedMs} ms",
                message.DeliveryCount, timer.ElapsedMilliseconds);

            try
            {
                await messageActions.AbandonMessageAsync(message);
                messageHandled = true;
                _logger.LogInformation("Successfully abandoned message after error. Delivery count: {DeliveryCount}", message.DeliveryCount);
            }
            catch (Exception abandonEx)
            {
                _logger.LogError(abandonEx, "Failed to abandon message after processing error. This may indicate a lock timeout.");
            }
        }

        if (!messageHandled)
        {
            _logger.LogWarning("Message was not handled (neither completed nor abandoned). This indicates a code logic error.");
        }
    }
}