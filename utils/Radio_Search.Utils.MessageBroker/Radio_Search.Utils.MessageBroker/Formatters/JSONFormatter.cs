using Radio_Search.Utils.MessageBroker.Exceptions;
using Radio_Search.Utils.MessageBroker.Formatters.Interfaces;
using System.Text.Json;

namespace Radio_Search.Utils.MessageBroker.Formatters
{
    public class JSONFormatter : IFormatter
    {
        /// <inheritdoc/>
        public T Deserialize<T>(byte[] value)
        {
            T? deserializedValue;

            try
            {
                deserializedValue = JsonSerializer.Deserialize<T>(value);
            }
            catch (Exception ex){
                throw new SerializationException("Failed to deserialize message.", ex);
            }

            if (deserializedValue == null)
                throw new SerializationException("Deserialized to a null value.");

            return deserializedValue;
        }

        /// <inheritdoc/>
        public byte[] Serialize(object? value)
        {
            if (value == null)
                return [];

            try
            {
                return JsonSerializer.SerializeToUtf8Bytes(value);
            }
            catch (Exception ex)
            {
                throw new SerializationException("Failed to serialize object.", ex);
            }
        }
    }
}
