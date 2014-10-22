using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class VtCloseRequest : IConfirmedRequest
    {
        /// <summary>
        /// The confirmed service choice for vt close requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.VtClose; } }
    }

}
