using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class UTCTimeSynchronizationRequest
	{
		public DateAndTime Time { get; private set; }

		public UTCTimeSynchronizationRequest(DateAndTime time)
		{
			this.Time = time;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("Time", 255, Value<DateAndTime>.Schema));

		public static UTCTimeSynchronizationRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var time = Value<DateAndTime>.Load(stream);
			stream.LeaveSequence();
			return new UTCTimeSynchronizationRequest(time);
		}

		public static void Save(IValueSink sink, UTCTimeSynchronizationRequest value)
		{
			sink.EnterSequence();
			Value<DateAndTime>.Save(sink, value.Time);
			sink.LeaveSequence();
		}
	}
}
