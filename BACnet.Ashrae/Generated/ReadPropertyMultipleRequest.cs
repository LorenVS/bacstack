using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class ReadPropertyMultipleRequest
	{
		public ReadOnlyArray<ReadAccessSpecification> ListOfReadAccessSpecs { get; private set; }

		public ReadPropertyMultipleRequest(ReadOnlyArray<ReadAccessSpecification> listOfReadAccessSpecs)
		{
			this.ListOfReadAccessSpecs = listOfReadAccessSpecs;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ListOfReadAccessSpecs", 255, Value<ReadOnlyArray<ReadAccessSpecification>>.Schema));

		public static ReadPropertyMultipleRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var listOfReadAccessSpecs = Value<ReadOnlyArray<ReadAccessSpecification>>.Load(stream);
			stream.LeaveSequence();
			return new ReadPropertyMultipleRequest(listOfReadAccessSpecs);
		}

		public static void Save(IValueSink sink, ReadPropertyMultipleRequest value)
		{
			sink.EnterSequence();
			Value<ReadOnlyArray<ReadAccessSpecification>>.Save(sink, value.ListOfReadAccessSpecs);
			sink.LeaveSequence();
		}
	}
}
