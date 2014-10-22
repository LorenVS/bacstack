using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class WritePropertyMultipleRequest
	{
		public ReadOnlyArray<WriteAccessSpecification> ListOfwriteAccessSpecifications { get; private set; }

		public WritePropertyMultipleRequest(ReadOnlyArray<WriteAccessSpecification> listOfwriteAccessSpecifications)
		{
			this.ListOfwriteAccessSpecifications = listOfwriteAccessSpecifications;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ListOfwriteAccessSpecifications", 255, Value<ReadOnlyArray<WriteAccessSpecification>>.Schema));

		public static WritePropertyMultipleRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var listOfwriteAccessSpecifications = Value<ReadOnlyArray<WriteAccessSpecification>>.Load(stream);
			stream.LeaveSequence();
			return new WritePropertyMultipleRequest(listOfwriteAccessSpecifications);
		}

		public static void Save(IValueSink sink, WritePropertyMultipleRequest value)
		{
			sink.EnterSequence();
			Value<ReadOnlyArray<WriteAccessSpecification>>.Save(sink, value.ListOfwriteAccessSpecifications);
			sink.LeaveSequence();
		}
	}
}
