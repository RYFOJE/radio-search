using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Radio_Search.Importer.Canada.Services.Interfaces.EnvironmentManagement;
using Spire.Pdf;

namespace Radio_Search.Importer.Canada.Services.Implementations
{
    public class RunAtStart : IHostedService
    {
        private readonly IFontManagement _fontManagement;
        private readonly IServiceBusManagement _serviceBusManagement;
        private readonly ILogger<RunAtStart> _logger;

        public RunAtStart(
            IFontManagement fontManagement,
            IServiceBusManagement serviceBusManagement,
            ILogger<RunAtStart> logger)
        {
            _fontManagement = fontManagement;
            _serviceBusManagement = serviceBusManagement;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Beginning to run at start services");
                _logger.LogInformation("Importing Fonts.");
                var fontDir = _fontManagement.InitializaFonts();

                PdfDocument.LoadCustomFontFolder(fontDir);
                _logger.LogInformation("Done importing Fonts. Registered font folder {FontDir}.", fontDir);

                _logger.LogInformation("Setting up servicebus");
                await _serviceBusManagement.SetupFilters();
                _logger.LogInformation("Done setting up servicebus");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occured while setting up environment.");
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}