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

        /// <summary>
        /// Handles the processing of a Service Bus message to initiate a table download operation.
        /// </summary>
        /// <remarks>This method logs the message details and attempts to download and save recent table
        /// data. If the download is successful, the message is marked as complete; otherwise, it is
        /// abandoned.</remarks>
        /// <param name="message">The received Service Bus message containing the details for the table download operation.</param>
        /// <param name="messageActions">Provides actions that can be performed on the message, such as completing or abandoning it.</param>
        /// <returns></returns>
        [Function("DownloadStartQueueHandler_TableDownload")]
        public async Task RunTableDownload(
            [ServiceBusTrigger("canada", Connection = "ServiceBus")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            var result = await _downloadFileService.DownloadAndSaveRecentTAFL();

            if (!result.Success)
            {
                await messageActions.AbandonMessageAsync(message);
                return;
            }

            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }

        /// <summary>
        /// Handles the processing of messages from the "canada-definition" Service Bus queue to download and save the
        /// latest TAFL definition.
        /// </summary>
        /// <remarks>This method logs the message details and attempts to download and save the latest
        /// TAFL definition using the <see cref="_downloadFileService"/>. If the download is unsuccessful, the message
        /// is abandoned; otherwise, the message is completed.</remarks>
        /// <param name="message">The received Service Bus message containing the details for the definition download.</param>
        /// <param name="messageActions">Provides actions that can be performed on the message, such as completing or abandoning it.</param>
        /// <returns></returns>
        [Function("DownloadStartQueueHandler_DefinitionDownload")]
        public async Task RunDefinitionDownload(
            [ServiceBusTrigger("canada-definition", Connection = "ServiceBus")] // TODO: Change this to something better
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
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
