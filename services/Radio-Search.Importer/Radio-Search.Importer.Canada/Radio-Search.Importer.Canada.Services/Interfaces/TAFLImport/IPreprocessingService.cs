using Radio_Search.Importer.Canada.Services.Data;
using Radio_Search.Importer.Canada.Services.Responses;

namespace Radio_Search.Importer.Canada.Services.Interfaces.TAFLImport
{
    public interface IPreprocessingService
    {

        /// <summary>
        /// Removes duplicate rows from a TAFL file and returns a list of unique
        /// entries.
        /// </summary>
        /// <remarks>This method processes the provided TAFL file stream to identify and remove duplicate
        /// rows based on the file's content. The caller is responsible for ensuring the stream is valid and properly
        /// formatted.</remarks>
        /// <param name="fullTAFLStream">A <see cref="Stream"/> containing the full TAFL file data. The stream must be readable and positioned at the
        /// beginning of the file.</param>
        /// <returns>A list of <see cref="TAFLEntryRawRow"/> objects representing the unique rows in the TAFL file.</returns>
        public List<TAFLEntryRawRow> DeduplicateFullFile(Stream fullTAFLStream);

        
        /// <summary>
        /// Generates a stream containing the chunk file data based on the provided rows.
        /// </summary>
        /// <remarks>This method processes the provided rows and generates a chunk file in a stream
        /// format. The stream can be used to save the chunk file to disk or transmit it over a network. Ensure that the
        /// input rows meet the required format and constraints for successful processing.</remarks>
        /// <param name="rows">A list of <see cref="TAFLEntryRawRow"/> objects representing the raw data rows to be included in the chunk
        /// file. Each row must contain valid data for the chunk file generation process.</param>
        /// <returns>A <see cref="Stream"/> containing the generated chunk file data. The caller is responsible for disposing of
        /// the stream.</returns>
        public Stream GenerateChunkFile(List<TAFLEntryRawRow> rows);

        /// <summary>
        /// Retrieves valid raw rows from the provided file stream.
        /// </summary>
        /// <remarks>This method processes the provided file stream to extract rows that meet the validity criteria.</remarks>
        /// <param name="fileStream">The input stream containing the file data to be processed. Must not be null.</param>
        /// <returns>A <see cref="GetValidRawRowsResponse"/> object containing the valid raw rows extracted from the file.</returns>
        public Task<GetValidRawRowsResponse> GetValidRawRows(Stream fileStream);

    }
}
