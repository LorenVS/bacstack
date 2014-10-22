using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Tagging
{
    public enum ApplicationTag : byte
    {
        Null = 0x00,
        Boolean = 0x01,
        Unsigned = 0x02,
        Signed = 0x03,
        Float32 = 0x04,
        Float64 = 0x05,
        OctetString = 0x06,
        CharString = 0x07,
        BitString = 0x08,
        Enumerated = 0x09,
        Date = 0x0A,
        Time = 0x0B,
        ObjectId = 0x0C
    }
}
