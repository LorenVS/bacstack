using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class GetEventInformationRequest
	{
		public Option<ObjectId> LastReceivedObjectIdentifier { get; private set; }

		public GetEventInformationRequest(Option<ObjectId> lastReceivedObjectIdentifier)
		{
			this.LastReceivedObjectIdentifier = lastReceivedObjectIdentifier;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("LastReceivedObjectIdentifier", 0, Value<Option<ObjectId>>.Schema));

		public static GetEventInformationRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var lastReceivedObjectIdentifier = Value<Option<ObjectId>>.Load(stream);
			stream.LeaveSequence();
			return new GetEventInformationRequest(lastReceivedObjectIdentifier);
		}

		public static void Save(IValueSink sink, GetEventInformationRequest value)
		{
			sink.EnterSequence();
			Value<Option<ObjectId>>.Save(sink, value.LastReceivedObjectIdentifier);
			sink.LeaveSequence();
		}
	}
}
