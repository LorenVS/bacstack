using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class ConfirmedPrivateTransferError
	{
		public Error ErrorType { get; private set; }

		public uint VendorID { get; private set; }

		public uint ServiceNumber { get; private set; }

		public Option<GenericValue> ErrorParameters { get; private set; }

		public ConfirmedPrivateTransferError(Error errorType, uint vendorID, uint serviceNumber, Option<GenericValue> errorParameters)
		{
			this.ErrorType = errorType;
			this.VendorID = vendorID;
			this.ServiceNumber = serviceNumber;
			this.ErrorParameters = errorParameters;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ErrorType", 0, Value<Error>.Schema),
			new FieldSchema("VendorID", 1, Value<uint>.Schema),
			new FieldSchema("ServiceNumber", 2, Value<uint>.Schema),
			new FieldSchema("ErrorParameters", 3, Value<Option<GenericValue>>.Schema));

		public static ConfirmedPrivateTransferError Load(IValueStream stream)
		{
			stream.EnterSequence();
			var errorType = Value<Error>.Load(stream);
			var vendorID = Value<uint>.Load(stream);
			var serviceNumber = Value<uint>.Load(stream);
			var errorParameters = Value<Option<GenericValue>>.Load(stream);
			stream.LeaveSequence();
			return new ConfirmedPrivateTransferError(errorType, vendorID, serviceNumber, errorParameters);
		}

		public static void Save(IValueSink sink, ConfirmedPrivateTransferError value)
		{
			sink.EnterSequence();
			Value<Error>.Save(sink, value.ErrorType);
			Value<uint>.Save(sink, value.VendorID);
			Value<uint>.Save(sink, value.ServiceNumber);
			Value<Option<GenericValue>>.Save(sink, value.ErrorParameters);
			sink.LeaveSequence();
		}
	}
}
