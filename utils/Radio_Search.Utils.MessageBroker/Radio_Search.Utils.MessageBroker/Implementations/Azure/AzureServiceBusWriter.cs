using Azure.Messaging.ServiceBus;
using Radio_Search.Utils.MessageBroker.Enums;
using Radio_Search.Utils.MessageBroker.Exceptions;
using Radio_Search.Utils.MessageBroker.Formatters;
using Radio_Search.Utils.MessageBroker.Interfaces;

namespace Radio_Search.Utils.MessageBroker.Implementations.Azure
{
    public class AzureServiceBusWriter : IMessageBrokerWriter
    {
        private readonly ServiceBusSender _sender;
        private readonly string _destinationName;

        internal AzureServiceBusWriter(string destinationName, ServiceBusClient client) 
        {
            if (string.IsNullOrWhiteSpace(destinationName))
                throw new InvalidMessageBrokerClientException("Destination Name cannot be Null or Whitespace.");

            _destinationName = destinationName;

            try
            {
                _sender = client.CreateSender(destinationName);
            }
            catch(Exception ex) 
            {
                throw new InvalidMessageBrokerClientException(ex.Message, ex);
            }
        }

        /// <inheritdoc/>
        public async Task WriteMessageAsync(object message, string? target = null, FormatTypes type = FormatTypes.JSON)
        {
            var formatter = Formatter.GetFormatter(type);
            var messageToBeSent = formatter.Serialize(message);

            ServiceBusMessage sbMessage = new()
            {
                Body = new BinaryData(messageToBeSent ?? [])
            };

            if (target != null)
            {
                sbMessage.ApplicationProperties["Target"] = target;
            }

            try
            {
                await _sender.SendMessageAsync(sbMessage);
            }
            catch (Exception ex)
            {
                throw new MessageBrokerWriteException(ex.Message, ex);
            }
        }
    }
}
