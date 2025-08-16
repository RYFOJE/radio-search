using Radio_Search.Utils.MessageBroker.Implementations;

namespace Radio_Search.Utils.MessageBroker.Interfaces
{
    public interface IMessageBrokerClient
    {
        public IMessageBrokerWriter GetMessageWriter(string destinationName);

        public Task<IMessageBrokerClient> AddMessageFilter(string destinationName, MessageFilter filter);
    }
}
