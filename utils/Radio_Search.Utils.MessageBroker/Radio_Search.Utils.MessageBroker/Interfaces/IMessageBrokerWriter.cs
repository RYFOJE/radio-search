using Radio_Search.Utils.MessageBroker.Enums;

namespace Radio_Search.Utils.MessageBroker.Interfaces
{
    public interface IMessageBrokerWriter
    {
        /// <summary>
        /// Asynchronously writes a message to the output in the specified format.
        /// </summary>
        /// <param name="message">The message object to be written. This cannot be null.</param>
        /// <param name="target">An optional target associated with the message. If null, no target will be included.</param>
        /// <param name="type">The format in which the message should be written. The default is <see cref="FormatTypes.JSON"/>.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        Task WriteMessageAsync(object message, string? target = null, FormatTypes type = FormatTypes.JSON);
    }
}
