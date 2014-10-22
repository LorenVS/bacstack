using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class COVSubscription
	{
		public RecipientProcess Recipient { get; private set; }

		public ObjectPropertyReference MonitoredPropertyReference { get; private set; }

		public bool IssueConfirmedNotifications { get; private set; }

		public uint TimeRemaining { get; private set; }

		public Option<float> COVIncrement { get; private set; }

		public COVSubscription(RecipientProcess recipient, ObjectPropertyReference monitoredPropertyReference, bool issueConfirmedNotifications, uint timeRemaining, Option<float> cOVIncrement)
		{
			this.Recipient = recipient;
			this.MonitoredPropertyReference = monitoredPropertyReference;
			this.IssueConfirmedNotifications = issueConfirmedNotifications;
			this.TimeRemaining = timeRemaining;
			this.COVIncrement = cOVIncrement;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("Recipient", 0, Value<RecipientProcess>.Schema),
			new FieldSchema("MonitoredPropertyReference", 1, Value<ObjectPropertyReference>.Schema),
			new FieldSchema("IssueConfirmedNotifications", 2, Value<bool>.Schema),
			new FieldSchema("TimeRemaining", 3, Value<uint>.Schema),
			new FieldSchema("COVIncrement", 4, Value<Option<float>>.Schema));

		public static COVSubscription Load(IValueStream stream)
		{
			stream.EnterSequence();
			var recipient = Value<RecipientProcess>.Load(stream);
			var monitoredPropertyReference = Value<ObjectPropertyReference>.Load(stream);
			var issueConfirmedNotifications = Value<bool>.Load(stream);
			var timeRemaining = Value<uint>.Load(stream);
			var cOVIncrement = Value<Option<float>>.Load(stream);
			stream.LeaveSequence();
			return new COVSubscription(recipient, monitoredPropertyReference, issueConfirmedNotifications, timeRemaining, cOVIncrement);
		}

		public static void Save(IValueSink sink, COVSubscription value)
		{
			sink.EnterSequence();
			Value<RecipientProcess>.Save(sink, value.Recipient);
			Value<ObjectPropertyReference>.Save(sink, value.MonitoredPropertyReference);
			Value<bool>.Save(sink, value.IssueConfirmedNotifications);
			Value<uint>.Save(sink, value.TimeRemaining);
			Value<Option<float>>.Save(sink, value.COVIncrement);
			sink.LeaveSequence();
		}
	}
}
