using Radio_Search.Importer.Canada.Services.Data.Enums;

namespace Radio_Search.Importer.Canada.Services.Exceptions.TAFLDefinitionExceptions
{
    public class TAFLDefinitionException : Exception
    {
        public TAFLDefinitionException() { }
        public TAFLDefinitionException(string message) : base(message) { }

        public TAFLDefinitionException(string message, Exception innerException) : base(message, innerException) { }
    }
}
