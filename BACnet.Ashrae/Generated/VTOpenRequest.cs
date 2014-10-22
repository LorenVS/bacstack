using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class VTOpenRequest
	{
		public VTClass VtClass { get; private set; }

		public byte LocalVTSessionIdentifier { get; private set; }

		public VTOpenRequest(VTClass vtClass, byte localVTSessionIdentifier)
		{
			this.VtClass = vtClass;
			this.LocalVTSessionIdentifier = localVTSessionIdentifier;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("VtClass", 255, Value<VTClass>.Schema),
			new FieldSchema("LocalVTSessionIdentifier", 255, Value<byte>.Schema));

		public static VTOpenRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var vtClass = Value<VTClass>.Load(stream);
			var localVTSessionIdentifier = Value<byte>.Load(stream);
			stream.LeaveSequence();
			return new VTOpenRequest(vtClass, localVTSessionIdentifier);
		}

		public static void Save(IValueSink sink, VTOpenRequest value)
		{
			sink.EnterSequence();
			Value<VTClass>.Save(sink, value.VtClass);
			Value<byte>.Save(sink, value.LocalVTSessionIdentifier);
			sink.LeaveSequence();
		}
	}
}
