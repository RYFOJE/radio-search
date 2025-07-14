using Microsoft.Extensions.DependencyInjection;

namespace Radio_Search.Importer.Canada.Services
{
    public static class HttpClientFactoryExtension
    {

        public static IServiceCollection ImporterCanadaAddHTTPClients(this IServiceCollection services)
        {
            services.AddHttpClient(HttpClientNames.TAFL_DOWNLOADER, x => { });
            services.AddHttpClient(HttpClientNames.TAFL_FIELD_DESCRIPTION_DOWNLOADER, x => { });

            return services;
        }

    }
}
