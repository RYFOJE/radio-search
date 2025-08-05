using Radio_Search.Utils.MessageBroker.Enums;

namespace Radio_Search.Utils.MessageBroker.Interfaces
{
    public interface IMessageBrokerWriter
    {
        /// <summary>
        /// Asynchronously writes a message to the output in the specified format.
        /// </summary>
        /// <param name="message">The message object to be written. This cannot be null.</param>
        /// <param name="subject">An optional subject associated with the message. If null, no subject will be included.</param>
        /// <param name="type">The format in which the message should be written. The default is <see cref="FormatTypes.JSON"/>.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        Task WriteMessageAsync(object message, string? subject = null, FormatTypes type = FormatTypes.JSON);
    }
}
