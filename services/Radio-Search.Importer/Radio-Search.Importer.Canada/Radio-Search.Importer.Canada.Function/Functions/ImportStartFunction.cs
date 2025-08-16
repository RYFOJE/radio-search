using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Radio_Search.Importer.Canada.Services.Interfaces;

namespace Radio_Search.Importer.Canada.Function.Functions;

public class ImportStartFunction
{
    private readonly ILogger<ImportStartFunction> _logger;
    private readonly IImportManagerService _importManager;

    public ImportStartFunction(
        ILogger<ImportStartFunction> logger,
        IImportManagerService importManager)
    {
        _logger = logger;
        _importManager = importManager;
    }

    [Function(nameof(ImportStartFunction))]
    public async Task Run(
        [ServiceBusTrigger("canada", "ImportStart", Connection = "sb_importer", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        try
        {
            await _importManager.StartImportJob();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the message. Message will be abandoned.");

            // Abandon the message to allow retry
            await messageActions.AbandonMessageAsync(message);
            return;
        }

        await messageActions.CompleteMessageAsync(message);
    }
}