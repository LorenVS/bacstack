using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Network.Messages
{
    public enum MessageType : byte
    {
        WhoIsRouterToNetwork = 0x00,
        IAmRouterToNetwork = 0x01,
        ICouldBeRouterToNetwork = 0x02,
        RejectMessageToNetwork = 0x03,
        RouterBusyToNetwork = 0x04,
        RouterAvailableToNetwork = 0x05,
        InitializeRoutingTable = 0x06,
        InitializeRoutingTableAck = 0x07,
        EstablishConnectionToNetwork = 0x08,
        DisconnectConnectionToNetwork = 0x09
    }
}
