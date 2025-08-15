namespace Radio_Search.Importer.Canada.Services.Interfaces
{
    public interface IImportManagerService
    {

        /// <summary>
        /// Starts an import job by creating a corresponding Table entry for that import job then triggering the download of
        /// TAFL CSV and TAFL Definition.
        /// </summary>
        /// <returns>An asynchronous task</returns>
        Task StartImportJob();

        /// <summary>
        /// Handles the completion of TAFL Definition and TAFL CSV Download. This function runs in a series of steps.
        /// 1. It updates all Definition rows in the DB
        /// 2. It Generates a list of all license IDs seen inside the TAFL CSV File and persists it to Blob Storage
        /// 3. It Segments out the CSV Files into smaller more manageable "Chunks" that can be further processed and
        /// converted to actual DB updates/inserts further down the line.
        /// </summary>
        /// <param name="importJobID">The import job associated to the download complete</param>
        /// <returns>An asynchronous task</returns>
        Task HandleDownloadComplete(int importJobID);

        /// <summary>
        /// Processes a CSV Chunk record. It calculates the the difference between the DB and the records in the DB
        /// it the updates the DB.
        /// </summary>
        /// <param name="importJobID">The import job associated to the chunk.</param>
        /// <param name="chunkID">The chunks unique ID for the Job ID</param>
        /// <returns>A task</returns>
        Task ProcessChunk(int importJobID, int chunkID);

        /// <summary>
        /// Marks an import job as failed and puts a message into queue to revert the failed import
        /// </summary>
        /// <param name="importJobID">The import job id to mark as failed</param>
        /// <returns>A task</returns>
        Task MarkImportAsFailed(int importJobID);

        /// <summary>
        /// Reverts records associated to the Import Job ID
        /// </summary>
        /// <param name="importJobID">Import Job ID</param>
        /// <returns>A task</returns>
        Task RevertFailedImport(int importJobID);
    }
}
