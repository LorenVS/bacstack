using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public interface IUnconfirmedRequest
    {
        UnconfirmedServiceChoice ServiceChoice { get; }
    }
}
