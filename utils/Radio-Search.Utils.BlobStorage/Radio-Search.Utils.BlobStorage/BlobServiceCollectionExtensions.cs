using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;
using Radio_Search.Utils.BlobStorage.Interfaces;

namespace Radio_Search.Utils.BlobStorage
{
    public static class BlobServiceCollectionExtensions
    {
        public static IServiceCollection AddBlobStorage(this IServiceCollection services, string blobConnectionString, string containerName)
        {
            services.AddSingleton<IBlobStorageService>(provider =>
            {
                var client = new BlobServiceClient(
                    new Uri(blobConnectionString),
                    new DefaultAzureCredential());

                return new BlobStorageService(client, containerName);
            });

            return services;
        }
    }
}
