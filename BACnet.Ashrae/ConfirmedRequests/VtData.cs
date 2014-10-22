using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class VtDataRequest : IConfirmedRequest
    {
        /// <summary>
        /// The confirmed service choice for vt data requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.VtData; } }
    }

    public partial class VtDataAck : IComplexAck
    {
        /// <summary>
        /// The confirmed service choice for vt data acks
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.VtData; } }
    }
}
