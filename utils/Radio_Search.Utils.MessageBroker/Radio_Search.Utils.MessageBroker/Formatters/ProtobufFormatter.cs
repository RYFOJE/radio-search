using Google.Protobuf;
using Radio_Search.Utils.MessageBroker.Exceptions;
using Radio_Search.Utils.MessageBroker.Formatters.Interfaces;

namespace Radio_Search.Utils.MessageBroker.Formatters
{
    internal class ProtobufFormatter : IFormatter
    {

        /// <inheritdoc/>
        public T Deserialize<T>(byte[] value)
        {
            if (!typeof(IMessage).IsAssignableFrom(typeof(T)))
                throw new SerializationException("Cannot deserialize to a type that is not a protobuf.");

            try
            {
                var message = (IMessage)Activator.CreateInstance(typeof(T))!;
                message.MergeFrom(new CodedInputStream(value));
                return (T)message;
            }
            catch (Exception ex)
            {
                throw new SerializationException("Failed to deserialize value.", ex);
            }
        }

        /// <inheritdoc/>
        public byte[] Serialize(object value)
        {
            if (value is not IMessage)
                throw new SerializationException("Failed to serialize message as it was not a Protobuf IMessage.");

            try
            {
                return ((IMessage)value).ToByteArray();
            }
            catch (Exception ex)
            {
                throw new SerializationException("Failed to serialize value.", ex);
            }
        }
    }
}
