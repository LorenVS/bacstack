using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class RequestKeyRequest
	{
		public ObjectId RequestingDeviceIdentifier { get; private set; }

		public NetworkAddress RequestingDeviceAddress { get; private set; }

		public ObjectId RemoteDeviceIdentifier { get; private set; }

		public NetworkAddress RemoteDeviceAddress { get; private set; }

		public RequestKeyRequest(ObjectId requestingDeviceIdentifier, NetworkAddress requestingDeviceAddress, ObjectId remoteDeviceIdentifier, NetworkAddress remoteDeviceAddress)
		{
			this.RequestingDeviceIdentifier = requestingDeviceIdentifier;
			this.RequestingDeviceAddress = requestingDeviceAddress;
			this.RemoteDeviceIdentifier = remoteDeviceIdentifier;
			this.RemoteDeviceAddress = remoteDeviceAddress;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("RequestingDeviceIdentifier", 255, Value<ObjectId>.Schema),
			new FieldSchema("RequestingDeviceAddress", 255, Value<NetworkAddress>.Schema),
			new FieldSchema("RemoteDeviceIdentifier", 255, Value<ObjectId>.Schema),
			new FieldSchema("RemoteDeviceAddress", 255, Value<NetworkAddress>.Schema));

		public static RequestKeyRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var requestingDeviceIdentifier = Value<ObjectId>.Load(stream);
			var requestingDeviceAddress = Value<NetworkAddress>.Load(stream);
			var remoteDeviceIdentifier = Value<ObjectId>.Load(stream);
			var remoteDeviceAddress = Value<NetworkAddress>.Load(stream);
			stream.LeaveSequence();
			return new RequestKeyRequest(requestingDeviceIdentifier, requestingDeviceAddress, remoteDeviceIdentifier, remoteDeviceAddress);
		}

		public static void Save(IValueSink sink, RequestKeyRequest value)
		{
			sink.EnterSequence();
			Value<ObjectId>.Save(sink, value.RequestingDeviceIdentifier);
			Value<NetworkAddress>.Save(sink, value.RequestingDeviceAddress);
			Value<ObjectId>.Save(sink, value.RemoteDeviceIdentifier);
			Value<NetworkAddress>.Save(sink, value.RemoteDeviceAddress);
			sink.LeaveSequence();
		}
	}
}
