using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class ReadPropertyConditionalRequest : IConfirmedRequest
    {
        /// <summary>
        /// The confirmed service choice for read property conditional requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.ReadPropertyConditional; } }
    }

    public partial class ReadPropertyConditionalAck : IComplexAck
    {
        /// <summary>
        /// The confirmed service choice for read property conditional acks
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.ReadPropertyConditional; } }
    }
}
