using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class WritePropertyRequest
	{
		public ObjectId ObjectIdentifier { get; private set; }

		public PropertyIdentifier PropertyIdentifier { get; private set; }

		public Option<uint> PropertyArrayIndex { get; private set; }

		public GenericValue PropertyValue { get; private set; }

		public Option<byte> Priority { get; private set; }

		public WritePropertyRequest(ObjectId objectIdentifier, PropertyIdentifier propertyIdentifier, Option<uint> propertyArrayIndex, GenericValue propertyValue, Option<byte> priority)
		{
			this.ObjectIdentifier = objectIdentifier;
			this.PropertyIdentifier = propertyIdentifier;
			this.PropertyArrayIndex = propertyArrayIndex;
			this.PropertyValue = propertyValue;
			this.Priority = priority;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ObjectIdentifier", 0, Value<ObjectId>.Schema),
			new FieldSchema("PropertyIdentifier", 1, Value<PropertyIdentifier>.Schema),
			new FieldSchema("PropertyArrayIndex", 2, Value<Option<uint>>.Schema),
			new FieldSchema("PropertyValue", 3, Value<GenericValue>.Schema),
			new FieldSchema("Priority", 4, Value<Option<byte>>.Schema));

		public static WritePropertyRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var objectIdentifier = Value<ObjectId>.Load(stream);
			var propertyIdentifier = Value<PropertyIdentifier>.Load(stream);
			var propertyArrayIndex = Value<Option<uint>>.Load(stream);
			var propertyValue = Value<GenericValue>.Load(stream);
			var priority = Value<Option<byte>>.Load(stream);
			stream.LeaveSequence();
			return new WritePropertyRequest(objectIdentifier, propertyIdentifier, propertyArrayIndex, propertyValue, priority);
		}

		public static void Save(IValueSink sink, WritePropertyRequest value)
		{
			sink.EnterSequence();
			Value<ObjectId>.Save(sink, value.ObjectIdentifier);
			Value<PropertyIdentifier>.Save(sink, value.PropertyIdentifier);
			Value<Option<uint>>.Save(sink, value.PropertyArrayIndex);
			Value<GenericValue>.Save(sink, value.PropertyValue);
			Value<Option<byte>>.Save(sink, value.Priority);
			sink.LeaveSequence();
		}
	}
}
