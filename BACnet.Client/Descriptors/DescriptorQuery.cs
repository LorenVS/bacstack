using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BACnet.Client.Descriptors
{
    public class DescriptorQuery
    {
        /// <summary>
        /// The device instance to filter
        /// </summary>
        public uint? DeviceInstance { get; private set; }

        /// <summary>
        /// The object type to filter
        /// </summary>
        public ushort? ObjectType { get; private set; }

        /// <summary>
        /// The name regex to filter
        /// </summary>
        public string NameRegex { get; private set; }

        public DescriptorQuery(uint? deviceInstance = null, ushort? objectType = null, string nameRegex = null)
        {
            this.DeviceInstance = deviceInstance;
            this.ObjectType = objectType;
            this.NameRegex = nameRegex;
        }

        /// <summary>
        /// Determines whether this query matches an object info instance
        /// </summary>
        /// <param name="objectInfo">The object info instance</param>
        /// <returns>True if the query matches</returns>
        public bool Matches(ObjectInfo objectInfo)
        {
            if (DeviceInstance != null && DeviceInstance.Value != objectInfo.DeviceInstance)
                return false;
            else if (ObjectType != null && ObjectType.Value != objectInfo.ObjectIdentifier.Type)
                return false;
            else if (NameRegex != null && !Regex.IsMatch(objectInfo.Name, NameRegex, RegexOptions.IgnoreCase))
                return false;
            return true;
        }
    }
}
