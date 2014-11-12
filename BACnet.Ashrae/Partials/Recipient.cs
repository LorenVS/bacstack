using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class Recipient
    {
        /// <summary>
        /// Compares this recipient to another object
        /// </summary>
        /// <param name="obj">The object to compare to</param>
        /// <returns>True if the objects are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            var other = obj as Recipient;
            if (other == null)
                return false;
            else
                return this == other;
        }

        /// <summary>
        /// Retrieves a hash code for this recipient
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + this.Tag.GetHashCode();
                switch (this.Tag)
                {
                    case Tags.Address:
                        hash = hash * 31 + this.AsAddress.GetHashCode();
                        break;
                    case Tags.Device:
                        hash = hash * 31 + this.AsDevice.GetHashCode();
                        break;
                }
                return hash;
            }
        }

        /// <summary>
        /// Compares two recipients to determine if they are equal
        /// </summary>
        /// <param name="r1">The first recipient</param>
        /// <param name="r2">The second recipient</param>
        /// <returns>True if the recipients are equal, false otherwise</returns>
        public static bool operator==(Recipient r1, Recipient r2)
        {
            if (Object.ReferenceEquals(r1, r2))
                return true;
            else if (Object.ReferenceEquals(r1, null) || Object.ReferenceEquals(r2, null))
                return false;
            else if (r1.Tag != r2.Tag)
                return false;
            else
            {
                switch (r1.Tag)
                {
                    case Tags.Address:
                        return r1.AsAddress == r2.AsAddress;
                    case Tags.Device:
                        return r1.AsDevice == r2.AsDevice;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Compares two recipients to determine if they are inequal
        /// </summary>
        /// <param name="r1">The first recipient</param>
        /// <param name="r2">The second recipient</param>
        /// <returns>True if the recipients are not equal, false otherwise</returns>
        public static bool operator!=(Recipient r1, Recipient r2)
        {
            return !(r1 == r2);
        }
    }
}
