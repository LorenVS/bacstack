using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class SetpointReference
	{
		public Option<ObjectPropertyReference> Reference { get; private set; }

		public SetpointReference(Option<ObjectPropertyReference> reference)
		{
			this.Reference = reference;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("Reference", 0, Value<Option<ObjectPropertyReference>>.Schema));

		public static SetpointReference Load(IValueStream stream)
		{
			stream.EnterSequence();
			var reference = Value<Option<ObjectPropertyReference>>.Load(stream);
			stream.LeaveSequence();
			return new SetpointReference(reference);
		}

		public static void Save(IValueSink sink, SetpointReference value)
		{
			sink.EnterSequence();
			Value<Option<ObjectPropertyReference>>.Save(sink, value.Reference);
			sink.LeaveSequence();
		}
	}
}
