using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class ReinitializeDeviceRequest : IConfirmedRequest
    {
        /// <summary>
        /// The service choice for reinitialize device requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.ReinitializeDevice; } }
    }
}
