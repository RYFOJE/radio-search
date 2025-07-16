namespace Radio_Search.Importer.Canada.Services.Exceptions.TAFLDefinitionExceptions
{
    public class TAFLDefinitionHeaderCountException : TAFLDefinitionException
    {

        private const string ErrorMessage = "An unexpected amount of columns were found in the table.";
        public int ExpectedCount { get; private set; }
        public int ReceivedCount { get; private set; }

        public TAFLDefinitionHeaderCountException(int expectedCount, int receivedCount) : base(ErrorMessage)
        {
            ExpectedCount = expectedCount;
            ReceivedCount = receivedCount;
        }
    }
}
