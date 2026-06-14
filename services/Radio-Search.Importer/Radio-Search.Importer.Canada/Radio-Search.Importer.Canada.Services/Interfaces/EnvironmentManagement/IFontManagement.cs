namespace Radio_Search.Importer.Canada.Services.Interfaces.EnvironmentManagement
{
    /// <summary>
    /// Font management service that allows the application to control the fonts available to it
    /// </summary>
    public interface IFontManagement
    {
        /// <summary>
        /// Ensures all required fonts are installed and available to the application
        /// </summary>
        /// <returns></returns>
        void InitializaFonts();
    }
}
