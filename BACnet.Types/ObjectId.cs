using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public struct ObjectId : IEquatable<ObjectId>
    {
        /// <summary>
        /// The maximum instance value, used to identify
        /// the local device
        /// </summary>
        public const uint MaxInstance = 4194303;

        /// <summary>
        /// The object type
        /// </summary>
        public ushort Type { get; private set; }

        /// <summary>
        /// The object instance
        /// </summary>
        public uint Instance { get; private set; }

        /// <summary>
        /// Constructs a new ObjectId value
        /// </summary>
        /// <param name="type">The object type</param>
        /// <param name="instance">The object instance</param>
        public ObjectId(ushort  type, uint instance) : this()
        {
            this.Type = type;
            this.Instance = instance;
        }

        public override string ToString()
        {
            return this.Type + "." + this.Instance;
        }

        /// <summary>
        /// Retrieves a hash code for this object id
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            int ret = Type << 16;
            ret |= Instance.GetHashCode() & 0x0000FFFF;
            return ret;
        }

        /// <summary>
        /// Determines whether this object id is identical
        /// to another
        /// </summary>
        /// <param name="obj">The object to compare with</param>
        /// <returns>True if the objects are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else if (!(obj is ObjectId))
                return false;

            ObjectId other = (ObjectId)obj;
            return this == other;
        }

        /// <summary>
        /// Determines whether this object id is identical
        /// to another
        /// </summary>
        /// <param name="other">The other object id to compare with</param>
        /// <returns>True if the object ids are equal, false otherwise</returns>
        public bool Equals(ObjectId other)
        {
            return this == other;
        }

        /// <summary>
        /// Compares two object id instances to determine
        /// if they are equal
        /// </summary>
        /// <param name="o1">The first object id</param>
        /// <param name="o2">The second object id</param>
        /// <returns>True if the ids are equal, false otherwise</returns>
        public static bool operator==(ObjectId o1, ObjectId o2)
        {
            return o1.Type == o2.Type
                && o1.Instance == o2.Instance;
        }

        /// <summary>
        /// Compares two object id instances to determine
        /// if they are inequal
        /// </summary>
        /// <param name="o1">The first object id</param>
        /// <param name="o2">The second object id</param>
        /// <returns>True if the ids are inequal, false otherwise</returns>
        public static bool operator!=(ObjectId o1, ObjectId o2)
        {
            return !(o1 == o2);
        }

    }
}
