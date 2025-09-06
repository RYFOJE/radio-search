using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio_Search.Utils.MessageBroker.Exceptions
{
    public class InvalidMessageBrokerClientException : Exception
    {
        public InvalidMessageBrokerClientException() { }

        public InvalidMessageBrokerClientException(string message)
            : base(message) { }

        public InvalidMessageBrokerClientException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
