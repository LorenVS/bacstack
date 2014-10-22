using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BACnet.Core.Datalink;

namespace BACnet.IP
{
    internal static class IPUtils
    {
        /// <summary>
        /// Converts a BACnet mac address
        /// to an ip endpoint
        /// </summary>
        /// <param name="mac">The mac address to convert</param>
        /// <returns>The converted ip endpoint</returns>
        internal static IPEndPoint MacToIPEndPoint(Mac mac)
        {
            if (mac.Length != 6)
                throw new Exception("Only a 6-byte long mac address can be converted to an ip endpoint");

            ushort port = mac[4];
            port <<= 8;
            port |= mac[5];

            byte[] addrBytes = new byte[] { mac[0], mac[1], mac[2], mac[3] };
            IPAddress addr = new IPAddress(addrBytes);
            return new IPEndPoint(addr, port);
        }

        /// <summary>
        /// Converts an IP endpoint to a BACnet
        /// mac address
        /// </summary>
        /// <param name="ep">The endpoint to convert</param>
        /// <returns>The converted mac address</returns>
        internal static Mac IPEndPointToMac(IPEndPoint ep)
        {
            IPAddress addr = ep.Address;
            if (addr.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
                throw new Exception("Only IPv4 addresses can be converted to BACnet mac addresses");

            byte[] addrBytes = addr.GetAddressBytes();
            ushort port = (ushort)ep.Port;
            byte[] bytes = new byte[] { addrBytes[0], addrBytes[1], addrBytes[2], addrBytes[3], (byte)(port >> 8), (byte)port };
            return new Mac(bytes, false);
        }
    }
}
