using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Client
{
    public struct GlobalObjectId : IEquatable<GlobalObjectId>
    {
        /// <summary>
        /// The device instance of the object
        /// </summary>
        public uint DeviceInstance { get; private set; }

        /// <summary>
        /// The object identifier of the object
        /// </summary>
        public ObjectId ObjectIdentifier { get; private set; }

        /// <summary>
        /// Constructs a new global object id instance
        /// </summary>
        /// <param name="deviceInstance">The device instance of the object</param>
        /// <param name="objectIdentifier">The object identifier of the object</param>
        public GlobalObjectId(uint deviceInstance, ObjectId objectIdentifier) : this()
        {
            this.DeviceInstance = deviceInstance;
            this.ObjectIdentifier = objectIdentifier;
        }

        /// <summary>
        /// Gets the hash code for an object
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            int ret = (int)(DeviceInstance.GetHashCode() & 0xFFFF0000);
            ret |= ObjectIdentifier.GetHashCode();
            return ret;
        }

        /// <summary>
        /// Determines whether this global object id
        /// is equal to another object
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns>True if the two objects are equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else if (!(obj is GlobalObjectId))
                return false;

            GlobalObjectId other = (GlobalObjectId)obj;
            return this == other;
        }


        /// <summary>
        /// Determines whether this global object id is
        /// the same as another
        /// </summary>
        /// <param name="other">The object to compare</param>
        /// <returns>True if the object ids are identical, false otherwise</returns>
        public bool Equals(GlobalObjectId other)
        {
            return this == other;
        }

        /// <summary>
        /// Compares two global object id instances to determine
        /// if they are equal
        /// </summary>
        /// <param name="o1">The first global object id</param>
        /// <param name="o2">The second global object id</param>
        /// <returns>True if the ids are equal, false otherwise</returns>
        public static bool operator==(GlobalObjectId o1, GlobalObjectId o2)
        {
            return o1.DeviceInstance == o2.DeviceInstance
                && o1.ObjectIdentifier == o2.ObjectIdentifier;
        }

        /// <summary>
        /// Compares two global object id instances to determine
        /// if they are inequal
        /// </summary>
        /// <param name="o1">The first global object id</param>
        /// <param name="o2">The second global object id</param>
        /// <returns>True if the ids are inequal, false otherwise</returns>
        public static bool operator!=(GlobalObjectId o1, GlobalObjectId o2)
        {
            return !(o1 == o2);
        }


    }
}
