using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class ConfirmedTextMessageRequest : IConfirmedRequest
    {
        /// <summary>
        /// The service choice for confirmed text message requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.ConfirmedTextMessage; } }
    }
}
