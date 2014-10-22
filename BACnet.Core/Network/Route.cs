using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Core.Datalink;

namespace BACnet.Core.Network
{
    public class Route
    {
        /// <summary>
        /// The network number of the route
        /// </summary>
        public ushort Network { get; private set; }

        /// <summary>
        /// The port id of the next hop port
        /// </summary>
        public byte PortId { get; private set; }

        /// <summary>
        /// The next hop mac address, or a broadcast address for a local port
        /// </summary>
        public Mac NextHop { get; private set; }

        /// <summary>
        /// Constructs a new Route instance
        /// </summary>
        /// <param name="network">The network number of the route</param>
        /// <param name="portId">The port id of the next hop port</param>
        /// <param name="nextHop">The next hop mac address, or a broadcast address for a local port</param>
        public Route(ushort network, byte portId, Mac nextHop)
        {
            this.Network = network;
            this.PortId = portId;
            this.NextHop = nextHop;
        }
    }
}
