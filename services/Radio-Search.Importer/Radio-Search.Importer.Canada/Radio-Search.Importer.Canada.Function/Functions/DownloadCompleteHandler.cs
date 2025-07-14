using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Radio_Search.Importer.Canada.Function.Functions;

public class DownloadCompleteHandler
{
    private readonly ILogger<DownloadCompleteHandler> _logger;

    public DownloadCompleteHandler(ILogger<DownloadCompleteHandler> logger)
    {
        _logger = logger;
    }

    [Function(nameof(DownloadCompleteHandler))]
    public async Task Run([BlobTrigger("canada/unprocessed/{name}", Connection = "canada-blob")] Stream blobData, string name)
    {   
        _logger.LogInformation("C# Blob trigger function Processed blob\n Name: {name}", name);
    }
}