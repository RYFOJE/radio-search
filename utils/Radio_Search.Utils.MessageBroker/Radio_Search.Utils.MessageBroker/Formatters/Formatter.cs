using Radio_Search.Utils.MessageBroker.Enums;
using Radio_Search.Utils.MessageBroker.Formatters.Interfaces;

namespace Radio_Search.Utils.MessageBroker.Formatters
{

    /// <summary>
    /// Provides functionality to retrieve formatter instances based on specified format types.
    /// </summary>
    /// <remarks>This class supports retrieving formatters for predefined format types, such as JSON and
    /// Protobuf. It is intended for internal use and ensures that the appropriate formatter is returned based on the
    /// specified <see cref="FormatTypes"/> value.</remarks>
    internal static class Formatter
    {
        private static readonly JSONFormatter _jsonFormatter = new();
        private static readonly ProtobufFormatter _protobufFormatter = new();

        /// <summary>
        /// Retrieves an <see cref="IFormatter"/> instance based on the specified format type.
        /// </summary>
        /// <param name="formatterType">The type of formatter to retrieve. Must be a valid <see cref="FormatTypes"/> value.</param>
        /// <returns>An <see cref="IFormatter"/> instance corresponding to the specified <paramref name="formatterType"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="formatterType"/> is not a supported <see cref="FormatTypes"/> value.</exception>
        internal static IFormatter GetFormatter(FormatTypes formatterType)
        {

            return formatterType switch
            {
                FormatTypes.JSON => _jsonFormatter,
                FormatTypes.Protobuf => _protobufFormatter,
                _ => throw new ArgumentOutOfRangeException(nameof(formatterType), $"Formatter of type {formatterType} is not supported")
            };

        }

    }
}
