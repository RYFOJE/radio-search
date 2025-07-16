using Radio_Search.Importer.Canada.Services.Data.Enums;

namespace Radio_Search.Importer.Canada.Services.Configuration
{
    public class TAFLDefinitionTablesOrder
    {
        public List<string> Headers { get; set; } = new();

        public List<string> TableOrder { get; set; } = new();

        public List<TableDefinitions> TableEnumOrder =>
            TableOrder.Select(name =>
            {
                if (!Enum.TryParse<TableDefinitions>(name, ignoreCase: true, out var parsed))
                    throw new InvalidOperationException($"Table name '{name}' does not match any TableDefinitions enum value.");
                return parsed;
            }).ToList();
    }
}
