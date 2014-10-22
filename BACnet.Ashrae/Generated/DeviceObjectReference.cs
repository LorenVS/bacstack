using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class DeviceObjectReference
	{
		public Option<ObjectId> DeviceIdentifier { get; private set; }

		public ObjectId ObjectIdentifier { get; private set; }

		public DeviceObjectReference(Option<ObjectId> deviceIdentifier, ObjectId objectIdentifier)
		{
			this.DeviceIdentifier = deviceIdentifier;
			this.ObjectIdentifier = objectIdentifier;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("DeviceIdentifier", 0, Value<Option<ObjectId>>.Schema),
			new FieldSchema("ObjectIdentifier", 1, Value<ObjectId>.Schema));

		public static DeviceObjectReference Load(IValueStream stream)
		{
			stream.EnterSequence();
			var deviceIdentifier = Value<Option<ObjectId>>.Load(stream);
			var objectIdentifier = Value<ObjectId>.Load(stream);
			stream.LeaveSequence();
			return new DeviceObjectReference(deviceIdentifier, objectIdentifier);
		}

		public static void Save(IValueSink sink, DeviceObjectReference value)
		{
			sink.EnterSequence();
			Value<Option<ObjectId>>.Save(sink, value.DeviceIdentifier);
			Value<ObjectId>.Save(sink, value.ObjectIdentifier);
			sink.LeaveSequence();
		}
	}
}
