using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class ConfirmedEventNotificationRequest : IConfirmedRequest
    {
        /// <summary>
        /// The service choice for confirmed event notification requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.ConfirmedEventNotification; } }
    }
}
