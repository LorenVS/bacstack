using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BACnet.Ashrae.Objects;
using BACnet.Types;

namespace BACnet.Client.Descriptors
{
    public class DeviceInfo : ObjectInfo
    {
        /// <summary>
        /// The database revision of the device
        /// </summary>
        [JsonProperty("rev")]
        public byte? DatabaseRevision
        {
            get { return _databaseRevision; }
            set { changeProperty(ref _databaseRevision, value, "DatabaseRevision"); }
        }
        private byte? _databaseRevision;

        /// <summary>
        /// Constructs a new device info instance
        /// </summary>
        /// <param name="vendorId">The vendor id of the device</param>
        /// <param name="deviceInstance">The device instance</param>
        /// <param name="objectIdentifier">The object identifier of the device</param>
        public DeviceInfo(ushort vendorId, uint deviceInstance, ObjectId objectIdentifier)
            : base(vendorId, deviceInstance, objectIdentifier)
        { }

        /// <summary>
        /// Copies this object's properties from another
        /// </summary>
        /// <param name="other">The object to copy from</param>
        public override void CopyFrom(object other)
        {
            var di = other as DeviceInfo;
            if(di != null)
            {
                base.CopyFrom(di);
                this.DatabaseRevision = di.DatabaseRevision;
            }
        }

        /// <summary>
        /// Creates a clone of this descriptor
        /// </summary>
        /// <returns>The cloned descriptor</returns>
        public override object Clone()
        {
            var clone = new DeviceInfo(VendorId, DeviceInstance, ObjectIdentifier);
            clone.CopyFrom(this);
            return clone;
        }

        /// <summary>
        /// Syncs updates from another instance to this one
        /// </summary>
        /// <param name="other">The instance to sync from</param>
        /// <param name="name">The name of the property to sync</param>
        protected override void syncFrom(object other, string name)
        {
            var di = other as DeviceInfo;
            if(di != null)
            {
                switch (name)
                {
                    case "DatabaseRevision":
                        this.DatabaseRevision = di.DatabaseRevision;
                        break;
                    default:
                        base.syncFrom(di, name);
                        break;
                }
            }
        }

        /// <summary>
        /// Refreshes this descriptor
        /// </summary>
        /// <param name="queue"></param>
        public override void Refresh(ReadQueue queue)
        {
            base.Refresh(queue);
            var handle = queue.With<IDevice>(DeviceInstance, ObjectIdentifier);
            handle.Enqueue(dev => dev.DatabaseRevision,
                rev => this.DatabaseRevision = (byte)rev,
                err => this.DatabaseRevision = null);
        }
    }
}
