namespace Radio_Search.Importer.Canada.Services.Data
{
    public class TableDefinitionRow : IEquatable<TableDefinitionRow>
    {
        public string Code { get; set; } = string.Empty;
        public string DescriptionEN { get; set; } = string.Empty;
        public string DescriptionFR { get; set; } = string.Empty;


        public bool Equals(TableDefinitionRow? other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(other, this))
                return true;

            return string.Equals(Code, other.Code);
        }
    }
}
