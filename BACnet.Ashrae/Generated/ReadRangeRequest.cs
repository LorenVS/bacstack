using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class ReadRangeRequest
	{
		public ObjectId ObjectIdentifier { get; private set; }

		public PropertyIdentifier PropertyIdentifier { get; private set; }

		public Option<uint> PropertyArrayIndex { get; private set; }

		public Option<RangeType> Range { get; private set; }

		public ReadRangeRequest(ObjectId objectIdentifier, PropertyIdentifier propertyIdentifier, Option<uint> propertyArrayIndex, Option<RangeType> range)
		{
			this.ObjectIdentifier = objectIdentifier;
			this.PropertyIdentifier = propertyIdentifier;
			this.PropertyArrayIndex = propertyArrayIndex;
			this.Range = range;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ObjectIdentifier", 0, Value<ObjectId>.Schema),
			new FieldSchema("PropertyIdentifier", 1, Value<PropertyIdentifier>.Schema),
			new FieldSchema("PropertyArrayIndex", 2, Value<Option<uint>>.Schema),
			new FieldSchema("Range", 255, Value<Option<RangeType>>.Schema));

		public static ReadRangeRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var objectIdentifier = Value<ObjectId>.Load(stream);
			var propertyIdentifier = Value<PropertyIdentifier>.Load(stream);
			var propertyArrayIndex = Value<Option<uint>>.Load(stream);
			var range = Value<Option<RangeType>>.Load(stream);
			stream.LeaveSequence();
			return new ReadRangeRequest(objectIdentifier, propertyIdentifier, propertyArrayIndex, range);
		}

		public static void Save(IValueSink sink, ReadRangeRequest value)
		{
			sink.EnterSequence();
			Value<ObjectId>.Save(sink, value.ObjectIdentifier);
			Value<PropertyIdentifier>.Save(sink, value.PropertyIdentifier);
			Value<Option<uint>>.Save(sink, value.PropertyArrayIndex);
			Value<Option<RangeType>>.Save(sink, value.Range);
			sink.LeaveSequence();
		}

		public enum Tags : byte
		{
			ByPosition = 0,
			BySequenceNumber = 1,
			ByTime = 2
		}

		public abstract  partial class RangeType
		{
			public abstract Tags Tag { get; }

			public bool IsByPosition { get { return this.Tag == Tags.ByPosition; } }

			public ByPosition AsByPosition { get { return (ByPosition)this; } }

			public static RangeType NewByPosition(uint referenceIndex, int count)
			{
				return new ByPosition(referenceIndex, count);
			}

			public bool IsBySequenceNumber { get { return this.Tag == Tags.BySequenceNumber; } }

			public BySequenceNumber AsBySequenceNumber { get { return (BySequenceNumber)this; } }

			public static RangeType NewBySequenceNumber(uint referenceIndex, int count)
			{
				return new BySequenceNumber(referenceIndex, count);
			}

			public bool IsByTime { get { return this.Tag == Tags.ByTime; } }

			public ByTime AsByTime { get { return (ByTime)this; } }

			public static RangeType NewByTime(DateAndTime referenceTime, int count)
			{
				return new ByTime(referenceTime, count);
			}

			public static readonly ISchema Schema = new ChoiceSchema(false, 
				new FieldSchema("ByPosition", 3, Value<ByPosition>.Schema),
				new FieldSchema("BySequenceNumber", 6, Value<BySequenceNumber>.Schema),
				new FieldSchema("ByTime", 7, Value<ByTime>.Schema));

			public static RangeType Load(IValueStream stream)
			{
				RangeType ret = null;
				Tags tag = (Tags)stream.EnterChoice();
				switch(tag)
				{
					case Tags.ByPosition:
						ret = Value<ByPosition>.Load(stream);
						break;
					case Tags.BySequenceNumber:
						ret = Value<BySequenceNumber>.Load(stream);
						break;
					case Tags.ByTime:
						ret = Value<ByTime>.Load(stream);
						break;
					default:
						throw new Exception();
				}
				stream.LeaveChoice();
				return ret;
			}

			public static void Save(IValueSink sink, RangeType value)
			{
				sink.EnterChoice((byte)value.Tag);
				switch(value.Tag)
				{
					case Tags.ByPosition:
						Value<ByPosition>.Save(sink, (ByPosition)value);
						break;
					case Tags.BySequenceNumber:
						Value<BySequenceNumber>.Save(sink, (BySequenceNumber)value);
						break;
					case Tags.ByTime:
						Value<ByTime>.Save(sink, (ByTime)value);
						break;
					default:
						throw new Exception();
				}
				sink.LeaveChoice();
			}
		}
		public  partial class ByPosition : RangeType
		{
			public override Tags Tag { get { return Tags.ByPosition; } }

			public uint ReferenceIndex { get; private set; }

			public int Count { get; private set; }

			public ByPosition(uint referenceIndex, int count)
			{
				this.ReferenceIndex = referenceIndex;
				this.Count = count;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("ReferenceIndex", 255, Value<uint>.Schema),
				new FieldSchema("Count", 255, Value<int>.Schema));

			public static new ByPosition Load(IValueStream stream)
			{
				stream.EnterSequence();
				var referenceIndex = Value<uint>.Load(stream);
				var count = Value<int>.Load(stream);
				stream.LeaveSequence();
				return new ByPosition(referenceIndex, count);
			}

			public static void Save(IValueSink sink, ByPosition value)
			{
				sink.EnterSequence();
				Value<uint>.Save(sink, value.ReferenceIndex);
				Value<int>.Save(sink, value.Count);
				sink.LeaveSequence();
			}
		}
		public  partial class BySequenceNumber : RangeType
		{
			public override Tags Tag { get { return Tags.BySequenceNumber; } }

			public uint ReferenceIndex { get; private set; }

			public int Count { get; private set; }

			public BySequenceNumber(uint referenceIndex, int count)
			{
				this.ReferenceIndex = referenceIndex;
				this.Count = count;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("ReferenceIndex", 255, Value<uint>.Schema),
				new FieldSchema("Count", 255, Value<int>.Schema));

			public static new BySequenceNumber Load(IValueStream stream)
			{
				stream.EnterSequence();
				var referenceIndex = Value<uint>.Load(stream);
				var count = Value<int>.Load(stream);
				stream.LeaveSequence();
				return new BySequenceNumber(referenceIndex, count);
			}

			public static void Save(IValueSink sink, BySequenceNumber value)
			{
				sink.EnterSequence();
				Value<uint>.Save(sink, value.ReferenceIndex);
				Value<int>.Save(sink, value.Count);
				sink.LeaveSequence();
			}
		}
		public  partial class ByTime : RangeType
		{
			public override Tags Tag { get { return Tags.ByTime; } }

			public DateAndTime ReferenceTime { get; private set; }

			public int Count { get; private set; }

			public ByTime(DateAndTime referenceTime, int count)
			{
				this.ReferenceTime = referenceTime;
				this.Count = count;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("ReferenceTime", 255, Value<DateAndTime>.Schema),
				new FieldSchema("Count", 255, Value<int>.Schema));

			public static new ByTime Load(IValueStream stream)
			{
				stream.EnterSequence();
				var referenceTime = Value<DateAndTime>.Load(stream);
				var count = Value<int>.Load(stream);
				stream.LeaveSequence();
				return new ByTime(referenceTime, count);
			}

			public static void Save(IValueSink sink, ByTime value)
			{
				sink.EnterSequence();
				Value<DateAndTime>.Save(sink, value.ReferenceTime);
				Value<int>.Save(sink, value.Count);
				sink.LeaveSequence();
			}
		}
	}
}
