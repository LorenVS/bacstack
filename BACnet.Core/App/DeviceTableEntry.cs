using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Core.Network;
using BACnet.Types;

namespace BACnet.Core.App
{
    public class DeviceTableEntry
    {
        /// <summary>
        /// The device instance
        /// </summary>
        public uint Instance { get; private set; }

        /// <summary>
        /// The object identifier for the device
        /// </summary>
        public ObjectId ObjectIdentifier { get { return new ObjectId((ushort)ObjectType.Device, Instance); } }

        /// <summary>
        /// The address of the device
        /// </summary>
        public Address Address { get; private set; }

        /// <summary>
        /// The maximum appgram length that this device
        /// can receive
        /// </summary>
        public uint MaxAppgramLength { get; private set; }

        /// <summary>
        /// The segmentation support of the device
        /// </summary>
        public Segmentation SegmentationSupport { get; private set; }

        /// <summary>
        /// The vendor id of the device
        /// </summary>
        public ushort VendorId { get; private set; }

        /// <summary>
        /// Constructs a new DeviceTableEntry instance
        /// </summary>
        /// <param name="instance">The device instance</param>
        /// <param name="address">The address of the device</param>
        /// <param name="maxAppgramLength">The maximum appgram length that this device can receive</param>
        /// <param name="segmentationSupport">The segmentation support of the device</param>
        /// <param name="vendorId">The vendor id of the device</param>
        public DeviceTableEntry(uint instance, Address address, uint maxAppgramLength, Segmentation segmentationSupport, ushort vendorId)
        {
            this.Instance = instance;
            this.Address = address;
            this.MaxAppgramLength = maxAppgramLength;
            this.SegmentationSupport = segmentationSupport;
            this.VendorId = vendorId;
        }
    }
}
