using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class SubscribeCOVRequest : IConfirmedRequest
    {
        /// <summary>
        /// The service choice for subscribe cov requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.SubscribeCOV; } }
    }
}
