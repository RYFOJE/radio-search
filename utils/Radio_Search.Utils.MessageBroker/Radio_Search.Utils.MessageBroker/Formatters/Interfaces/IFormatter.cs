using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Radio_Search.Utils.MessageBroker.Formatters.Interfaces
{
    internal interface IFormatter
    {

        /// <summary>
        /// Converts a serialized message
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal byte[] Serialize(object? value);

        /// <summary>
        /// Converts the specified object to an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type to which the object should be deserialized.</typeparam>
        /// <param name="value">The object to deserialize. Must be compatible with the target type <typeparamref name="T"/>.</param>
        /// <returns>An instance of type <typeparamref name="T"/> representing the deserialized object.</returns>
        internal T Deserialize<T>(byte[] value);

    }
}