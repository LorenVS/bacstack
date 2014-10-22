using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class VTDataRequest
	{
		public byte VtSessionIdentifier { get; private set; }

		public byte[] VtNewData { get; private set; }

		public uint VtDataFlag { get; private set; }

		public VTDataRequest(byte vtSessionIdentifier, byte[] vtNewData, uint vtDataFlag)
		{
			this.VtSessionIdentifier = vtSessionIdentifier;
			this.VtNewData = vtNewData;
			this.VtDataFlag = vtDataFlag;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("VtSessionIdentifier", 255, Value<byte>.Schema),
			new FieldSchema("VtNewData", 255, Value<byte[]>.Schema),
			new FieldSchema("VtDataFlag", 255, Value<uint>.Schema));

		public static VTDataRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var vtSessionIdentifier = Value<byte>.Load(stream);
			var vtNewData = Value<byte[]>.Load(stream);
			var vtDataFlag = Value<uint>.Load(stream);
			stream.LeaveSequence();
			return new VTDataRequest(vtSessionIdentifier, vtNewData, vtDataFlag);
		}

		public static void Save(IValueSink sink, VTDataRequest value)
		{
			sink.EnterSequence();
			Value<byte>.Save(sink, value.VtSessionIdentifier);
			Value<byte[]>.Save(sink, value.VtNewData);
			Value<uint>.Save(sink, value.VtDataFlag);
			sink.LeaveSequence();
		}
	}
}
