using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Radio_Search.Importer.Canada.Services.Configuration;
using Radio_Search.Importer.Canada.Services.Interfaces;
using Radio_Search.Importer.Canada.Services.Responses;
using Radio_Search.Utils.BlobStorage.Interfaces;
using System.IO.Compression;

namespace Radio_Search.Importer.Canada.Services.Implementations
{
    public class DownloadFileService : IDownloadFileService
    {
        private readonly ILogger<DownloadFileService> _logger;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DownloaderURLs _URLs;
        private readonly IBlobStorageService _blobStorageService;

        public DownloadFileService(
            ILogger<DownloadFileService> logger,
            IConfiguration config,
            IHttpClientFactory httpFactory,
            IOptions<DownloaderURLs> URLs,
            IBlobStorageService blobStorageService)
        {
            _logger = logger;
            _config = config;
            _httpClientFactory = httpFactory;
            _URLs = URLs.Value;
            _blobStorageService = blobStorageService;
        }

        #region public

        /// <inheritdoc/>
        public async Task<DownloadFileResponse> DownloadAndSaveRecentTAFL()
        {
            Stream? taflStream = null;
            Stream? unzippedFileStream = null;
            var newFileName = DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyy-MM-dd") + ".csv";
            Uri resp;

            try
            {
                taflStream = await DownloadTAFLFromSource();
                unzippedFileStream = UnzipSingleFile(taflStream);

                resp = await _blobStorageService.UploadAsync($"csv/unprocessed/{newFileName}", unzippedFileStream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the download and unzip process.");

                return new()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
            finally
            {
                taflStream?.Dispose();
                unzippedFileStream?.Dispose();
            }

            return new()
            {
                Success = true,
                Message = "Successfully uploaded new TAFL File",
                FileName = newFileName,
                FullPath = resp
            };
        }

        /// <inheritdoc/>
        public async Task<DownloadFileResponse> DownloadAndSaveRecentTAFLDefinition()
        {
            Stream? taflStream = null;
            var newFileName = DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyy-MM-dd") + ".pdf";
            Uri resp;

            try
            {
                taflStream = await DownloadTAFLDefinitionFromSource();
                resp = await _blobStorageService.UploadAsync($"pdf/unprocessed/{newFileName}", taflStream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the download and unzip process.");

                return new()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
            finally
            {
                taflStream?.Dispose();
            }

            return new()
            {
                Success = true,
                Message = "Successfully uploaded new TAFL Definition File",
                FileName = newFileName,
                FullPath = resp
            };
        }

        #endregion

        #region private

        /// <summary>
        /// Downloads the file from the Canadian website
        /// </summary>
        /// <returns>A stream representing the full file</returns>
        private async Task<Stream> DownloadTAFLFromSource()
        {
            try
            {
                var client = _httpClientFactory.CreateClient(HttpClientNames.TAFL_DOWNLOADER);

                return await client.GetStreamAsync(_URLs.TAFLUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while attempting to DownloadTAFLFromSource URL:{TAFL_URL}", _URLs.TAFLUrl);
                throw;
            }
        }

        private async Task<Stream> DownloadTAFLDefinitionFromSource()
        {
            try
            {
                var client = _httpClientFactory.CreateClient(HttpClientNames.TAFL_DOWNLOADER);

                return await client.GetStreamAsync(_URLs.TAFLDescriptionUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while attempting to DownloadTAFLDefinitionFromSource URL:{TAFL_URL}", _URLs.TAFLDescriptionUrl);
                throw;
            }
        }

        /// <summary>
        /// Extracts the first file from a ZIP archive provided as a stream.
        /// </summary>
        /// <remarks>The method assumes the input stream is positioned at the beginning of the ZIP
        /// archive. The input stream is not closed or disposed by this method.</remarks>
        /// <param name="inputStream">The input stream containing the ZIP archive. The stream must support seeking.</param>
        /// <returns>A stream representing the extracted file. The caller is responsible for disposing of this stream.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the ZIP archive is empty.</exception>
        private Stream UnzipSingleFile(Stream inputStream)
        {
            try
            {
                // Ensure the stream is seekable
                Stream seekableStream;
                if (!inputStream.CanSeek)
                {
                    seekableStream = new MemoryStream();
                    inputStream.CopyTo(seekableStream);
                    seekableStream.Position = 0;
                }
                else
                {
                    seekableStream = inputStream;
                    seekableStream.Position = 0;
                }

                var archive = new ZipArchive(seekableStream, ZipArchiveMode.Read, leaveOpen: true);

                if (archive.Entries.Count > 1)
                {
                    archive.Dispose();
                    throw new InvalidOperationException("More than one file contained inside the ZIP File.");
                }

                var entry = archive.Entries.FirstOrDefault();
                if (entry == null)
                {
                    archive.Dispose();
                    throw new InvalidOperationException("ZIP archive is empty.");
                }

                var entryStream = entry.Open();

                return entryStream;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while attempting to UnzipFile.");
                throw;
            }
        }

        #endregion
    }
}
