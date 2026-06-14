using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio_Search.Utils.BlobStorage.Exceptions
{
    public class AcquireLeaseException : Exception
    {
        public AcquireLeaseException(Exception innerException, string message) : base(message, innerException) { }

    }
}