using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class UnconfirmedEventNotificationRequest : IUnconfirmedRequest
    {
        /// <summary>
        /// The service choice for unconfirmed event notification requests
        /// </summary>
        public UnconfirmedServiceChoice ServiceChoice { get { return UnconfirmedServiceChoice.UnconfirmedEventNotification; } }
    }
}
