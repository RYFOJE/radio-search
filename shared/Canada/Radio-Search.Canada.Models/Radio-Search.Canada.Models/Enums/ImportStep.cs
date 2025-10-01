namespace Radio_Search.Canada.Models.Enums
{
    public enum ImportStep
    {
        // VERY BIG STRONG NOTE FOR MYSELF: THIS IS NOT FUTURE PROOF, IF THERES A STEP GETTING ADDED IT WILL FUDGE UP THE DB BECAUSE VALUES WILL REMAP TO OTHER VALUES.. WHAT THE FREAK
        Reverting = -1,
        DownloadingFiles = 0,
        Chunking = 1,
        ProcessingChunks = 2,
        Complete = 3
    }
}
