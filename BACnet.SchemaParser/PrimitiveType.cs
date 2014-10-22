using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.SchemaParser
{
    public enum PrimitiveType
    {
        Null,
        Boolean,
        Unsigned8,
        Unsigned16,
        Unsigned32,
        Unsigned64,
        Signed8,
        Signed16,
        Signed32,
        Signed64,
        Float32,
        Float64,
        OctetString,
        CharString,
        BitString8,
        BitString24,
        BitString56,
        Enumerated,
        Date,
        Time,
        ObjectId,
        Generic
    }
}
