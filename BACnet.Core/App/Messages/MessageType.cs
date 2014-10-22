using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.App.Messages
{
    public enum MessageType : byte
    {
        ConfirmedRequest = 0x00,
        UnconfirmedRequest = 0x01,
        SimpleAck = 0x02,
        ComplexAck = 0x03,
        SegmentAck = 0x04,
        Error = 0x05,
        Reject = 0x06,
        Abort = 0x07
    }
}
