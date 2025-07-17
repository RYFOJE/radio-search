using Radio_Search.Importer.Canada.Services.Responses;

namespace Radio_Search.Importer.Canada.Services.Interfaces
{
    /// <summary>
    /// Used to check to see when the TAFL has last updated
    /// </summary>
    public interface IUpdateVerificationService
    {
        /// <summary>
        /// Asynchronously retrieves the date of the latest TAFL update.
        /// </summary>
        /// <returns>A <see cref="GetTAFLUpdateDateResponse"/>Representing the date of the most recent TAFL update.</returns>
        Task<GetTAFLUpdateDateResponse> GetTAFLUpdateDateAsync();
    }
}
