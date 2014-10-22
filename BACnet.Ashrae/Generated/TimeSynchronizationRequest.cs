using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class TimeSynchronizationRequest
	{
		public DateAndTime Time { get; private set; }

		public TimeSynchronizationRequest(DateAndTime time)
		{
			this.Time = time;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("Time", 255, Value<DateAndTime>.Schema));

		public static TimeSynchronizationRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var time = Value<DateAndTime>.Load(stream);
			stream.LeaveSequence();
			return new TimeSynchronizationRequest(time);
		}

		public static void Save(IValueSink sink, TimeSynchronizationRequest value)
		{
			sink.EnterSequence();
			Value<DateAndTime>.Save(sink, value.Time);
			sink.LeaveSequence();
		}
	}
}
