using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Datalink
{
    public struct Mac
    {
        /// <summary>
        /// Shared instance of the broadcast mac
        /// </summary>
        public static readonly Mac Broadcast = new Mac();

        /// <summary>
        /// The bytes of the mac address
        /// </summary>
        private byte[] bytes;

        /// <summary>
        /// The byte length of the mac address
        /// </summary>
        public int Length { get { return bytes == null ? 0 : bytes.Length; } }

        /// <summary>
        /// Retrieves the index-th byte of the mac address
        /// </summary>
        /// <param name="index">The index of the byte to retrieve</param>
        /// <returns>The byte at the index</returns>
        public byte this[int index]
        {
            get
            {
                return bytes[index];
            }
        }

        /// <summary>
        /// Constructs a new mac address instance
        /// </summary>
        /// <param name="bytes">The bytes of the mac address</param>
        /// <param name="copy">True if the bytes must be copied to a new array, false otherwise</param>
        public Mac(byte[] bytes, bool copy = true)
        {
            if(copy)
            {
                if (bytes == null || bytes.Length == 0)
                    this.bytes = null;
                else
                {
                    this.bytes = new byte[bytes.Length];
                    Buffer.BlockCopy(bytes, 0, this.bytes, 0, bytes.Length);
                }
            }
            else
            {
                this.bytes = bytes;
            }
        }

        /// <summary>
        /// Constructs a new mac address instance
        /// by copying bytes from a buffer
        /// </summary>
        /// <param name="bytes">The buffer containing the bytes to copy</param>
        /// <param name="offset">The offset of the mac address bytes</param>
        /// <param name="length">The length of the mac address bytes</param>
        public Mac(byte[] bytes, int offset, int length)
        {
            if (length == 0)
                this.bytes = null;
            else
            {
                this.bytes = new byte[length];
                Buffer.BlockCopy(bytes, offset, this.bytes, 0, length);
            }
        }

        /// <summary>
        /// Checks to see if this mac address contains a broadcast address
        /// </summary>
        /// <returns>True if the mac address is a broadcast address, false otherwise</returns>
        public bool IsBroadcast()
        {
            return this.bytes == null;
        }

        /// <summary>
        /// Converts this mac address to a byte array
        /// </summary>
        /// <returns>The mac address bytes</returns>
        public byte[] ToBytes()
        {
            byte[] ret = new byte[this.bytes.Length];
            Array.Copy(this.bytes, ret, this.bytes.Length);
            return ret;
        }

        /// <summary>
        /// Compares two mac addresses for equality
        /// </summary>
        /// <param name="m1">The first mac address</param>
        /// <param name="m2">The second mac address</param>
        /// <returns>True if the addresses are equal, false otherwise</returns>
        public static bool operator==(Mac m1, Mac m2)
        {
            if (m1.bytes == m2.bytes)
                return true;
            else if (m1.bytes == null || m2.bytes == null)
                return false;
            else if (m1.bytes.Length != m2.bytes.Length)
                return false;
            else
            {
                for(int i = 0; i < m1.bytes.Length; i++)
                {
                    if (m1.bytes[i] != m2.bytes[i])
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Compares two mac addresses for inequality
        /// </summary>
        /// <param name="m1">The first mac address</param>
        /// <param name="m2">The second mac address</param>
        /// <returns>True if the addresses are inequal, false otherwise</returns>
        public static bool operator !=(Mac m1, Mac m2)
        {
            return !(m1 == m2);
        }

        /// <summary>
        /// Tests to see if this object equals another
        /// </summary>
        /// <param name="obj">The object to compare this object with</param>
        /// <returns>True if the objects are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else if(!(obj is Mac))
                return false;
            else
            {
                Mac m2 = (Mac)obj;
                return this == m2;
            }
        }

        /// <summary>
        /// Computes a hash code for the mac address
        /// </summary>
        /// <returns>The hash code for the mac address</returns>
        public override int GetHashCode()
        {
            if (bytes == null)
                return 0;

            unchecked
            {
                var result = 0;
                foreach (byte b in bytes)
                    result = (result * 31) ^ b;
                return result;
            }
        }
    }
}
