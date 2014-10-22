using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class VTDataAck
	{
		public bool AllNewDataAccepted { get; private set; }

		public Option<uint> AcceptedOctetCount { get; private set; }

		public VTDataAck(bool allNewDataAccepted, Option<uint> acceptedOctetCount)
		{
			this.AllNewDataAccepted = allNewDataAccepted;
			this.AcceptedOctetCount = acceptedOctetCount;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("AllNewDataAccepted", 0, Value<bool>.Schema),
			new FieldSchema("AcceptedOctetCount", 1, Value<Option<uint>>.Schema));

		public static VTDataAck Load(IValueStream stream)
		{
			stream.EnterSequence();
			var allNewDataAccepted = Value<bool>.Load(stream);
			var acceptedOctetCount = Value<Option<uint>>.Load(stream);
			stream.LeaveSequence();
			return new VTDataAck(allNewDataAccepted, acceptedOctetCount);
		}

		public static void Save(IValueSink sink, VTDataAck value)
		{
			sink.EnterSequence();
			Value<bool>.Save(sink, value.AllNewDataAccepted);
			Value<Option<uint>>.Save(sink, value.AcceptedOctetCount);
			sink.LeaveSequence();
		}
	}
}
