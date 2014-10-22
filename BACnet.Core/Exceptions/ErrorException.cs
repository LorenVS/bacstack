using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;

namespace BACnet.Core.Exceptions
{
    public class ErrorException : BACnetException
    {
        /// <summary>
        /// The error
        /// </summary>
        public ServiceError Error { get; private set; }

        /// <summary>
        /// Constructs a new error exception instance
        /// </summary>
        /// <param name="error">The error</param>
        public ErrorException(ServiceError error)
        {
            this.Error = error;
        }
    }
}
