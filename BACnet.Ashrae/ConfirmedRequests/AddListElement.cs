using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class AddListElementRequest : IConfirmedRequest
    {
        /// <summary>
        /// The service choice for add list element requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.AddListElement; } }
    }
}
