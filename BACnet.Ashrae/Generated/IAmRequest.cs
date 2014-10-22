using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class IAmRequest
	{
		public ObjectId IAmDeviceIdentifier { get; private set; }

		public uint MaxAPDULengthAccepted { get; private set; }

		public Segmentation SegmentationSupported { get; private set; }

		public uint VendorID { get; private set; }

		public IAmRequest(ObjectId iAmDeviceIdentifier, uint maxAPDULengthAccepted, Segmentation segmentationSupported, uint vendorID)
		{
			this.IAmDeviceIdentifier = iAmDeviceIdentifier;
			this.MaxAPDULengthAccepted = maxAPDULengthAccepted;
			this.SegmentationSupported = segmentationSupported;
			this.VendorID = vendorID;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("IAmDeviceIdentifier", 255, Value<ObjectId>.Schema),
			new FieldSchema("MaxAPDULengthAccepted", 255, Value<uint>.Schema),
			new FieldSchema("SegmentationSupported", 255, Value<Segmentation>.Schema),
			new FieldSchema("VendorID", 255, Value<uint>.Schema));

		public static IAmRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var iAmDeviceIdentifier = Value<ObjectId>.Load(stream);
			var maxAPDULengthAccepted = Value<uint>.Load(stream);
			var segmentationSupported = Value<Segmentation>.Load(stream);
			var vendorID = Value<uint>.Load(stream);
			stream.LeaveSequence();
			return new IAmRequest(iAmDeviceIdentifier, maxAPDULengthAccepted, segmentationSupported, vendorID);
		}

		public static void Save(IValueSink sink, IAmRequest value)
		{
			sink.EnterSequence();
			Value<ObjectId>.Save(sink, value.IAmDeviceIdentifier);
			Value<uint>.Save(sink, value.MaxAPDULengthAccepted);
			Value<Segmentation>.Save(sink, value.SegmentationSupported);
			Value<uint>.Save(sink, value.VendorID);
			sink.LeaveSequence();
		}
	}
}
