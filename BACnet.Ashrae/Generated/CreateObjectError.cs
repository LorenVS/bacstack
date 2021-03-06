using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class CreateObjectError
	{
		public Error ErrorType { get; private set; }

		public uint FirstFailedElementNumber { get; private set; }

		public CreateObjectError(Error errorType, uint firstFailedElementNumber)
		{
			this.ErrorType = errorType;
			this.FirstFailedElementNumber = firstFailedElementNumber;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ErrorType", 0, Value<Error>.Schema),
			new FieldSchema("FirstFailedElementNumber", 1, Value<uint>.Schema));

		public static CreateObjectError Load(IValueStream stream)
		{
			stream.EnterSequence();
			var errorType = Value<Error>.Load(stream);
			var firstFailedElementNumber = Value<uint>.Load(stream);
			stream.LeaveSequence();
			return new CreateObjectError(errorType, firstFailedElementNumber);
		}

		public static void Save(IValueSink sink, CreateObjectError value)
		{
			sink.EnterSequence();
			Value<Error>.Save(sink, value.ErrorType);
			Value<uint>.Save(sink, value.FirstFailedElementNumber);
			sink.LeaveSequence();
		}
	}
}
