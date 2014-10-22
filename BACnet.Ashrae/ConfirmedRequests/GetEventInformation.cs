using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class GetEventInformationRequest : IConfirmedRequest
    {
        /// <summary>
        /// The service choice for get event information requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.GetEventInformation; } }
    }

    public partial class GetEventInformationAck : IComplexAck
    {
        /// <summary>
        /// The service choice for get event information acks
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.GetEventInformation; } }
    }
}
