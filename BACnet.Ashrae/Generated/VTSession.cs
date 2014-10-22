using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class VTSession
	{
		public byte LocalVtSessionID { get; private set; }

		public byte RemoteVtSessionID { get; private set; }

		public NetworkAddress RemoteVtAddress { get; private set; }

		public VTSession(byte localVtSessionID, byte remoteVtSessionID, NetworkAddress remoteVtAddress)
		{
			this.LocalVtSessionID = localVtSessionID;
			this.RemoteVtSessionID = remoteVtSessionID;
			this.RemoteVtAddress = remoteVtAddress;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("LocalVtSessionID", 255, Value<byte>.Schema),
			new FieldSchema("RemoteVtSessionID", 255, Value<byte>.Schema),
			new FieldSchema("RemoteVtAddress", 255, Value<NetworkAddress>.Schema));

		public static VTSession Load(IValueStream stream)
		{
			stream.EnterSequence();
			var localVtSessionID = Value<byte>.Load(stream);
			var remoteVtSessionID = Value<byte>.Load(stream);
			var remoteVtAddress = Value<NetworkAddress>.Load(stream);
			stream.LeaveSequence();
			return new VTSession(localVtSessionID, remoteVtSessionID, remoteVtAddress);
		}

		public static void Save(IValueSink sink, VTSession value)
		{
			sink.EnterSequence();
			Value<byte>.Save(sink, value.LocalVtSessionID);
			Value<byte>.Save(sink, value.RemoteVtSessionID);
			Value<NetworkAddress>.Save(sink, value.RemoteVtAddress);
			sink.LeaveSequence();
		}
	}
}
