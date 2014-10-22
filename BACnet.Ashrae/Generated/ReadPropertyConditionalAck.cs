using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class ReadPropertyConditionalAck
	{
		public Option<ReadOnlyArray<ReadAccessResult>> ListOfReadAccessResults { get; private set; }

		public ReadPropertyConditionalAck(Option<ReadOnlyArray<ReadAccessResult>> listOfReadAccessResults)
		{
			this.ListOfReadAccessResults = listOfReadAccessResults;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ListOfReadAccessResults", 255, Value<Option<ReadOnlyArray<ReadAccessResult>>>.Schema));

		public static ReadPropertyConditionalAck Load(IValueStream stream)
		{
			stream.EnterSequence();
			var listOfReadAccessResults = Value<Option<ReadOnlyArray<ReadAccessResult>>>.Load(stream);
			stream.LeaveSequence();
			return new ReadPropertyConditionalAck(listOfReadAccessResults);
		}

		public static void Save(IValueSink sink, ReadPropertyConditionalAck value)
		{
			sink.EnterSequence();
			Value<Option<ReadOnlyArray<ReadAccessResult>>>.Save(sink, value.ListOfReadAccessResults);
			sink.LeaveSequence();
		}
	}
}
