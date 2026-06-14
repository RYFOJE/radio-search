namespace Radio_Search.Importer.Canada.Services.Interfaces.EnvironmentManagement
{
    /// <summary>
    /// Font management service that allows the application to control the fonts available to it
    /// </summary>
    public interface IFontManagement
    {
        /// <summary>
        /// Ensures all required fonts are written to a writable folder and available to the application.
        /// </summary>
        /// <returns>The absolute path of the folder the fonts were written to.</returns>
        string InitializaFonts();
    }
}
