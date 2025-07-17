namespace Radio_Search.Importer.Canada.Services.Exceptions.TAFLDefinitionExceptions
{

    /// <summary>
    /// Represents an exception that is thrown when a header name does not match the expected value in a TAFL
    /// definition.
    /// </summary>
    /// <remarks>This exception is typically used to indicate that the header name received does not match the
    /// expected header name during the processing of a TAFL definition. It provides details about both the expected and
    /// received header names.</remarks>
    public class TAFLDefinitionHeaderMismatchException : TAFLDefinitionException
    {

        private const string ErrorMessage = "An unexpected header name was found.";
        public string ExpectedHeaderName { get; private set; }
        public string ReceivedHeaderName { get; private set; }

        public TAFLDefinitionHeaderMismatchException(string expectedHeaderName, string receivedHeaderName) : base(ErrorMessage)
        {
            ExpectedHeaderName = expectedHeaderName;
            ReceivedHeaderName = receivedHeaderName;
        }

    }
}
