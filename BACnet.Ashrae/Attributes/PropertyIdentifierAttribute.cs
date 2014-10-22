using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Ashrae
{
    public class PropertyIdAttribute : PropertyAttribute
    {
        public PropertyIdAttribute(PropertyIdentifier propertyIdentifier)
            : base((ushort)propertyIdentifier) { }
    }
}
