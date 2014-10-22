using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class TimeValue
	{
		public Time Time { get; private set; }

		public GenericValue Value { get; private set; }

		public TimeValue(Time time, GenericValue value)
		{
			this.Time = time;
			this.Value = value;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("Time", 255, Value<Time>.Schema),
			new FieldSchema("Value", 255, Value<GenericValue>.Schema));

		public static TimeValue Load(IValueStream stream)
		{
			stream.EnterSequence();
			var time = Value<Time>.Load(stream);
			var value = Value<GenericValue>.Load(stream);
			stream.LeaveSequence();
			return new TimeValue(time, value);
		}

		public static void Save(IValueSink sink, TimeValue value)
		{
			sink.EnterSequence();
			Value<Time>.Save(sink, value.Time);
			Value<GenericValue>.Save(sink, value.Value);
			sink.LeaveSequence();
		}
	}
}
