using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class ConfirmedPrivateTransferRequest : IConfirmedRequest
    {
        /// <summary>
        /// The confirmed service choice for confirmed private transfer requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.ConfirmedPrivateTransfer; } }
    }

    public partial class ConfirmedPrivateTransferAck : IComplexAck
    {
        /// <summary>
        /// The confirmed service choice for confirmed private transfer acks
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.ConfirmedPrivateTransfer; } }
    }
}
