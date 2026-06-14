using CsvHelper.Configuration.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Radio_Search.Importer.Canada.Services.Configuration;
using Radio_Search.Importer.Canada.Services.Interfaces;
using Radio_Search.Utils.BlobStorage.Interfaces;
using System.Diagnostics;

namespace Radio_Search.Importer.Canada.Services.Implementations
{
    public class FontService(
        ILogger<FontService> _logger, 
        IOptionsMonitor<RequiredFonts> _requiredFonts,
        IBlobStorageService _blobStorageService
        ) : IFontService
    {
        /// <summary>
        /// Stale time for font, once passed that time. Trigger a redownload
        /// </summary>
        private readonly TimeSpan FONT_STALE_TIME = TimeSpan.FromDays(31);

        /// <summary>
        /// The last time fonts were downloaded, use this so that when the lock releases the other instances don't
        /// download the file again
        /// </summary>
        private readonly DateTime _lastDownloadTime = DateTime.MinValue;

        /// <summary>
        /// Directory name to be appended to temp directory
        /// </summary>
        private const string TEMP_FONT_DIRECTORY_NAME = "Fonts";
        private const string BLOB_FONT_SUBDIRECTORY = @"HelperFiles/Fonts";
        private string _lockFileLocation => Path.Combine(TEMP_FONT_DIRECTORY_NAME, "lockfile");
        private readonly TimeSpan lockAcquireRetryTime = TimeSpan.FromSeconds(10);
        private const int MAX_CONCURRENT_DOWNLOADS = 3;

        ///<inheritdoc/>
        public bool IsFontsAvailable()
        {
            var directory = GetFontLocation();

            try
            {
                if (!Directory.Exists(directory))
                    return false;

                var directoryInformation = new DirectoryInfo(directory).EnumerateFiles();

                return _requiredFonts.CurrentValue.Fonts.All(x =>
                {
                    var fontFile = directoryInformation.FirstOrDefault(ff => ff.Name == x.FileName);

                    if (fontFile is null)
                        return false;

                    if ((DateTime.UtcNow - fontFile.CreationTimeUtc) > FONT_STALE_TIME)
                        return false;

                    return true;
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while checking IsFontsAvailable for directory: {Directory}", directory);
                throw;
            }
        }

        ///<inheritdoc/>
        public async Task DownloadFonts()
        {
            var timer = Stopwatch.StartNew();

            if ((DateTime.UtcNow - _lastDownloadTime) < FONT_STALE_TIME)
                return;

            var lockClient = _blobStorageService.GetBlobLeaseClient(_lockFileLocation);

            // Wait to acquire lock on fonts directory
            await WaitTillLockAcquired(lockClient);
            var blobFiles = await _blobStorageService.ListBlobsForDirectory(BLOB_FONT_SUBDIRECTORY);
            // TODO: Font stale time should be addressed here to refresh those fonts in blob

            var missingFiles = _requiredFonts.CurrentValue.Fonts.Where(x => !blobFiles.Contains(x.FileName));

            Parallel.ForEach(missingFiles, new() { MaxDegreeOfParallelism = MAX_CONCURRENT_DOWNLOADS }, x =>
            {

            });

            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public string GetFontLocation()
        {
            var tempDir = System.IO.Path.GetTempPath();
            return Path.Combine(tempDir, TEMP_FONT_DIRECTORY_NAME);
        }


        private async Task WaitTillLockAcquired(IBlobLockClient lockClient, uint maxWaitTimeMinutes = 3)
        {
            DateTime startTime = DateTime.UtcNow;
            TimeSpan maxTimeSpan = TimeSpan.FromMinutes(maxWaitTimeMinutes);

            while (true)
            {
                if ((DateTime.UtcNow - startTime) > maxTimeSpan)
                    throw new TimeoutException("Timed out while waiting to acquire lock.");

                if (!await lockClient.IsLockedAsync())
                    break;

                // Wait to retry
                await Task.Delay(lockAcquireRetryTime);
            }
        }

    }
}
