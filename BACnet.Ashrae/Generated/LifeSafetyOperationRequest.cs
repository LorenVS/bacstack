using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class LifeSafetyOperationRequest
	{
		public uint RequestingProcessIdentifier { get; private set; }

		public string RequestingSource { get; private set; }

		public LifeSafetyOperation Request { get; private set; }

		public Option<ObjectId> ObjectIdentifier { get; private set; }

		public LifeSafetyOperationRequest(uint requestingProcessIdentifier, string requestingSource, LifeSafetyOperation request, Option<ObjectId> objectIdentifier)
		{
			this.RequestingProcessIdentifier = requestingProcessIdentifier;
			this.RequestingSource = requestingSource;
			this.Request = request;
			this.ObjectIdentifier = objectIdentifier;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("RequestingProcessIdentifier", 0, Value<uint>.Schema),
			new FieldSchema("RequestingSource", 1, Value<string>.Schema),
			new FieldSchema("Request", 2, Value<LifeSafetyOperation>.Schema),
			new FieldSchema("ObjectIdentifier", 3, Value<Option<ObjectId>>.Schema));

		public static LifeSafetyOperationRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var requestingProcessIdentifier = Value<uint>.Load(stream);
			var requestingSource = Value<string>.Load(stream);
			var request = Value<LifeSafetyOperation>.Load(stream);
			var objectIdentifier = Value<Option<ObjectId>>.Load(stream);
			stream.LeaveSequence();
			return new LifeSafetyOperationRequest(requestingProcessIdentifier, requestingSource, request, objectIdentifier);
		}

		public static void Save(IValueSink sink, LifeSafetyOperationRequest value)
		{
			sink.EnterSequence();
			Value<uint>.Save(sink, value.RequestingProcessIdentifier);
			Value<string>.Save(sink, value.RequestingSource);
			Value<LifeSafetyOperation>.Save(sink, value.Request);
			Value<Option<ObjectId>>.Save(sink, value.ObjectIdentifier);
			sink.LeaveSequence();
		}
	}
}
