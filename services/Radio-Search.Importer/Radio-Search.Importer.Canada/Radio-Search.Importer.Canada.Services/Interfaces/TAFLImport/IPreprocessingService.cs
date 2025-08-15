using Radio_Search.Importer.Canada.Services.Responses;

namespace Radio_Search.Importer.Canada.Services.Interfaces.TAFLImport
{
    public interface IPreprocessingService
    {

        /// <summary>
        /// Generates a chunked file from the provided stream, splitting the data into smaller parts with a specified
        /// number of items per file.
        /// </summary>
        /// <remarks>This method processes the input stream in chunks, creating separate files for each
        /// chunk. It is the caller's responsibility to ensure the <paramref name="streamReader"/> is properly
        /// initialized and disposed of after use.</remarks>
        /// <param name="streamReader">The <see cref="StreamReader"/> used to read the input data stream.</param>
        /// <param name="countPerFile">The maximum number of items to include in each chunked file. Must be greater than zero.</param>
        /// <returns>A <see cref="GenerateChunkResponse"/> object containing details about the generated chunked files, including
        /// their locations and metadata.</returns>
        public Stream GenerateChunkFile(StreamReader streamReader, int countPerFile);

        /// <summary>
        /// Retrieves valid raw rows from the provided file stream.
        /// </summary>
        /// <remarks>This method processes the provided file stream to extract rows that meet the validity criteria.</remarks>
        /// <param name="fileStream">The input stream containing the file data to be processed. Must not be null.</param>
        /// <returns>A <see cref="GetValidRawRowsResponse"/> object containing the valid raw rows extracted from the file.</returns>
        public Task<GetValidRawRowsResponse> GetValidRawRows(Stream fileStream);

    }
}
