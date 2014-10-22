using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class ReadPropertyMultipleRequest : IConfirmedRequest
    {
        /// <summary>
        /// The confirmed service choice for read property multiple requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.ReadPropertyMultiple; } }
    }

    public partial class ReadPropertyMultipleAck : IComplexAck
    {
        /// <summary>
        /// The confirmed service choice for read property multiple acks
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.ReadPropertyMultiple; } }
    }
}
