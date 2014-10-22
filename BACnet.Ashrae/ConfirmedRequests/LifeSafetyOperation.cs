using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class LifeSafetyOperationRequest : IConfirmedRequest
    {
        /// <summary>
        /// The service choice for life safety operation requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.LifeSafetyOperation; } }
    }
}
