using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class SpecialEvent
	{
		public PeriodType Period { get; private set; }

		public ReadOnlyArray<TimeValue> ListOfTimeValues { get; private set; }

		public uint EventPriority { get; private set; }

		public SpecialEvent(PeriodType period, ReadOnlyArray<TimeValue> listOfTimeValues, uint eventPriority)
		{
			this.Period = period;
			this.ListOfTimeValues = listOfTimeValues;
			this.EventPriority = eventPriority;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("Period", 255, Value<PeriodType>.Schema),
			new FieldSchema("ListOfTimeValues", 2, Value<ReadOnlyArray<TimeValue>>.Schema),
			new FieldSchema("EventPriority", 3, Value<uint>.Schema));

		public static SpecialEvent Load(IValueStream stream)
		{
			stream.EnterSequence();
			var period = Value<PeriodType>.Load(stream);
			var listOfTimeValues = Value<ReadOnlyArray<TimeValue>>.Load(stream);
			var eventPriority = Value<uint>.Load(stream);
			stream.LeaveSequence();
			return new SpecialEvent(period, listOfTimeValues, eventPriority);
		}

		public static void Save(IValueSink sink, SpecialEvent value)
		{
			sink.EnterSequence();
			Value<PeriodType>.Save(sink, value.Period);
			Value<ReadOnlyArray<TimeValue>>.Save(sink, value.ListOfTimeValues);
			Value<uint>.Save(sink, value.EventPriority);
			sink.LeaveSequence();
		}

		public enum Tags : byte
		{
			CalendarEntry = 0,
			CalendarReference = 1
		}

		public abstract  partial class PeriodType
		{
			public abstract Tags Tag { get; }

			public bool IsCalendarEntry { get { return this.Tag == Tags.CalendarEntry; } }

			public CalendarEntry AsCalendarEntry { get { return ((CalendarEntryWrapper)this).Item; } }

			public static PeriodType NewCalendarEntry(CalendarEntry calendarEntry)
			{
				return new CalendarEntryWrapper(calendarEntry);
			}

			public bool IsCalendarReference { get { return this.Tag == Tags.CalendarReference; } }

			public ObjectId AsCalendarReference { get { return ((CalendarReferenceWrapper)this).Item; } }

			public static PeriodType NewCalendarReference(ObjectId calendarReference)
			{
				return new CalendarReferenceWrapper(calendarReference);
			}

			public static readonly ISchema Schema = new ChoiceSchema(false, 
				new FieldSchema("CalendarEntry", 0, Value<CalendarEntry>.Schema),
				new FieldSchema("CalendarReference", 1, Value<ObjectId>.Schema));

			public static PeriodType Load(IValueStream stream)
			{
				PeriodType ret = null;
				Tags tag = (Tags)stream.EnterChoice();
				switch(tag)
				{
					case Tags.CalendarEntry:
						ret = Value<CalendarEntryWrapper>.Load(stream);
						break;
					case Tags.CalendarReference:
						ret = Value<CalendarReferenceWrapper>.Load(stream);
						break;
					default:
						throw new Exception();
				}
				stream.LeaveChoice();
				return ret;
			}

			public static void Save(IValueSink sink, PeriodType value)
			{
				sink.EnterChoice((byte)value.Tag);
				switch(value.Tag)
				{
					case Tags.CalendarEntry:
						Value<CalendarEntryWrapper>.Save(sink, (CalendarEntryWrapper)value);
						break;
					case Tags.CalendarReference:
						Value<CalendarReferenceWrapper>.Save(sink, (CalendarReferenceWrapper)value);
						break;
					default:
						throw new Exception();
				}
				sink.LeaveChoice();
			}
		}

		public  partial class CalendarEntryWrapper : PeriodType
		{
			public override Tags Tag { get { return Tags.CalendarEntry; } }

			public CalendarEntry Item { get; private set; }

			public CalendarEntryWrapper(CalendarEntry item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<CalendarEntry>.Schema;

			public static new CalendarEntryWrapper Load(IValueStream stream)
			{
				var temp = Value<CalendarEntry>.Load(stream);
				return new CalendarEntryWrapper(temp);
			}

			public static void Save(IValueSink sink, CalendarEntryWrapper value)
			{
				Value<CalendarEntry>.Save(sink, value.Item);
			}

		}

		public  partial class CalendarReferenceWrapper : PeriodType
		{
			public override Tags Tag { get { return Tags.CalendarReference; } }

			public ObjectId Item { get; private set; }

			public CalendarReferenceWrapper(ObjectId item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ObjectId>.Schema;

			public static new CalendarReferenceWrapper Load(IValueStream stream)
			{
				var temp = Value<ObjectId>.Load(stream);
				return new CalendarReferenceWrapper(temp);
			}

			public static void Save(IValueSink sink, CalendarReferenceWrapper value)
			{
				Value<ObjectId>.Save(sink, value.Item);
			}

		}
	}
}
