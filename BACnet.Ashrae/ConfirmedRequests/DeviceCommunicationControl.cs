using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class DeviceCommunicationControlRequest : IConfirmedRequest
    {
        /// <summary>
        /// The service choice for device communication control requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.DeviceCommunicationControl; } }
    }
}
