using Microsoft.Extensions.DependencyInjection;
using Radio_Search.Importer.Canada.Data.Repositories;
using Radio_Search.Importer.Canada.Data.Repositories.Interfaces;

namespace Radio_Search.Importer.Canada.Data
{
    public static class ConfigurationSetupExtension
    {
        public static IServiceCollection ImporterCanadaAddData(this IServiceCollection services)
        {
            services.AddScoped<ITAFLDefinitionRepo, TAFLDefinitionRepo>();
            services.AddScoped<ITAFLImportHistoryRepo, TAFLImportHistoryRepo>();
            services.AddScoped<ITAFLRepo, TAFLRepo>();

            return services;
        }
    }
}
