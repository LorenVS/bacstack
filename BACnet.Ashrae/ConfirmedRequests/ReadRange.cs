using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class ReadRangeRequest : IConfirmedRequest
    {
        /// <summary>
        /// The confirmed service choice for read range requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.ReadRange; } }
    }

    public partial class ReadRangeAck : IComplexAck
    {
        /// <summary>
        /// The confirmed service choice for read range acks
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.ReadRange; } }
    }
}
