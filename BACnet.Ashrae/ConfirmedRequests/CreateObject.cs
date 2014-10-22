using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class CreateObjectRequest : IConfirmedRequest
    {
        /// <summary>
        /// The service choice for create object requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.CreateObject; } }
    }

    public partial class CreateObjectAck : IComplexAck
    {
        /// <summary>
        /// The service choice for create object ack
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.CreateObject; } }
    }
}
