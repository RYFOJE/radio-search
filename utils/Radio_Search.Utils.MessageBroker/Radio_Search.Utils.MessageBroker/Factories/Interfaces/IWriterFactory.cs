using Radio_Search.Utils.MessageBroker.Interfaces;

namespace Radio_Search.Utils.MessageBroker.Factories.Interfaces
{
    public interface IWriterFactory
    {
        /// <summary>
        /// Creates an asynchronous message broker writer for the specified destination.
        /// </summary>
        /// <remarks>The returned <see cref="IMessageBrokerWriter"/> can be used to send messages to the
        /// specified  destination. Ensure proper disposal of the writer to release any associated resources.</remarks>
        /// <param name="DestinationName">The name of the destination (e.g., topic or queue) to which the writer will send messages.  This value
        /// cannot be null or empty.</param>
        /// <param name="ClientName">An optional client name to associate with the writer. If null, a default client name will be used.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see
        /// cref="IMessageBrokerWriter"/> instance configured for the specified destination.</returns>
        IMessageBrokerWriter GetMessageBrokerWriter(string DestinationName, string? ClientName = null);
    }
}
