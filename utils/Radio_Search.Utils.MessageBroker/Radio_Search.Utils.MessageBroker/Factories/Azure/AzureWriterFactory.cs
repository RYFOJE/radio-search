using Microsoft.Extensions.DependencyInjection;
using Radio_Search.Utils.MessageBroker.Factories.Interfaces;
using Radio_Search.Utils.MessageBroker.Interfaces;
using System.Collections.Concurrent;

namespace Radio_Search.Utils.MessageBroker.Factories.Azure
{

    /// <summary>
    /// Azure Writer Factory meant to get instances of Service Bus Writers.
    /// <note type="important">
    /// This factory is meant to be injected only as a Singleton
    /// </note>
    /// </summary>
    public class AzureWriterFactory : IMessageBrokerWriteFactory, IAsyncDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        public record WriterKey(string? ClientName, string DestinationName);
        private ConcurrentDictionary<WriterKey, IMessageBrokerWriter> _writers = new();

        public AzureWriterFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public IMessageBrokerWriter GetMessageBrokerWriter(string destinationName, string? clientName = null)
        {
            IMessageBrokerClient client;

            if (clientName == null)
                client = _serviceProvider.GetRequiredService<IMessageBrokerClient>();

            else
                client = _serviceProvider.GetRequiredKeyedService<IMessageBrokerClient>(clientName);

            var key = new WriterKey(clientName, destinationName);

            if (_writers.ContainsKey(key))
            {
                return _writers[key];
            }

            return _writers.GetOrAdd(key, _ => client.GetMessageWriter(destinationName));
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var writer in _writers.Values)
            {
                if (writer is IAsyncDisposable asyncDisposableSender)
                {
                    await asyncDisposableSender.DisposeAsync();
                }
                else if (writer is IDisposable disposableSender)
                {
                    disposableSender.Dispose();
                }
            }

            GC.SuppressFinalize(this);
        }
    }
}
