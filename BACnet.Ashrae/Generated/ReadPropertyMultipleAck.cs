using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class ReadPropertyMultipleAck
	{
		public ReadOnlyArray<ReadAccessResult> ListOfReadAccessResults { get; private set; }

		public ReadPropertyMultipleAck(ReadOnlyArray<ReadAccessResult> listOfReadAccessResults)
		{
			this.ListOfReadAccessResults = listOfReadAccessResults;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ListOfReadAccessResults", 255, Value<ReadOnlyArray<ReadAccessResult>>.Schema));

		public static ReadPropertyMultipleAck Load(IValueStream stream)
		{
			stream.EnterSequence();
			var listOfReadAccessResults = Value<ReadOnlyArray<ReadAccessResult>>.Load(stream);
			stream.LeaveSequence();
			return new ReadPropertyMultipleAck(listOfReadAccessResults);
		}

		public static void Save(IValueSink sink, ReadPropertyMultipleAck value)
		{
			sink.EnterSequence();
			Value<ReadOnlyArray<ReadAccessResult>>.Save(sink, value.ListOfReadAccessResults);
			sink.LeaveSequence();
		}
	}
}
