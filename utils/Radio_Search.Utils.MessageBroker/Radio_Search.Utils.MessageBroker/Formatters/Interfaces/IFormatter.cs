namespace Radio_Search.Utils.MessageBroker.Formatters.Interfaces
{
    public interface IFormatter
    {

        /// <summary>
        /// Converts a serialized message
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public byte[] Serialize(object? value);

        /// <summary>
        /// Converts the specified object to an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type to which the object should be deserialized.</typeparam>
        /// <param name="value">The object to deserialize. Must be compatible with the target type <typeparamref name="T"/>.</param>
        /// <returns>An instance of type <typeparamref name="T"/> representing the deserialized object.</returns>
        public T Deserialize<T>(byte[] value);

    }
}