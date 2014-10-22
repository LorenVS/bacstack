using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class ConfirmedCOVNotificationRequest : IConfirmedRequest
    {
        /// <summary>
        /// The confirmed service choice for confirmed cov notification requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.ConfirmedCOVNotification; } }
    }
}
