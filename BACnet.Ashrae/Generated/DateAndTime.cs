using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class DateAndTime
	{
		public Date Date { get; private set; }

		public Time Time { get; private set; }

		public DateAndTime(Date date, Time time)
		{
			this.Date = date;
			this.Time = time;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("Date", 255, Value<Date>.Schema),
			new FieldSchema("Time", 255, Value<Time>.Schema));

		public static DateAndTime Load(IValueStream stream)
		{
			stream.EnterSequence();
			var date = Value<Date>.Load(stream);
			var time = Value<Time>.Load(stream);
			stream.LeaveSequence();
			return new DateAndTime(date, time);
		}

		public static void Save(IValueSink sink, DateAndTime value)
		{
			sink.EnterSequence();
			Value<Date>.Save(sink, value.Date);
			Value<Time>.Save(sink, value.Time);
			sink.LeaveSequence();
		}
	}
}
