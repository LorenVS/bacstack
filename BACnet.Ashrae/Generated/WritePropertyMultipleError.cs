using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class WritePropertyMultipleError
	{
		public Error ErrorType { get; private set; }

		public ObjectPropertyReference FirstFailedWriteAttempt { get; private set; }

		public WritePropertyMultipleError(Error errorType, ObjectPropertyReference firstFailedWriteAttempt)
		{
			this.ErrorType = errorType;
			this.FirstFailedWriteAttempt = firstFailedWriteAttempt;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ErrorType", 0, Value<Error>.Schema),
			new FieldSchema("FirstFailedWriteAttempt", 1, Value<ObjectPropertyReference>.Schema));

		public static WritePropertyMultipleError Load(IValueStream stream)
		{
			stream.EnterSequence();
			var errorType = Value<Error>.Load(stream);
			var firstFailedWriteAttempt = Value<ObjectPropertyReference>.Load(stream);
			stream.LeaveSequence();
			return new WritePropertyMultipleError(errorType, firstFailedWriteAttempt);
		}

		public static void Save(IValueSink sink, WritePropertyMultipleError value)
		{
			sink.EnterSequence();
			Value<Error>.Save(sink, value.ErrorType);
			Value<ObjectPropertyReference>.Save(sink, value.FirstFailedWriteAttempt);
			sink.LeaveSequence();
		}
	}
}
