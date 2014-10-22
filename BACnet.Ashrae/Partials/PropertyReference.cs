using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Ashrae
{
    public partial class PropertyReference
    {
        public PropertyReference(PropertyIdentifier propertyId) : this(propertyId, Option<uint>.None)
        { }

    }
}
