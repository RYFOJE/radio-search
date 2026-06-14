namespace Radio_Search.Utils.BlobStorage.Interfaces
{
    public interface IBlobLockClient
    {

        /// <summary>
        /// Asynchronously determines whether the resource is currently locked.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the
        /// resource is locked; otherwise, <see langword="false"/>.</returns>
        Task<bool> IsLockedAsync();

        /// <summary>
        /// Attempts to acquire a lock for the specified duration.
        /// </summary>
        /// <param name="lockTime">The length of time for which the lock should be held. Must be a positive value.</param>
        /// <returns>A task that represents the asynchronous operation of acquiring the lock.</returns>
        Task AcquireLock(TimeSpan lockTime);

        /// <summary>
        /// Removes the lock on the resource.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation of removing the lock.</returns>
        Task RemoveLock();

        /// <summary>
        /// Renews the lock on the resource for the specified duration.
        /// </summary>
        /// <param name="lockTime">The length of time to extend the lock. Must be a positive value.</param>
        /// <returns>A task that represents the asynchronous operation of renewing the lock.</returns>
        Task RenewLock();

    }
}
