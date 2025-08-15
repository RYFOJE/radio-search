using Radio_Search.Importer.Canada.Services.Data;
using Radio_Search.Importer.Canada.Services.Data.Enums;
using Radio_Search.Importer.Canada.Services.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio_Search.Importer.Canada.Services.Interfaces.TAFLDefinitionImport
{
    public interface ITAFLDefinitionImportService
    {

        /// <summary>
        /// Processes a TAFL file definition from the provided PDF stream.
        /// </summary>
        /// <remarks>The method reads and processes the TAFL definition from the specified stream,
        /// returning a response that includes the outcome of the processing. Ensure the stream is positioned at the
        /// beginning of the TAFL definition before calling this method.</remarks>
        /// <param name="stream">The input stream containing the TAFL definition to be processed. Must not be null.</param>
        /// <returns>A <see cref="ProcessTAFLDefinitionResponse"/> object containing the results of the processing operation.</returns>
        ProcessTAFLDefinitionResponse ProcessTAFLDefinition(Stream stream);

        /// <summary>
        /// Saves the specified table definitions and their associated rows to the database.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to persist the provided table
        /// definitions and their rows into the database. Ensure that the <paramref name="Tables"/> parameter is not
        /// <see langword="null"/> and contains valid data before calling this method.</remarks>
        /// <param name="Tables">A dictionary where each key is a <see cref="TAFLDefinitionTableEnum"/> object representing a table definition, and
        /// each value is a <see cref="HashSet{T}"/> of <see cref="TAFLDefinitionRawRow"/> objects representing the rows
        /// to be saved.</param>
        /// <returns>The total count of modified/inserted/delete rows</returns>
        Task<int> SaveTAFLDefinitionToDBAsync(Dictionary<TAFLDefinitionTableEnum, HashSet<TAFLDefinitionRawRow>> Tables);

    }
}
