using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;

namespace BACnet.Core.Exceptions
{
    public class RejectException : BACnetException
    {
        /// <summary>
        /// The reject reason
        /// </summary>
        public RejectReason Reason { get; private set; }

        /// <summary>
        /// Constructs a new reject exception instance
        /// </summary>
        /// <param name="reason">The reject reason</param>
        public RejectException(RejectReason reason)
        {
            this.Reason = reason;
        }
    }
}
