namespace Radio_Search.Importer.Canada.Services.Interfaces
{
    public interface IFontService
    {
        /// <summary>
        /// Will validate that all required Fonts are available/downloaded and ready to be used.
        /// </summary>
        /// <returns>Returns True when all required Fonts are installed. Returns False when font conditions are not met</returns>
        public bool IsFontsAvailable();

        /// <summary>
        /// Returns the local directory containing all Fonts.
        /// </summary>
        /// <returns></returns>
        public string GetFontLocation();

        /// <summary>
        /// Will download Fonts from a given list of sources. These are set in App Configuration values.
        /// NOTE: This will obtain a lock so only one instance can download Fonts at once. When the lock is
        /// released, other instances will skip the download step and simply return.
        /// </summary>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public Task DownloadFonts();

    }
}
