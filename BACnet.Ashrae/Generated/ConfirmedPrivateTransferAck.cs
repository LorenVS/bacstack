using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class ConfirmedPrivateTransferAck
	{
		public uint VendorID { get; private set; }

		public uint ServiceNumber { get; private set; }

		public Option<GenericValue> ResultBlock { get; private set; }

		public ConfirmedPrivateTransferAck(uint vendorID, uint serviceNumber, Option<GenericValue> resultBlock)
		{
			this.VendorID = vendorID;
			this.ServiceNumber = serviceNumber;
			this.ResultBlock = resultBlock;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("VendorID", 0, Value<uint>.Schema),
			new FieldSchema("ServiceNumber", 1, Value<uint>.Schema),
			new FieldSchema("ResultBlock", 2, Value<Option<GenericValue>>.Schema));

		public static ConfirmedPrivateTransferAck Load(IValueStream stream)
		{
			stream.EnterSequence();
			var vendorID = Value<uint>.Load(stream);
			var serviceNumber = Value<uint>.Load(stream);
			var resultBlock = Value<Option<GenericValue>>.Load(stream);
			stream.LeaveSequence();
			return new ConfirmedPrivateTransferAck(vendorID, serviceNumber, resultBlock);
		}

		public static void Save(IValueSink sink, ConfirmedPrivateTransferAck value)
		{
			sink.EnterSequence();
			Value<uint>.Save(sink, value.VendorID);
			Value<uint>.Save(sink, value.ServiceNumber);
			Value<Option<GenericValue>>.Save(sink, value.ResultBlock);
			sink.LeaveSequence();
		}
	}
}
