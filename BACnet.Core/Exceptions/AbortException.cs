using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;

namespace BACnet.Core.Exceptions
{
    public class AbortException : BACnetException
    {
        /// <summary>
        /// The abort reason
        /// </summary>
        public AbortReason Reason { get; private set; }

        /// <summary>
        /// Constructs a new abort exception instance
        /// </summary>
        /// <param name="reason">The abort reason</param>
        public AbortException(AbortReason reason)
        {
            this.Reason = reason;
        }
    }
}
