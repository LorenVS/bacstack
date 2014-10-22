using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.SchemaParser
{
    public enum DefinitionType
    {
        Enumeration,
        BitString,
        Sequence,
        Choice,
        Option,
        Array,
        Name,
        Primitive
    }
}
