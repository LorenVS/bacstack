using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class SessionKey
	{
		public byte[] Key { get; private set; }

		public NetworkAddress PeerAddress { get; private set; }

		public SessionKey(byte[] key, NetworkAddress peerAddress)
		{
			this.Key = key;
			this.PeerAddress = peerAddress;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("Key", 255, Value<byte[]>.Schema),
			new FieldSchema("PeerAddress", 255, Value<NetworkAddress>.Schema));

		public static SessionKey Load(IValueStream stream)
		{
			stream.EnterSequence();
			var key = Value<byte[]>.Load(stream);
			var peerAddress = Value<NetworkAddress>.Load(stream);
			stream.LeaveSequence();
			return new SessionKey(key, peerAddress);
		}

		public static void Save(IValueSink sink, SessionKey value)
		{
			sink.EnterSequence();
			Value<byte[]>.Save(sink, value.Key);
			Value<NetworkAddress>.Save(sink, value.PeerAddress);
			sink.LeaveSequence();
		}
	}
}
