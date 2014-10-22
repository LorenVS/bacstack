using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class ReadRangeAck
	{
		public ObjectId ObjectIdentifier { get; private set; }

		public PropertyIdentifier PropertyIdentifier { get; private set; }

		public Option<uint> PropertyArrayIndex { get; private set; }

		public ResultFlags ResultFlags { get; private set; }

		public uint ItemCount { get; private set; }

		public ReadOnlyArray<GenericValue> ItemData { get; private set; }

		public Option<uint> FirstSequenceNumber { get; private set; }

		public ReadRangeAck(ObjectId objectIdentifier, PropertyIdentifier propertyIdentifier, Option<uint> propertyArrayIndex, ResultFlags resultFlags, uint itemCount, ReadOnlyArray<GenericValue> itemData, Option<uint> firstSequenceNumber)
		{
			this.ObjectIdentifier = objectIdentifier;
			this.PropertyIdentifier = propertyIdentifier;
			this.PropertyArrayIndex = propertyArrayIndex;
			this.ResultFlags = resultFlags;
			this.ItemCount = itemCount;
			this.ItemData = itemData;
			this.FirstSequenceNumber = firstSequenceNumber;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ObjectIdentifier", 0, Value<ObjectId>.Schema),
			new FieldSchema("PropertyIdentifier", 1, Value<PropertyIdentifier>.Schema),
			new FieldSchema("PropertyArrayIndex", 2, Value<Option<uint>>.Schema),
			new FieldSchema("ResultFlags", 3, Value<ResultFlags>.Schema),
			new FieldSchema("ItemCount", 4, Value<uint>.Schema),
			new FieldSchema("ItemData", 5, Value<ReadOnlyArray<GenericValue>>.Schema),
			new FieldSchema("FirstSequenceNumber", 6, Value<Option<uint>>.Schema));

		public static ReadRangeAck Load(IValueStream stream)
		{
			stream.EnterSequence();
			var objectIdentifier = Value<ObjectId>.Load(stream);
			var propertyIdentifier = Value<PropertyIdentifier>.Load(stream);
			var propertyArrayIndex = Value<Option<uint>>.Load(stream);
			var resultFlags = Value<ResultFlags>.Load(stream);
			var itemCount = Value<uint>.Load(stream);
			var itemData = Value<ReadOnlyArray<GenericValue>>.Load(stream);
			var firstSequenceNumber = Value<Option<uint>>.Load(stream);
			stream.LeaveSequence();
			return new ReadRangeAck(objectIdentifier, propertyIdentifier, propertyArrayIndex, resultFlags, itemCount, itemData, firstSequenceNumber);
		}

		public static void Save(IValueSink sink, ReadRangeAck value)
		{
			sink.EnterSequence();
			Value<ObjectId>.Save(sink, value.ObjectIdentifier);
			Value<PropertyIdentifier>.Save(sink, value.PropertyIdentifier);
			Value<Option<uint>>.Save(sink, value.PropertyArrayIndex);
			Value<ResultFlags>.Save(sink, value.ResultFlags);
			Value<uint>.Save(sink, value.ItemCount);
			Value<ReadOnlyArray<GenericValue>>.Save(sink, value.ItemData);
			Value<Option<uint>>.Save(sink, value.FirstSequenceNumber);
			sink.LeaveSequence();
		}
	}
}
