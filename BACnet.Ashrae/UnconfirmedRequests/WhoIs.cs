using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class WhoIsRequest : IUnconfirmedRequest
    {
        /// <summary>
        /// The service choice for who is requests
        /// </summary>
        public UnconfirmedServiceChoice ServiceChoice { get { return UnconfirmedServiceChoice.WhoIs; } }
    }
}
