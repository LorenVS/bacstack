using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class NetworkAddress
    {
        /// <summary>
        /// Compares this network address to
        /// another object
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True if the objects are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            var other = obj as NetworkAddress;
            if (other == null)
                return false;
            else
                return this == other;
        }

        /// <summary>
        /// Retrieves a hash code for this network address
        /// </summary>
        /// <returns>The hash code instance</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + this.NetworkNumber.GetHashCode();
                for (int i = 0; i < this.MacAddress.Length; i++)
                    hash = (hash * 31) ^ this.MacAddress[i];
                return hash;
            }
        }

        /// <summary>
        /// Compares two network addresses to determine
        /// if they are equal
        /// </summary>
        /// <param name="a1">The first address</param>
        /// <param name="a2">The second address</param>
        /// <returns>True if the addresses are equal, false otherwise</returns>
        public static bool operator==(NetworkAddress a1, NetworkAddress a2)
        {
            if (Object.ReferenceEquals(a1, a2))
                return true;
            else if (Object.ReferenceEquals(a1, null) || Object.ReferenceEquals(a2, null))
                return false;
            else if (a1.NetworkNumber != a2.NetworkNumber)
                return false;
            else if (a1.MacAddress.Length != a2.MacAddress.Length)
                return false;
            else
            {
                for(int i = 0; i < a1.MacAddress.Length; i++)
                    if(a1.MacAddress[i] != a2.MacAddress[i])
                        return false;
                return true;
            }
        }

        /// <summary>
        /// Compares two network addresses to determine 
        /// if they are not equal
        /// </summary>
        /// <param name="a1">The first address</param>
        /// <param name="a2">The second address</param>
        /// <returns>True if the addresses are not equal, false otherwise</returns>
        public static bool operator!=(NetworkAddress a1, NetworkAddress a2)
        {
            return !(a1 == a2);
        }
    }
}
