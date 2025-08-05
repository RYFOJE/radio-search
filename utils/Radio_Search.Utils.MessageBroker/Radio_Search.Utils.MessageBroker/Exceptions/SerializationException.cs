namespace Radio_Search.Utils.MessageBroker.Exceptions
{
    public class SerializationException : Exception
    {
        Exception? ex;
        public SerializationException() { }

        public SerializationException(string message) : base(message) { }

        public SerializationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
