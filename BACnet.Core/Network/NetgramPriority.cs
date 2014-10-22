using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Network
{
    public enum NetgramPriority : byte
    {
        LifeSafetyMessage = 0x03,
        CriticalEquipmentMessage = 0x02,
        UrgentMessage = 0x01,
        Normal = 0x00
    }
}
