using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Radio_Search.Utils.BlobStorage.Exceptions;
using Radio_Search.Utils.BlobStorage.Interfaces;

namespace Radio_Search.Utils.BlobStorage
{
    public class BlobLockClient(BlobClient _blobClient) : IBlobLockClient
    {
        private BlobLeaseClient? _leaseClient;

        /// <inheritdoc/>
        public async Task AcquireLock(TimeSpan lockTime)
        {
            try
            {
                _leaseClient ??= _blobClient.GetBlobLeaseClient();
                await _leaseClient.AcquireAsync(lockTime);
            }
            catch (RequestFailedException ex) when (ex.ErrorCode == "LeaseAlreadyPresent")
            {
                throw new AcquireLeaseException(ex, "Could not acquire lease");
            }
        }

        /// <inheritdoc/>
        public async Task<bool> IsLockedAsync()
        {
            var blobDetails = await _blobClient.GetPropertiesAsync();
            return blobDetails.Value.LeaseState == LeaseState.Leased;
        }

        /// <inheritdoc/>
        public async Task RemoveLock()
        {
            if (_leaseClient is null)
                return;
            await _leaseClient.ReleaseAsync();
        }

        /// <inheritdoc/>
        public async Task RenewLock()
        {
            if (_leaseClient == null)
                throw new NullReferenceException("Lease Client is Null.");
            await _leaseClient.RenewAsync();
        }
    }
}