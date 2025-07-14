namespace Radio_Search.Importer.Canada.Services.Interfaces
{
    /// <summary>
    /// Used to check to see when the TAFL has last updated
    /// </summary>
    public interface IUpdateVerificationService
    {

        Task<DateOnly> GetTAFLUpdateDateAsync();
    }
}
