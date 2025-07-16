using Radio_Search.Importer.Canada.Services.Responses;

namespace Radio_Search.Importer.Canada.Services.Interfaces
{
    public interface IDownloadFileService
    {
        /// <summary>
        /// Downloads the most recent TAFL file and saves it to a predefined location.
        /// </summary>
        /// <remarks>This method initiates an asynchronous download of the latest TAFL file.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see
        /// cref="DownloadFileResponse"/> object, which includes details about the download operation, such as the file
        /// path and status.</returns>
        public Task<DownloadFileResponse> DownloadAndSaveRecentTAFL();

        /// <summary>
        /// Downloads the most recent TAFL definition file and saves it to a predefined location.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to retrieve the latest TAFL definition
        /// file. Ensure that the caller handles  the task appropriately, including any exceptions that may occur during
        /// the download process.</remarks>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The task result contains a <see
        /// cref="DownloadFileResponse"/>  indicating the outcome of the download and save operation.</returns>
        public Task<DownloadFileResponse> DownloadAndSaveRecentTAFLDefinition();
    }
}
