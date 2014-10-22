using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class PropertyValue
	{
		public PropertyIdentifier PropertyIdentifier { get; private set; }

		public Option<uint> PropertyArrayIndex { get; private set; }

		public GenericValue Value { get; private set; }

		public Option<uint> Priority { get; private set; }

		public PropertyValue(PropertyIdentifier propertyIdentifier, Option<uint> propertyArrayIndex, GenericValue value, Option<uint> priority)
		{
			this.PropertyIdentifier = propertyIdentifier;
			this.PropertyArrayIndex = propertyArrayIndex;
			this.Value = value;
			this.Priority = priority;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("PropertyIdentifier", 0, Value<PropertyIdentifier>.Schema),
			new FieldSchema("PropertyArrayIndex", 1, Value<Option<uint>>.Schema),
			new FieldSchema("Value", 2, Value<GenericValue>.Schema),
			new FieldSchema("Priority", 3, Value<Option<uint>>.Schema));

		public static PropertyValue Load(IValueStream stream)
		{
			stream.EnterSequence();
			var propertyIdentifier = Value<PropertyIdentifier>.Load(stream);
			var propertyArrayIndex = Value<Option<uint>>.Load(stream);
			var value = Value<GenericValue>.Load(stream);
			var priority = Value<Option<uint>>.Load(stream);
			stream.LeaveSequence();
			return new PropertyValue(propertyIdentifier, propertyArrayIndex, value, priority);
		}

		public static void Save(IValueSink sink, PropertyValue value)
		{
			sink.EnterSequence();
			Value<PropertyIdentifier>.Save(sink, value.PropertyIdentifier);
			Value<Option<uint>>.Save(sink, value.PropertyArrayIndex);
			Value<GenericValue>.Save(sink, value.Value);
			Value<Option<uint>>.Save(sink, value.Priority);
			sink.LeaveSequence();
		}
	}
}
