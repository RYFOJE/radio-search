namespace Radio_Search.Importer.Canada.Services.Interfaces
{
    /// <summary>
    /// Provides services for processing PDF documents, including merging multiple pages into a single page.
    /// </summary>
    public interface IPDFProcessingServices
    {
        /// <summary>
        /// Merges multiple PDF pages from the input stream into a single page.
        /// </summary>
        /// <param name="fileStream">The stream containing the PDF document to be merged. Must be readable and seekable.</param>
        /// <returns>A stream containing the merged PDF with all pages combined into a single page.</returns>
        Stream MergePDFToSinglePage(Stream fileStream);

    }
}
