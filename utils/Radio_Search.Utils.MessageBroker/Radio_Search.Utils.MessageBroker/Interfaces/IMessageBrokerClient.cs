namespace Radio_Search.Utils.MessageBroker.Interfaces
{
    public interface IMessageBrokerClient
    {
        public IMessageBrokerWriter GetMessageWriter(string destinationName);
    }
}
