namespace Radio_Search.Importer.Canada.Services.Exceptions.TAFLDefinitionExceptions
{

    /// <summary>
    /// Generic TAFL Definition Import exception.
    /// </summary>
    public class TAFLDefinitionException : Exception
    {
        public TAFLDefinitionException() { }
        public TAFLDefinitionException(string message) : base(message) { }

        public TAFLDefinitionException(string message, Exception innerException) : base(message, innerException) { }
    }
}
