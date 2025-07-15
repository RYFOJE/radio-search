using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Radio_Search.Importer.Canada.Services.Interfaces;

namespace Radio_Search.Importer.Canada.Function.Functions
{
    public class DownloadStartQueueHandler
    {
        private readonly ILogger<DownloadStartQueueHandler> _logger;
        private readonly IDownloadFileService _downloadFileService;

        public DownloadStartQueueHandler(
            ILogger<DownloadStartQueueHandler> logger,
            IDownloadFileService downloadFileService)
        {
            _logger = logger;
            _downloadFileService = downloadFileService;
        }

        [Function("DownloadStartQueueHandler_TableDownload")]
        public async Task RunTableDownload(
            [ServiceBusTrigger("canada", Connection = "ServiceBus")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            var result = await _downloadFileService.DownloadAndSaveRecentTAFL();

            if (!result.Success)
            {
                await messageActions.AbandonMessageAsync(message);
                return;
            }

            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }

        [Function("DownloadStartQueueHandler_DefinitionDownload")]
        public async Task RunDefinitionDownload(
            [ServiceBusTrigger("canada-definition", Connection = "ServiceBus")] // TODO: Change this to something better
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            var result = await _downloadFileService.DownloadAndSaveRecentTAFLDefinition();

            if (!result.Success)
            {
                await messageActions.AbandonMessageAsync(message);
                return;
            }

            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
    }
}
