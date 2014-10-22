using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class VTCloseError
	{
		public Error ErrorType { get; private set; }

		public Option<ReadOnlyArray<byte>> ListOfVTSessionIdentifiers { get; private set; }

		public VTCloseError(Error errorType, Option<ReadOnlyArray<byte>> listOfVTSessionIdentifiers)
		{
			this.ErrorType = errorType;
			this.ListOfVTSessionIdentifiers = listOfVTSessionIdentifiers;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ErrorType", 0, Value<Error>.Schema),
			new FieldSchema("ListOfVTSessionIdentifiers", 1, Value<Option<ReadOnlyArray<byte>>>.Schema));

		public static VTCloseError Load(IValueStream stream)
		{
			stream.EnterSequence();
			var errorType = Value<Error>.Load(stream);
			var listOfVTSessionIdentifiers = Value<Option<ReadOnlyArray<byte>>>.Load(stream);
			stream.LeaveSequence();
			return new VTCloseError(errorType, listOfVTSessionIdentifiers);
		}

		public static void Save(IValueSink sink, VTCloseError value)
		{
			sink.EnterSequence();
			Value<Error>.Save(sink, value.ErrorType);
			Value<Option<ReadOnlyArray<byte>>>.Save(sink, value.ListOfVTSessionIdentifiers);
			sink.LeaveSequence();
		}
	}
}
