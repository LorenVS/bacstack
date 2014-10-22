using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class UnconfirmedTextMessageRequest : IUnconfirmedRequest
    {
        /// <summary>
        /// The service choice for unconfirmed text message requests
        /// </summary>
        public UnconfirmedServiceChoice ServiceChoice { get { return UnconfirmedServiceChoice.UnconfirmedTextMessage; } }
    }
}
