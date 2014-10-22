using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public interface IConfirmedRequest
    {
        /// <summary>
        /// The service choice of the confirmed request
        /// </summary>
        ConfirmedServiceChoice ServiceChoice { get; }
    }
}
