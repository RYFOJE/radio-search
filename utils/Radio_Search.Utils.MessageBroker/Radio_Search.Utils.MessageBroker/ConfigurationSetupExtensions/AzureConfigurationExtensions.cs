using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Radio_Search.Utils.MessageBroker.Configurations;
using Radio_Search.Utils.MessageBroker.Factories.Azure;
using Radio_Search.Utils.MessageBroker.Factories.Interfaces;
using Radio_Search.Utils.MessageBroker.Implementations;
using Radio_Search.Utils.MessageBroker.Implementations.Azure;
using Radio_Search.Utils.MessageBroker.Interfaces;

namespace Radio_Search.Utils.MessageBroker.ConfigurationSetupExtensions
{
    public static class AzureConfigurationExtensions
    {
        /// <summary>
        /// Adds an Azure Service Bus client to the service collection with the specified configuration.
        /// </summary>
        /// <remarks>This method registers the Azure Service Bus client as a singleton in the dependency
        /// injection container. The client can be retrieved later using the specified <paramref name="clientName"/> or
        /// the default name.</remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the client will be added.</param>
        /// <param name="config">The configuration settings for the Azure Service Bus client. This cannot be <see langword="null"/>.</param>
        /// <param name="clientName">An optional name for the client. If <see langword="null"/>, no name will be used and it will not be a keyed service.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AddAzureServiceBusClient(this IServiceCollection services, AzureServiceBusClientConfig config, string? clientName = null)
        {
            if (string.IsNullOrEmpty(clientName))
            {
                services.AddSingleton<IMessageBrokerClient>(
                    new AzureMessageBrokerClient(config));
            }
            else
            {
                services.AddKeyedSingleton<IMessageBrokerClient>(clientName,
                    (sp, key) => new AzureMessageBrokerClient(config));
            }

            return services;
        }

        /// <summary>
        /// Adds an <see cref="AzureWriterFactory"/> implementation of <see cref="IMessageBrokerWriteFactory"/> to the service
        /// collection.
        /// </summary>
        /// <remarks>This method registers the <see cref="AzureWriterFactory"/> as a singleton service for
        /// the <see cref="IMessageBrokerWriteFactory"/> interface.</remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> to which the factory will be added.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> to allow for method chaining.</returns>
        public static IServiceCollection AddAzureWriterFactory(this IServiceCollection services)
        {
            services.TryAddSingleton<IMessageBrokerWriteFactory, AzureWriterFactory>();

            return services;
        }

    }
}
