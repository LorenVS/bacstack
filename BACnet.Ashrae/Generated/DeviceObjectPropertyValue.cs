using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class DeviceObjectPropertyValue
	{
		public ObjectId DeviceIdentifier { get; private set; }

		public ObjectId ObjectIdentifier { get; private set; }

		public PropertyIdentifier PropertyIdentifier { get; private set; }

		public Option<uint> ArrayIndex { get; private set; }

		public GenericValue Value { get; private set; }

		public DeviceObjectPropertyValue(ObjectId deviceIdentifier, ObjectId objectIdentifier, PropertyIdentifier propertyIdentifier, Option<uint> arrayIndex, GenericValue value)
		{
			this.DeviceIdentifier = deviceIdentifier;
			this.ObjectIdentifier = objectIdentifier;
			this.PropertyIdentifier = propertyIdentifier;
			this.ArrayIndex = arrayIndex;
			this.Value = value;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("DeviceIdentifier", 0, Value<ObjectId>.Schema),
			new FieldSchema("ObjectIdentifier", 1, Value<ObjectId>.Schema),
			new FieldSchema("PropertyIdentifier", 2, Value<PropertyIdentifier>.Schema),
			new FieldSchema("ArrayIndex", 3, Value<Option<uint>>.Schema),
			new FieldSchema("Value", 4, Value<GenericValue>.Schema));

		public static DeviceObjectPropertyValue Load(IValueStream stream)
		{
			stream.EnterSequence();
			var deviceIdentifier = Value<ObjectId>.Load(stream);
			var objectIdentifier = Value<ObjectId>.Load(stream);
			var propertyIdentifier = Value<PropertyIdentifier>.Load(stream);
			var arrayIndex = Value<Option<uint>>.Load(stream);
			var value = Value<GenericValue>.Load(stream);
			stream.LeaveSequence();
			return new DeviceObjectPropertyValue(deviceIdentifier, objectIdentifier, propertyIdentifier, arrayIndex, value);
		}

		public static void Save(IValueSink sink, DeviceObjectPropertyValue value)
		{
			sink.EnterSequence();
			Value<ObjectId>.Save(sink, value.DeviceIdentifier);
			Value<ObjectId>.Save(sink, value.ObjectIdentifier);
			Value<PropertyIdentifier>.Save(sink, value.PropertyIdentifier);
			Value<Option<uint>>.Save(sink, value.ArrayIndex);
			Value<GenericValue>.Save(sink, value.Value);
			sink.LeaveSequence();
		}
	}
}
