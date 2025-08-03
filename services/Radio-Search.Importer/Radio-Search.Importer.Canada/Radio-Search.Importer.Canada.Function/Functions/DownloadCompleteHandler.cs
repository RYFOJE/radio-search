using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Radio_Search.Importer.Canada.Services.Interfaces;
using System.Net;
using System.Runtime.CompilerServices;

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

    /// <summary>
    /// Processes and imports a TAFL definition from a blob storage trigger.
    /// </summary>
    /// <remarks>This method is triggered by a blob upload event in the specified storage container. It
    /// processes the TAFL definition contained in the blob and saves the resulting data to the database.</remarks>
    /// <param name="blobData">The stream containing the blob data to be processed.</param>
    /// <param name="name">The name of the blob being processed.</param>
    /// <returns></returns>
    [Function(nameof(DownloadCompleteHandler))]
    public async Task RunDefinitionImport([BlobTrigger("canada/pdf/unprocessed/{name}", Connection = "canada-blob")] Stream blobData, string name)
    {
        // TODO: Add Checking
        var data = _importService.ProcessTAFLDefinition(blobData);

        await _importService.SaveTAFLDefinitionToDBAsync(data.Tables);
    }

    /// <summary>
    /// Initiates the TAFL import process for a specified blob.
    /// </summary>
    /// <remarks>This method is triggered by a blob event in the specified storage container. It uses the <see
    /// cref="_importService"/> to begin the import process.</remarks>
    /// <param name="blobData">The stream containing the blob data to be imported.</param>
    /// <param name="name">The name of the blob being processed.</param>
    /// <returns></returns>
    [Function(nameof(RunTAFLImport))]
    public async Task RunTAFLImport([BlobTrigger("canada/csv/unprocessed/{name}", Connection = "canada-blob")] Stream blobData, string name)
    {
        var data = await _importService.BeginTAFLImport(blobData);
    }


#if DEBUG
    [Function("DebugTrigger")]
    public async Task<HttpResponseData> DebugTrigger(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "debug/test")] HttpRequestData req,
        FunctionContext executionContext)
    {
        FileStream originalStream = File.OpenRead("TAFL_LTAF.csv");
        MemoryStream memoryStream = new MemoryStream();
        originalStream.CopyTo(memoryStream);
        memoryStream.Position = 0;
        originalStream.Dispose();

        await RunTAFLImport(memoryStream, "TAFL_LTAF.csv");

        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        response.WriteString("Debug trigger hit successfully!");
        return response;
    }
#endif

}