using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Client;
using BACnet.Client.Descriptors;

namespace BACnet.Explorer.Core.Models
{
    public class ObjectRow
    {
        /// <summary>
        /// The underlying object info instance
        /// </summary>
        public ObjectInfo Info { get; private set; }

        public uint DeviceInstance
        {
            get { return Info.DeviceInstance; }
        }

        public string Id
        {
            get
            {
                var type = (ObjectType)Info.ObjectIdentifier.Type;
                return type + "." + Info.ObjectIdentifier.Instance;
            }
        }

        public string Name
        {
            get { return Info.Name; }
        }

        public ObjectRow(ObjectInfo info)
        {
            this.Info = info;
        }
    }
}
