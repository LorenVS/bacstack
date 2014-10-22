using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.IP.Bvlc
{
    public enum FunctionCode : byte
    {
        Result = 0x00,
        WriteBroadcastDistributionTable = 0x01,
        ReadBroadcastDistributionTable = 0x02,
        ReadBroadcastDistributionTableAck = 0x03,
        ForwardedNpdu = 0x04,
        RegisterForeignDevice = 0x05,
        ReadForeignDeviceTable = 0x06,
        ReadForeignDeviceTableAck = 0x07,
        DeleteForeignDeviceTableEntry = 0x08,
        DistributeBroadcastToNetwork = 0x09,
        OriginalUnicastNpdu = 0x0A,
        OriginalBroadcastNpdu = 0x0B
    }
}
