namespace Radio_Search.Importer.Canada.Services.Exceptions.TAFLDefinitionExceptions
{
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
