using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Core.Datalink;

namespace BACnet.Core.Network
{
    public class Address
    {
        /// <summary>
        /// Shared global broadcast instance
        /// </summary>
        public static readonly Address GlobalBroadcast = new Address(65535, Mac.Broadcast);

        /// <summary>
        /// Shared address instance that represents
        /// a broadcast on all directly connected networks,
        /// without being a global broadcast
        /// </summary>
        public static readonly Address DirectlyConnectedBroadcast = new Address(65535, new Mac(new byte[] { 255 }));

        /// <summary>
        /// The network number of the address
        /// </summary>
        public ushort Network { get; private set; }

        /// <summary>
        /// The mac address of the address
        /// </summary>
        public Mac Mac { get; private set; }

        /// <summary>
        /// Constructs a new address instance
        /// </summary>
        /// <param name="network">The network number of the address</param>
        /// <param name="mac">The mac address of the address</param>
        public Address(ushort network, Mac mac)
        {
            this.Network = network;
            this.Mac = mac;
        }

        /// <summary>
        /// Constructs a new address based on an Ashrae NetworkAddress instance
        /// </summary>
        /// <param name="addr">The network address instance</param>
        public Address(NetworkAddress addr)
        {
            this.Network = addr.NetworkNumber;
            this.Mac = new Mac(addr.MacAddress);
        }

        /// <summary>
        /// Determines whether this address is a global broadcast address
        /// </summary>
        /// <returns>True if the addres is a global broadcast, false otherwise</returns>
        public bool IsGlobalBroadcast()
        {
            return Network == 65535 && Mac.Length == 0;
        }

        /// <summary>
        /// Determines whether this address is a directly connected broadcast address
        /// </summary>
        /// <returns>True if the address is a directly connected broadcast address, false otherwise</returns>
        public bool IsDirectedlyConnectedBroadcast()
        {
            return Network == 65535 && Mac.Length == 1;
        }

        /// <summary>
        /// Converts this address to an ASHRAE NetworkAddress
        /// </summary>
        /// <returns>The network address instance</returns>
        public NetworkAddress ToNetworkAddress()
        {
            return new NetworkAddress(
                this.Network,
                this.Mac.ToBytes());
        }

        /// <summary>
        /// Compares two addresses to determine
        /// if they are equal
        /// </summary>
        /// <param name="a1">The first address</param>
        /// <param name="a2">The second address</param>
        /// <returns>True if the addresses are equal, false otherwise</returns>
        public static bool operator==(Address a1, Address a2)
        {
            if ((object)a1 == null && (object)a2 == null)
                return true;
            else if ((object)a1 == null)
                return false;
            else if ((object)a2 == null)
                return false;
            else
            {
                return a1.Network == a2.Network &&
                    a1.Mac == a2.Mac;
            }
        }

        /// <summary>
        /// Compares two addresses to determine
        /// if they are inequal
        /// </summary>
        /// <param name="a1">The first address</param>
        /// <param name="a2">The second address</param>
        /// <returns>True if the addresses are inequal, false otherwise</returns>
        public static bool operator !=(Address a1, Address a2)
        {
            return !(a1 == a2);
        }

        /// <summary>
        /// Checks to see if this object is equal to another
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True if the objects are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Address))
                return false;
            var a2 = (Address)obj;
            return this == a2;
        }

        /// <summary>
        /// Retrieves a hashcode suitable for this object
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            int code = (this.Network << 16);
            code |= Mac.GetHashCode() & 0x0000FFFF;
            return code;
        }

    }
}
