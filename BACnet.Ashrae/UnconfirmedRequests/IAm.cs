using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class IAmRequest : IUnconfirmedRequest
    {
        /// <summary>
        /// The service choice for i am requests
        /// </summary>
        public UnconfirmedServiceChoice ServiceChoice { get { return UnconfirmedServiceChoice.IAm; } }
    }
}
