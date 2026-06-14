namespace Radio_Search.Importer.Canada.Services.Interfaces.EnvironmentManagement
{
    public interface IServiceBusManagement
    {
        /// <summary>
        /// Service call to setup all required filters for the application to properly function
        /// </summary>
        /// <returns>An awaitable task</returns>
        Task SetupFilters();

    }
}
