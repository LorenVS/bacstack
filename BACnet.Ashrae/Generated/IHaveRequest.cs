using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class IHaveRequest
	{
		public ObjectId DeviceIdentifier { get; private set; }

		public ObjectId ObjectIdentifier { get; private set; }

		public string ObjectName { get; private set; }

		public IHaveRequest(ObjectId deviceIdentifier, ObjectId objectIdentifier, string objectName)
		{
			this.DeviceIdentifier = deviceIdentifier;
			this.ObjectIdentifier = objectIdentifier;
			this.ObjectName = objectName;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("DeviceIdentifier", 255, Value<ObjectId>.Schema),
			new FieldSchema("ObjectIdentifier", 255, Value<ObjectId>.Schema),
			new FieldSchema("ObjectName", 255, Value<string>.Schema));

		public static IHaveRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var deviceIdentifier = Value<ObjectId>.Load(stream);
			var objectIdentifier = Value<ObjectId>.Load(stream);
			var objectName = Value<string>.Load(stream);
			stream.LeaveSequence();
			return new IHaveRequest(deviceIdentifier, objectIdentifier, objectName);
		}

		public static void Save(IValueSink sink, IHaveRequest value)
		{
			sink.EnterSequence();
			Value<ObjectId>.Save(sink, value.DeviceIdentifier);
			Value<ObjectId>.Save(sink, value.ObjectIdentifier);
			Value<string>.Save(sink, value.ObjectName);
			sink.LeaveSequence();
		}
	}
}
