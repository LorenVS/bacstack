using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class DeviceObjectPropertyReference
	{
		public ObjectId ObjectIdentifier { get; private set; }

		public PropertyIdentifier PropertyIdentifier { get; private set; }

		public Option<uint> PropertyArrayIndex { get; private set; }

		public Option<ObjectId> DeviceIdentifier { get; private set; }

		public DeviceObjectPropertyReference(ObjectId objectIdentifier, PropertyIdentifier propertyIdentifier, Option<uint> propertyArrayIndex, Option<ObjectId> deviceIdentifier)
		{
			this.ObjectIdentifier = objectIdentifier;
			this.PropertyIdentifier = propertyIdentifier;
			this.PropertyArrayIndex = propertyArrayIndex;
			this.DeviceIdentifier = deviceIdentifier;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ObjectIdentifier", 0, Value<ObjectId>.Schema),
			new FieldSchema("PropertyIdentifier", 1, Value<PropertyIdentifier>.Schema),
			new FieldSchema("PropertyArrayIndex", 2, Value<Option<uint>>.Schema),
			new FieldSchema("DeviceIdentifier", 3, Value<Option<ObjectId>>.Schema));

		public static DeviceObjectPropertyReference Load(IValueStream stream)
		{
			stream.EnterSequence();
			var objectIdentifier = Value<ObjectId>.Load(stream);
			var propertyIdentifier = Value<PropertyIdentifier>.Load(stream);
			var propertyArrayIndex = Value<Option<uint>>.Load(stream);
			var deviceIdentifier = Value<Option<ObjectId>>.Load(stream);
			stream.LeaveSequence();
			return new DeviceObjectPropertyReference(objectIdentifier, propertyIdentifier, propertyArrayIndex, deviceIdentifier);
		}

		public static void Save(IValueSink sink, DeviceObjectPropertyReference value)
		{
			sink.EnterSequence();
			Value<ObjectId>.Save(sink, value.ObjectIdentifier);
			Value<PropertyIdentifier>.Save(sink, value.PropertyIdentifier);
			Value<Option<uint>>.Save(sink, value.PropertyArrayIndex);
			Value<Option<ObjectId>>.Save(sink, value.DeviceIdentifier);
			sink.LeaveSequence();
		}
	}
}
