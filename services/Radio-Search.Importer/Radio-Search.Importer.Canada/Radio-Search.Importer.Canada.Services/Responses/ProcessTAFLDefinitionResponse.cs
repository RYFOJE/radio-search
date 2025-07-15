using Radio_Search.Importer.Canada.Services.Data;
using Radio_Search.Importer.Canada.Services.Data.Enums;

namespace Radio_Search.Importer.Canada.Services.Responses
{

    /// <summary>
    /// Represents the response containing processed table definitions and their associated rows.
    /// </summary>
    /// <remarks>This class is used to encapsulate the results of processing table definitions,  providing a
    /// mapping between each table definition and its corresponding set of rows.</remarks>
    public class ProcessTAFLDefinitionResponse : ResponseBase
    {

        /// <summary>
        /// Gets or sets the collection of tables, where each table is defined by a <see cref="TableDefinitions"/> key
        /// and contains a set of <see cref="TableDefinitionRow"/> entries.
        /// </summary>
        public Dictionary<TableDefinitions, HashSet<TableDefinitionRow>> Tables { get; set; } = new();
    }
}
