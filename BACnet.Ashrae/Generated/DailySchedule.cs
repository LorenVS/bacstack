using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class DailySchedule
	{
		public ReadOnlyArray<TimeValue> DaySchedule { get; private set; }

		public DailySchedule(ReadOnlyArray<TimeValue> daySchedule)
		{
			this.DaySchedule = daySchedule;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("DaySchedule", 0, Value<ReadOnlyArray<TimeValue>>.Schema));

		public static DailySchedule Load(IValueStream stream)
		{
			stream.EnterSequence();
			var daySchedule = Value<ReadOnlyArray<TimeValue>>.Load(stream);
			stream.LeaveSequence();
			return new DailySchedule(daySchedule);
		}

		public static void Save(IValueSink sink, DailySchedule value)
		{
			sink.EnterSequence();
			Value<ReadOnlyArray<TimeValue>>.Save(sink, value.DaySchedule);
			sink.LeaveSequence();
		}
	}
}
