using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class RecipientProcess
	{
		public Recipient Recipient { get; private set; }

		public uint ProcessIdentifier { get; private set; }

		public RecipientProcess(Recipient recipient, uint processIdentifier)
		{
			this.Recipient = recipient;
			this.ProcessIdentifier = processIdentifier;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("Recipient", 0, Value<Recipient>.Schema),
			new FieldSchema("ProcessIdentifier", 1, Value<uint>.Schema));

		public static RecipientProcess Load(IValueStream stream)
		{
			stream.EnterSequence();
			var recipient = Value<Recipient>.Load(stream);
			var processIdentifier = Value<uint>.Load(stream);
			stream.LeaveSequence();
			return new RecipientProcess(recipient, processIdentifier);
		}

		public static void Save(IValueSink sink, RecipientProcess value)
		{
			sink.EnterSequence();
			Value<Recipient>.Save(sink, value.Recipient);
			Value<uint>.Save(sink, value.ProcessIdentifier);
			sink.LeaveSequence();
		}
	}
}
