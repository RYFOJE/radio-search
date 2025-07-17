namespace Radio_Search.Importer.Canada.Services.Exceptions.TAFLDefinitionExceptions
{

    /// <summary>
    /// Represents an exception that is thrown when the number of columns in a table does not match the expected count.
    /// </summary>
    /// <remarks>This exception is specifically used to indicate a mismatch between the expected and actual
    /// number of columns in a table definition, which may be critical for processing or validating table
    /// data.</remarks>
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
