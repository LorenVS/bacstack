using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class UtcTimeSynchronizationRequest : IUnconfirmedRequest
    {
        /// <summary>
        /// The service choice for utc time synchronization requests
        /// </summary>
        public UnconfirmedServiceChoice ServiceChoice { get { return UnconfirmedServiceChoice.UtcTimeSynchronization; } }
    }
}
