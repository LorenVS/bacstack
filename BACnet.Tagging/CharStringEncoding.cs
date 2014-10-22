using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Tagging
{
    internal enum CharStringEncoding : byte
    {
        ANSI = 0x00,
        DBCS = 0x01,
        JIS_C_6226 = 0x02,
        UCS4 = 0x03
    }
}
