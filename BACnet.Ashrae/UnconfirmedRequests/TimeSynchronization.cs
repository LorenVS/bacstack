using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class TimeSynchronizationRequest : IUnconfirmedRequest
    {
        /// <summary>
        /// The service choice for time synchronization requests
        /// </summary>
        public UnconfirmedServiceChoice ServiceChoice { get { return UnconfirmedServiceChoice.TimeSynchronization; } }
    }
}
