using Radio_Search.Importer.Canada.Services.Data.Enums;

namespace Radio_Search.Importer.Canada.Services.Configuration
{
    public class TAFLDefinitionTablesOrder
    {
        public List<string> Headers { get; set; } = new();

        private List<string>? _tableNames;
        public List<string> TableOrder
        {
            get => _tableNames!;
            set
            {
                if (_tableNames != null)
                    throw new InvalidOperationException("TableNames can only be set once.");
                _tableNames = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        private List<TableDefinitions>? _tableEnumOrder;
        public List<TableDefinitions> TableEnumOrder
        {
            get
            {
                if (_tableEnumOrder == null)
                {
                    var enums = new List<TableDefinitions>();
                    foreach (var name in TableOrder)
                    {
                        if (!Enum.TryParse<TableDefinitions>(name, ignoreCase: true, out var parsed))
                            throw new InvalidOperationException($"Table name '{name}' does not match any TableDefinitions enum value.");
                        enums.Add(parsed);
                    }
                    _tableEnumOrder = enums;
                }
                return _tableEnumOrder;
            }
        }
    }
}
