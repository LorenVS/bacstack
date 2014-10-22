using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class ObjectPropertyValue
	{
		public ObjectId ObjectIdentifier { get; private set; }

		public PropertyIdentifier PropertyIdentifier { get; private set; }

		public Option<uint> PropertyArrayIndex { get; private set; }

		public GenericValue Value { get; private set; }

		public Option<uint> Priority { get; private set; }

		public ObjectPropertyValue(ObjectId objectIdentifier, PropertyIdentifier propertyIdentifier, Option<uint> propertyArrayIndex, GenericValue value, Option<uint> priority)
		{
			this.ObjectIdentifier = objectIdentifier;
			this.PropertyIdentifier = propertyIdentifier;
			this.PropertyArrayIndex = propertyArrayIndex;
			this.Value = value;
			this.Priority = priority;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ObjectIdentifier", 0, Value<ObjectId>.Schema),
			new FieldSchema("PropertyIdentifier", 1, Value<PropertyIdentifier>.Schema),
			new FieldSchema("PropertyArrayIndex", 2, Value<Option<uint>>.Schema),
			new FieldSchema("Value", 3, Value<GenericValue>.Schema),
			new FieldSchema("Priority", 4, Value<Option<uint>>.Schema));

		public static ObjectPropertyValue Load(IValueStream stream)
		{
			stream.EnterSequence();
			var objectIdentifier = Value<ObjectId>.Load(stream);
			var propertyIdentifier = Value<PropertyIdentifier>.Load(stream);
			var propertyArrayIndex = Value<Option<uint>>.Load(stream);
			var value = Value<GenericValue>.Load(stream);
			var priority = Value<Option<uint>>.Load(stream);
			stream.LeaveSequence();
			return new ObjectPropertyValue(objectIdentifier, propertyIdentifier, propertyArrayIndex, value, priority);
		}

		public static void Save(IValueSink sink, ObjectPropertyValue value)
		{
			sink.EnterSequence();
			Value<ObjectId>.Save(sink, value.ObjectIdentifier);
			Value<PropertyIdentifier>.Save(sink, value.PropertyIdentifier);
			Value<Option<uint>>.Save(sink, value.PropertyArrayIndex);
			Value<GenericValue>.Save(sink, value.Value);
			Value<Option<uint>>.Save(sink, value.Priority);
			sink.LeaveSequence();
		}
	}
}
