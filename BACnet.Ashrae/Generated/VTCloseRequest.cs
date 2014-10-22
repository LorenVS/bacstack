using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class VTCloseRequest
	{
		public ReadOnlyArray<byte> ListOfRemoteVTSessionIdentifiers { get; private set; }

		public VTCloseRequest(ReadOnlyArray<byte> listOfRemoteVTSessionIdentifiers)
		{
			this.ListOfRemoteVTSessionIdentifiers = listOfRemoteVTSessionIdentifiers;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ListOfRemoteVTSessionIdentifiers", 255, Value<ReadOnlyArray<byte>>.Schema));

		public static VTCloseRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var listOfRemoteVTSessionIdentifiers = Value<ReadOnlyArray<byte>>.Load(stream);
			stream.LeaveSequence();
			return new VTCloseRequest(listOfRemoteVTSessionIdentifiers);
		}

		public static void Save(IValueSink sink, VTCloseRequest value)
		{
			sink.EnterSequence();
			Value<ReadOnlyArray<byte>>.Save(sink, value.ListOfRemoteVTSessionIdentifiers);
			sink.LeaveSequence();
		}
	}
}
