using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class VtOpenRequest : IConfirmedRequest
    {
        /// <summary>
        /// The confirmed service choice for vt open requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.VtOpen; } }
    }

    public partial class VtOpenAck : IComplexAck
    {
        /// <summary>
        /// The confirmed service choice for vt open acks
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.VtOpen; } }
    }
}
