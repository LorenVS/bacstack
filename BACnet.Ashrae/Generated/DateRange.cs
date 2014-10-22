using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class DateRange
	{
		public Date StartDate { get; private set; }

		public Date EndDate { get; private set; }

		public DateRange(Date startDate, Date endDate)
		{
			this.StartDate = startDate;
			this.EndDate = endDate;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("StartDate", 255, Value<Date>.Schema),
			new FieldSchema("EndDate", 255, Value<Date>.Schema));

		public static DateRange Load(IValueStream stream)
		{
			stream.EnterSequence();
			var startDate = Value<Date>.Load(stream);
			var endDate = Value<Date>.Load(stream);
			stream.LeaveSequence();
			return new DateRange(startDate, endDate);
		}

		public static void Save(IValueSink sink, DateRange value)
		{
			sink.EnterSequence();
			Value<Date>.Save(sink, value.StartDate);
			Value<Date>.Save(sink, value.EndDate);
			sink.LeaveSequence();
		}
	}
}
