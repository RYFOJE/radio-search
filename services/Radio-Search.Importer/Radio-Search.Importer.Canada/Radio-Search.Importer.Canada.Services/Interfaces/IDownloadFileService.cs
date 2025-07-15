using Radio_Search.Importer.Canada.Services.Responses;

namespace Radio_Search.Importer.Canada.Services.Interfaces
{
    public interface IDownloadFileService
    {
        public Task<DownloadFileResponse> DownloadAndSaveRecentTAFL();

        public Task<DownloadFileResponse> DownloadAndSaveRecentTAFLDefinition();
    }
}
