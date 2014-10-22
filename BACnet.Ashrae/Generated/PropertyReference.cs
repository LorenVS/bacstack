using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class PropertyReference
	{
		public PropertyIdentifier PropertyIdentifier { get; private set; }

		public Option<uint> PropertyArrayIndex { get; private set; }

		public PropertyReference(PropertyIdentifier propertyIdentifier, Option<uint> propertyArrayIndex)
		{
			this.PropertyIdentifier = propertyIdentifier;
			this.PropertyArrayIndex = propertyArrayIndex;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("PropertyIdentifier", 0, Value<PropertyIdentifier>.Schema),
			new FieldSchema("PropertyArrayIndex", 1, Value<Option<uint>>.Schema));

		public static PropertyReference Load(IValueStream stream)
		{
			stream.EnterSequence();
			var propertyIdentifier = Value<PropertyIdentifier>.Load(stream);
			var propertyArrayIndex = Value<Option<uint>>.Load(stream);
			stream.LeaveSequence();
			return new PropertyReference(propertyIdentifier, propertyArrayIndex);
		}

		public static void Save(IValueSink sink, PropertyReference value)
		{
			sink.EnterSequence();
			Value<PropertyIdentifier>.Save(sink, value.PropertyIdentifier);
			Value<Option<uint>>.Save(sink, value.PropertyArrayIndex);
			sink.LeaveSequence();
		}
	}
}
