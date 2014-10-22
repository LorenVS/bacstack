using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public abstract  partial class CalendarEntry
	{
		public abstract Tags Tag { get; }

		public bool IsDate { get { return this.Tag == Tags.Date; } }

		public Date AsDate { get { return ((DateWrapper)this).Item; } }

		public static CalendarEntry NewDate(Date date)
		{
			return new DateWrapper(date);
		}

		public bool IsDateRange { get { return this.Tag == Tags.DateRange; } }

		public DateRange AsDateRange { get { return ((DateRangeWrapper)this).Item; } }

		public static CalendarEntry NewDateRange(DateRange dateRange)
		{
			return new DateRangeWrapper(dateRange);
		}

		public bool IsWeekNDay { get { return this.Tag == Tags.WeekNDay; } }

		public WeekNDay AsWeekNDay { get { return ((WeekNDayWrapper)this).Item; } }

		public static CalendarEntry NewWeekNDay(WeekNDay weekNDay)
		{
			return new WeekNDayWrapper(weekNDay);
		}

		public static readonly ISchema Schema = new ChoiceSchema(false, 
			new FieldSchema("Date", 0, Value<Date>.Schema),
			new FieldSchema("DateRange", 1, Value<DateRange>.Schema),
			new FieldSchema("WeekNDay", 2, Value<WeekNDay>.Schema));

		public static CalendarEntry Load(IValueStream stream)
		{
			CalendarEntry ret = null;
			Tags tag = (Tags)stream.EnterChoice();
			switch(tag)
			{
				case Tags.Date:
					ret = Value<DateWrapper>.Load(stream);
					break;
				case Tags.DateRange:
					ret = Value<DateRangeWrapper>.Load(stream);
					break;
				case Tags.WeekNDay:
					ret = Value<WeekNDayWrapper>.Load(stream);
					break;
				default:
					throw new Exception();
			}
			stream.LeaveChoice();
			return ret;
		}

		public static void Save(IValueSink sink, CalendarEntry value)
		{
			sink.EnterChoice((byte)value.Tag);
			switch(value.Tag)
			{
				case Tags.Date:
					Value<DateWrapper>.Save(sink, (DateWrapper)value);
					break;
				case Tags.DateRange:
					Value<DateRangeWrapper>.Save(sink, (DateRangeWrapper)value);
					break;
				case Tags.WeekNDay:
					Value<WeekNDayWrapper>.Save(sink, (WeekNDayWrapper)value);
					break;
				default:
					throw new Exception();
			}
			sink.LeaveChoice();
		}

		public enum Tags : byte
		{
			Date = 0,
			DateRange = 1,
			WeekNDay = 2
		}

		public  partial class DateWrapper : CalendarEntry
		{
			public override Tags Tag { get { return Tags.Date; } }

			public Date Item { get; private set; }

			public DateWrapper(Date item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Date>.Schema;

			public static new DateWrapper Load(IValueStream stream)
			{
				var temp = Value<Date>.Load(stream);
				return new DateWrapper(temp);
			}

			public static void Save(IValueSink sink, DateWrapper value)
			{
				Value<Date>.Save(sink, value.Item);
			}

		}

		public  partial class DateRangeWrapper : CalendarEntry
		{
			public override Tags Tag { get { return Tags.DateRange; } }

			public DateRange Item { get; private set; }

			public DateRangeWrapper(DateRange item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<DateRange>.Schema;

			public static new DateRangeWrapper Load(IValueStream stream)
			{
				var temp = Value<DateRange>.Load(stream);
				return new DateRangeWrapper(temp);
			}

			public static void Save(IValueSink sink, DateRangeWrapper value)
			{
				Value<DateRange>.Save(sink, value.Item);
			}

		}

		public  partial class WeekNDayWrapper : CalendarEntry
		{
			public override Tags Tag { get { return Tags.WeekNDay; } }

			public WeekNDay Item { get; private set; }

			public WeekNDayWrapper(WeekNDay item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<WeekNDay>.Schema;

			public static new WeekNDayWrapper Load(IValueStream stream)
			{
				var temp = Value<WeekNDay>.Load(stream);
				return new WeekNDayWrapper(temp);
			}

			public static void Save(IValueSink sink, WeekNDayWrapper value)
			{
				Value<WeekNDay>.Save(sink, value.Item);
			}

		}
	}
}
