using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class ConfirmedPrivateTransferRequest
	{
		public uint VendorID { get; private set; }

		public uint ServiceNumber { get; private set; }

		public Option<GenericValue> ServiceParameters { get; private set; }

		public ConfirmedPrivateTransferRequest(uint vendorID, uint serviceNumber, Option<GenericValue> serviceParameters)
		{
			this.VendorID = vendorID;
			this.ServiceNumber = serviceNumber;
			this.ServiceParameters = serviceParameters;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("VendorID", 0, Value<uint>.Schema),
			new FieldSchema("ServiceNumber", 1, Value<uint>.Schema),
			new FieldSchema("ServiceParameters", 2, Value<Option<GenericValue>>.Schema));

		public static ConfirmedPrivateTransferRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var vendorID = Value<uint>.Load(stream);
			var serviceNumber = Value<uint>.Load(stream);
			var serviceParameters = Value<Option<GenericValue>>.Load(stream);
			stream.LeaveSequence();
			return new ConfirmedPrivateTransferRequest(vendorID, serviceNumber, serviceParameters);
		}

		public static void Save(IValueSink sink, ConfirmedPrivateTransferRequest value)
		{
			sink.EnterSequence();
			Value<uint>.Save(sink, value.VendorID);
			Value<uint>.Save(sink, value.ServiceNumber);
			Value<Option<GenericValue>>.Save(sink, value.ServiceParameters);
			sink.LeaveSequence();
		}
	}
}
