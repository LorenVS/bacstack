using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class ReadPropertyRequest : IConfirmedRequest
    {
        /// <summary>
        /// The confirmed service choice for read property requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.ReadProperty; } }
    }

    public partial class ReadPropertyAck : IComplexAck
    {
        /// <summary>
        /// The confirmed service choice for read property acks
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.ReadProperty; } }
    }
}
