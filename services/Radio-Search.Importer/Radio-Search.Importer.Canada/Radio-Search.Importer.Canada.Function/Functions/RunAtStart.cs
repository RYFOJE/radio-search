using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Radio_Search.Importer.Canada.Services.Interfaces.EnvironmentManagement;

namespace Radio_Search.Importer.Canada.Services.Implementations
{
    public class RunAtStart : IHostedService
    {
        private readonly IServiceBusManagement _serviceBusManagement;
        private readonly ILogger<RunAtStart> _logger;

        public RunAtStart(
            IServiceBusManagement serviceBusManagement,
            ILogger<RunAtStart> logger)
        {
            _serviceBusManagement = serviceBusManagement;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Beginning to run at start services");

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