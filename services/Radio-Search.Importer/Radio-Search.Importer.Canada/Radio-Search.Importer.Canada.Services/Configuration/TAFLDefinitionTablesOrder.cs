using Radio_Search.Importer.Canada.Services.Data.Enums;

namespace Radio_Search.Importer.Canada.Services.Configuration
{

    /// <summary>
    /// Represents the order and headers of TAFL definition tables.
    /// </summary>
    /// <remarks>This class provides properties to manage the headers and order of tables in a TAFL definition
    /// context. It also provides a computed property to convert table names to their corresponding ENUM
    /// values.</remarks>
    public class TAFLDefinitionTablesOrder
    {
        /// <summary>
        /// List of expected headers.
        /// </summary>
        public List<string> Headers { get; set; } = new();

        private List<string> _tableOrder = new();

        /// <summary>
        /// Order of tables in string format.
        /// </summary>
        public List<string> TableOrder
        {
            get => _tableOrder;
            set
            {
                _tableOrder = value;
                _isTableOrderModified = true;
            }
        }

        private bool _isTableOrderModified = true;

        private List<TAFLDefinitionTableEnum> _tableEnumOrder = new();

        /// <summary>
        /// Order of tables in their hardcoded enums.
        /// </summary>          
        public List<TAFLDefinitionTableEnum> TableEnumOrder
        {
            get
            {
                if (!_isTableOrderModified)
                    return _tableEnumOrder;

                _tableEnumOrder = TableOrder.Select(name =>
                {
                    if (!Enum.TryParse<TAFLDefinitionTableEnum>(name, ignoreCase: true, out var parsed))
                        throw new InvalidOperationException($"Table name '{name}' does not match any TableDefinitions enum value.");
                    return parsed;
                }).ToList();

                _isTableOrderModified = false;
                return _tableEnumOrder;
            }
            private set => _tableEnumOrder = value ?? new List<TAFLDefinitionTableEnum>();
        }
    }
}
