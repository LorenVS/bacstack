using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class IHaveRequest : IUnconfirmedRequest
    {
        /// <summary>
        /// The service choice for i have requests
        /// </summary>
        public UnconfirmedServiceChoice ServiceChoice { get { return UnconfirmedServiceChoice.IHave; } }
    }
}
