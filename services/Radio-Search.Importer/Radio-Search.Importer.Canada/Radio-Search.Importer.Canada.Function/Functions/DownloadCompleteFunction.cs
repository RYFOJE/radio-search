using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Radio_Search.Importer.Canada.Data_Contracts.V1;
using Radio_Search.Importer.Canada.Services.Interfaces;
using Radio_Search.Utils.MessageBroker.Formatters;

namespace Radio_Search.Importer.Canada.Function.Functions;

public class DownloadCompleteFunction
{
    private readonly ILogger<DownloadCompleteFunction> _logger;
    private readonly IImportManagerService _importManager;

    public DownloadCompleteFunction(
        ILogger<DownloadCompleteFunction> logger,
        IImportManagerService importManager)
    {
        _logger = logger;
        _importManager = importManager;
    }

    [Function(nameof(DownloadCompleteFunction))]
    public async Task Run(
        [ServiceBusTrigger("canada", "DownloadComplete", Connection = "sb_importer", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        try
        {
            JSONFormatter formatter = new JSONFormatter();
            var deserializedMessage = formatter.Deserialize<DownloadCompleteMessage>(message.Body.ToArray());

            await _importManager.HandleDownloadComplete(deserializedMessage.ImportJobID);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process Download complete message. Attempt: {attemptNumber}", message.DeliveryCount);
            await messageActions.AbandonMessageAsync(message);
        }
            // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}