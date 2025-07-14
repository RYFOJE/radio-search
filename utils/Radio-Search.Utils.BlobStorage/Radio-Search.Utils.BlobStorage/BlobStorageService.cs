using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Radio_Search.Utils.BlobStorage.Interfaces;

namespace Radio_Search.Utils.BlobStorage
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageService"/> class with the specified blob service
        /// client and container name.
        /// </summary>
        /// <remarks>This constructor sets up the <see cref="BlobStorageService"/> to operate on a
        /// specific container within the Azure Blob Storage service. Ensure that the <paramref name="containerName"/>
        /// exists in the storage account associated with the <paramref name="blobServiceClient"/>.</remarks>
        /// <param name="blobServiceClient">The client used to interact with the Azure Blob Storage service.</param>
        /// <param name="containerName">The name of the container within the blob storage service. This must be a valid container name as per Azure
        /// Blob Storage naming rules.</param>
        public BlobStorageService(BlobServiceClient blobServiceClient, string containerName)
        {
            _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        }

        ///<inheritdoc/>
        public async Task DeleteAsync(string blobName)
        {
            var blob = _containerClient.GetBlobClient(blobName);

            if (!await blob.ExistsAsync())
                throw new InvalidOperationException($"The blob '{blobName}' does not exist.");

            await blob.DeleteAsync();
        }

        ///<inheritdoc/>
        public async Task<Stream> DownloadAsync(string blobName)
        {
            var blob = _containerClient.GetBlobClient(blobName);

            if (!await blob.ExistsAsync())
                throw new InvalidOperationException($"The blob '{blobName}' does not exist.");

            return await blob.OpenReadAsync();
        }

        public async Task<bool> ExistsAsync(string blobName)
        {
            var blob = _containerClient.GetBlobClient(blobName);
            return await blob.ExistsAsync();
        }

        ///<inheritdoc/>
        public async Task MoveAsync(string fromBlobName, string toBlobName)
        {
            var sourceBlob = _containerClient.GetBlobClient(fromBlobName);
            var toBlob = _containerClient.GetBlobClient(toBlobName);

            if (await toBlob.ExistsAsync())
                throw new InvalidOperationException($"The destination blob '{toBlobName}' already exists.");

            await toBlob.StartCopyFromUriAsync(sourceBlob.Uri);


            BlobProperties destProps;
            do
            {
                await Task.Delay(200);
                destProps = await toBlob.GetPropertiesAsync();
            } while (destProps.CopyStatus == CopyStatus.Pending);

            if (destProps.CopyStatus == CopyStatus.Success)
            {
                await sourceBlob.DeleteAsync();
            }
            else
            {
                throw new Exception($"Copy failed: {destProps.CopyStatusDescription}");
            }
        }

        ///<inheritdoc/>
        public async Task<Uri> UploadAsync(string blobName, Stream content)
        {
            if (content.CanSeek)
            {
                content.Position = 0;
            }

            var blob = _containerClient.GetBlobClient(blobName);

            if (await blob.ExistsAsync())
            {
                throw new InvalidOperationException($"The destination blob '{blobName}' already exists.");
            }

            await blob.UploadAsync(content);

            return blob.Uri;
        }
    }
}
