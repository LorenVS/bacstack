using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public interface IComplexAck
    {
        /// <summary>
        /// The service choice of the complex ack
        /// </summary>
        ConfirmedServiceChoice ServiceChoice { get; }
    }
}
