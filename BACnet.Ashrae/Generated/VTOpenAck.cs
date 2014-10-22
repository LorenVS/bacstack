using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class VTOpenAck
	{
		public byte RemoteVTSessionIdentifier { get; private set; }

		public VTOpenAck(byte remoteVTSessionIdentifier)
		{
			this.RemoteVTSessionIdentifier = remoteVTSessionIdentifier;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("RemoteVTSessionIdentifier", 255, Value<byte>.Schema));

		public static VTOpenAck Load(IValueStream stream)
		{
			stream.EnterSequence();
			var remoteVTSessionIdentifier = Value<byte>.Load(stream);
			stream.LeaveSequence();
			return new VTOpenAck(remoteVTSessionIdentifier);
		}

		public static void Save(IValueSink sink, VTOpenAck value)
		{
			sink.EnterSequence();
			Value<byte>.Save(sink, value.RemoteVTSessionIdentifier);
			sink.LeaveSequence();
		}
	}
}
