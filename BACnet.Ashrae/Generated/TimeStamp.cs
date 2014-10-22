using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public abstract  partial class TimeStamp
	{
		public abstract Tags Tag { get; }

		public bool IsTime { get { return this.Tag == Tags.Time; } }

		public Time AsTime { get { return ((TimeWrapper)this).Item; } }

		public static TimeStamp NewTime(Time time)
		{
			return new TimeWrapper(time);
		}

		public bool IsSequenceNumber { get { return this.Tag == Tags.SequenceNumber; } }

		public uint AsSequenceNumber { get { return ((SequenceNumberWrapper)this).Item; } }

		public static TimeStamp NewSequenceNumber(uint sequenceNumber)
		{
			return new SequenceNumberWrapper(sequenceNumber);
		}

		public bool IsDateTime { get { return this.Tag == Tags.DateTime; } }

		public DateAndTime AsDateTime { get { return ((DateTimeWrapper)this).Item; } }

		public static TimeStamp NewDateTime(DateAndTime dateTime)
		{
			return new DateTimeWrapper(dateTime);
		}

		public static readonly ISchema Schema = new ChoiceSchema(false, 
			new FieldSchema("Time", 0, Value<Time>.Schema),
			new FieldSchema("SequenceNumber", 1, Value<uint>.Schema),
			new FieldSchema("DateTime", 2, Value<DateAndTime>.Schema));

		public static TimeStamp Load(IValueStream stream)
		{
			TimeStamp ret = null;
			Tags tag = (Tags)stream.EnterChoice();
			switch(tag)
			{
				case Tags.Time:
					ret = Value<TimeWrapper>.Load(stream);
					break;
				case Tags.SequenceNumber:
					ret = Value<SequenceNumberWrapper>.Load(stream);
					break;
				case Tags.DateTime:
					ret = Value<DateTimeWrapper>.Load(stream);
					break;
				default:
					throw new Exception();
			}
			stream.LeaveChoice();
			return ret;
		}

		public static void Save(IValueSink sink, TimeStamp value)
		{
			sink.EnterChoice((byte)value.Tag);
			switch(value.Tag)
			{
				case Tags.Time:
					Value<TimeWrapper>.Save(sink, (TimeWrapper)value);
					break;
				case Tags.SequenceNumber:
					Value<SequenceNumberWrapper>.Save(sink, (SequenceNumberWrapper)value);
					break;
				case Tags.DateTime:
					Value<DateTimeWrapper>.Save(sink, (DateTimeWrapper)value);
					break;
				default:
					throw new Exception();
			}
			sink.LeaveChoice();
		}

		public enum Tags : byte
		{
			Time = 0,
			SequenceNumber = 1,
			DateTime = 2
		}

		public  partial class TimeWrapper : TimeStamp
		{
			public override Tags Tag { get { return Tags.Time; } }

			public Time Item { get; private set; }

			public TimeWrapper(Time item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Time>.Schema;

			public static new TimeWrapper Load(IValueStream stream)
			{
				var temp = Value<Time>.Load(stream);
				return new TimeWrapper(temp);
			}

			public static void Save(IValueSink sink, TimeWrapper value)
			{
				Value<Time>.Save(sink, value.Item);
			}

		}

		public  partial class SequenceNumberWrapper : TimeStamp
		{
			public override Tags Tag { get { return Tags.SequenceNumber; } }

			public uint Item { get; private set; }

			public SequenceNumberWrapper(uint item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<uint>.Schema;

			public static new SequenceNumberWrapper Load(IValueStream stream)
			{
				var temp = Value<uint>.Load(stream);
				return new SequenceNumberWrapper(temp);
			}

			public static void Save(IValueSink sink, SequenceNumberWrapper value)
			{
				Value<uint>.Save(sink, value.Item);
			}

		}

		public  partial class DateTimeWrapper : TimeStamp
		{
			public override Tags Tag { get { return Tags.DateTime; } }

			public DateAndTime Item { get; private set; }

			public DateTimeWrapper(DateAndTime item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<DateAndTime>.Schema;

			public static new DateTimeWrapper Load(IValueStream stream)
			{
				var temp = Value<DateAndTime>.Load(stream);
				return new DateTimeWrapper(temp);
			}

			public static void Save(IValueSink sink, DateTimeWrapper value)
			{
				Value<DateAndTime>.Save(sink, value.Item);
			}

		}
	}
}
