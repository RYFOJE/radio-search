namespace Radio_Search.Utils.BlobStorage.Interfaces
{
    /// <summary>
    /// Provides an interface for asynchronous operations on blob storage, including uploading, downloading, checking
    /// existence, deleting, and moving blobs.
    /// </summary>
    /// <remarks>Implementations of this interface should handle the underlying storage details, allowing
    /// users to interact with blob storage in a consistent manner. Users are responsible for managing the lifecycle of
    /// any streams returned by these methods.</remarks>
    public interface IBlobStorageService
    {
        /// <summary>
        /// Asynchronously uploads a stream to a blob storage and returns the URI of the uploaded blob.
        /// </summary>
        /// <param name="blobName">The name of the blob to which the content will be uploaded. Cannot be null or empty.</param>
        /// <param name="content">The stream containing the content to upload. The stream must be readable and will be read from its current
        /// position to the end.</param>
        /// <returns>A <see cref="Uri"/> representing the location of the uploaded blob.</returns>
        Task<Uri> UploadAsync(string blobName, Stream content);

        /// <summary>
        /// Asynchronously retrieves a writable <see cref="Stream"/> for the specified blob.
        /// </summary>
        /// <remarks>The returned <see cref="Stream"/> must be properly disposed after use to ensure that 
        /// resources are released and the write operation is finalized. If <paramref name="overwrite"/>  is <see
        /// langword="false"/> and the blob already exists, the operation may fail depending on  the
        /// implementation.</remarks>
        /// <param name="blobName">The name of the blob to write to. This cannot be null or empty.</param>
        /// <param name="overwrite">A value indicating whether to overwrite the blob if it already exists.  <see langword="true"/> to overwrite
        /// the existing blob; otherwise, <see langword="false"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a writable  <see cref="Stream"/>
        /// for the specified blob.</returns>
        Task<Stream> GetWriteStream(string blobName, bool overwrite = false);

        /// <summary>
        /// Asynchronously downloads the specified blob and returns its content as a stream.
        /// </summary>
        /// <remarks>The caller is responsible for disposing the returned <see cref="Stream"/> to free
        /// resources.</remarks>
        /// <param name="blobName">The name of the blob to download. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous download operation. The task result contains a <see cref="Stream"/>
        /// of the blob's content.</returns>
        Task<Stream> DownloadAsync(string blobName);

        /// <summary>
        /// Determines whether a blob with the specified name exists asynchronously.
        /// </summary>
        /// <param name="blobName">The name of the blob to check for existence. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the
        /// blob exists; otherwise, <see langword="false"/>.</returns>
        Task<bool> ExistsAsync(string blobName);

        /// <summary>
        /// Deletes the specified blob asynchronously.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to delete a blob identified by its
        /// name. Ensure that the blob name is correctly specified to avoid exceptions.</remarks>
        /// <param name="blobName">The name of the blob to delete. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        Task DeleteAsync(string blobName);

        /// <summary>
        /// Asynchronously moves a blob from the specified source name to the specified destination name.
        /// </summary>
        /// <remarks>This method performs the move operation by copying the blob to the new location and
        /// then deleting the original blob. Ensure that both the source and destination names are valid and that the
        /// destination does not already exist unless overwriting is intended.</remarks>
        /// <param name="fromBlobName">The name of the blob to move. Cannot be null or empty.</param>
        /// <param name="toBlobName">The name of the destination blob. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous move operation.</returns>
        Task MoveAsync(string fromBlobName, string toBlobName);
    }
}
