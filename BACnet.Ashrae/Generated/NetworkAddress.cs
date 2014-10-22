using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class NetworkAddress
	{
		public ushort NetworkNumber { get; private set; }

		public byte[] MacAddress { get; private set; }

		public NetworkAddress(ushort networkNumber, byte[] macAddress)
		{
			this.NetworkNumber = networkNumber;
			this.MacAddress = macAddress;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("NetworkNumber", 255, Value<ushort>.Schema),
			new FieldSchema("MacAddress", 255, Value<byte[]>.Schema));

		public static NetworkAddress Load(IValueStream stream)
		{
			stream.EnterSequence();
			var networkNumber = Value<ushort>.Load(stream);
			var macAddress = Value<byte[]>.Load(stream);
			stream.LeaveSequence();
			return new NetworkAddress(networkNumber, macAddress);
		}

		public static void Save(IValueSink sink, NetworkAddress value)
		{
			sink.EnterSequence();
			Value<ushort>.Save(sink, value.NetworkNumber);
			Value<byte[]>.Save(sink, value.MacAddress);
			sink.LeaveSequence();
		}
	}
}
