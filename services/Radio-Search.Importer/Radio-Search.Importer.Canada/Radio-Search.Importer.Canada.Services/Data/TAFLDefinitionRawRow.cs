namespace Radio_Search.Importer.Canada.Services.Data
{
    public class TAFLDefinitionRawRow : IEquatable<TAFLDefinitionRawRow>
    {
        public string Code { get; set; } = string.Empty;
        public string DescriptionEN { get; set; } = string.Empty;
        public string DescriptionFR { get; set; } = string.Empty;


        public bool Equals(TAFLDefinitionRawRow? other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(other, this))
                return true;

            return string.Equals(Code, other.Code);
        }
    }
}
