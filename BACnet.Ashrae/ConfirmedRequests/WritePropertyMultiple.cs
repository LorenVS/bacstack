using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class WritePropertyMultipleRequest : IConfirmedRequest
    {
        /// <summary>
        /// The service choice for write property multiple requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.WritePropertyMultiple; } }
    }
}
