using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class UnconfirmedCOVNotificationRequest : IUnconfirmedRequest
    {
        /// <summary>
        /// The service choice for unconfirmed cov notification requests
        /// </summary>
        public UnconfirmedServiceChoice ServiceChoice { get { return UnconfirmedServiceChoice.UnconfirmedCOVNotification; } }
    }
}
