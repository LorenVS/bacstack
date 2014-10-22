using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class AddressBinding
	{
		public ObjectId DeviceObjectIdentifier { get; private set; }

		public NetworkAddress DeviceAddress { get; private set; }

		public AddressBinding(ObjectId deviceObjectIdentifier, NetworkAddress deviceAddress)
		{
			this.DeviceObjectIdentifier = deviceObjectIdentifier;
			this.DeviceAddress = deviceAddress;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("DeviceObjectIdentifier", 255, Value<ObjectId>.Schema),
			new FieldSchema("DeviceAddress", 255, Value<NetworkAddress>.Schema));

		public static AddressBinding Load(IValueStream stream)
		{
			stream.EnterSequence();
			var deviceObjectIdentifier = Value<ObjectId>.Load(stream);
			var deviceAddress = Value<NetworkAddress>.Load(stream);
			stream.LeaveSequence();
			return new AddressBinding(deviceObjectIdentifier, deviceAddress);
		}

		public static void Save(IValueSink sink, AddressBinding value)
		{
			sink.EnterSequence();
			Value<ObjectId>.Save(sink, value.DeviceObjectIdentifier);
			Value<NetworkAddress>.Save(sink, value.DeviceAddress);
			sink.LeaveSequence();
		}
	}
}
