namespace Radio_Search.Utils.MessageBroker.Exceptions
{
    public class MessageBrokerWriteException : Exception
    {

        public MessageBrokerWriteException() { }

        public MessageBrokerWriteException(string message) : base(message) { }

        public MessageBrokerWriteException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
