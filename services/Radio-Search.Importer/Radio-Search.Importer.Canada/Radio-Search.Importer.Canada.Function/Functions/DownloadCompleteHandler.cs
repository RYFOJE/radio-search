using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Radio_Search.Importer.Canada.Services.Interfaces;

namespace Radio_Search.Importer.Canada.Function.Functions;

public class DownloadCompleteHandler
{
    private readonly ILogger<DownloadCompleteHandler> _logger;
    private readonly IImportService _importService;

    public DownloadCompleteHandler(ILogger<DownloadCompleteHandler> logger, IImportService importService)
    {
        _logger = logger;
        _importService = importService;
    }

    [Function(nameof(DownloadCompleteHandler))]
    public async Task RunDefinitionImport([BlobTrigger("canada/pdf/unprocessed/{name}", Connection = "canada-blob")] Stream blobData, string name)
    {
        _importService.ProcessTAFLDefinition(blobData);
    }
}