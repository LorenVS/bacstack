using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class DeviceObjectPropertyReference
    {
        /// <summary>
        /// Returns a string representation of the object
        /// </summary>
        /// <returns>The string representation</returns>
        public override string ToString()
        {
            return
                DeviceIdentifier.ToString() +
                (DeviceIdentifier.HasValue ? "." : string.Empty) +
                (ObjectType)ObjectIdentifier.Type +
                ObjectIdentifier.Instance +
                "." +
                PropertyIdentifier +
                (PropertyArrayIndex.HasValue ? "[" : string.Empty) +
                PropertyArrayIndex.ToString() +
                (PropertyArrayIndex.HasValue ? "]" : string.Empty);
        }
    }
}
