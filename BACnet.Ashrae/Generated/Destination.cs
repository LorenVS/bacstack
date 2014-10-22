using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class Destination
	{
		public DaysOfWeek ValidDays { get; private set; }

		public Time FromTime { get; private set; }

		public Time ToTime { get; private set; }

		public Recipient Recipient { get; private set; }

		public uint ProcessIdentifier { get; private set; }

		public bool IssueConfirmedNotifications { get; private set; }

		public EventTransitionBits Transitions { get; private set; }

		public Destination(DaysOfWeek validDays, Time fromTime, Time toTime, Recipient recipient, uint processIdentifier, bool issueConfirmedNotifications, EventTransitionBits transitions)
		{
			this.ValidDays = validDays;
			this.FromTime = fromTime;
			this.ToTime = toTime;
			this.Recipient = recipient;
			this.ProcessIdentifier = processIdentifier;
			this.IssueConfirmedNotifications = issueConfirmedNotifications;
			this.Transitions = transitions;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ValidDays", 255, Value<DaysOfWeek>.Schema),
			new FieldSchema("FromTime", 255, Value<Time>.Schema),
			new FieldSchema("ToTime", 255, Value<Time>.Schema),
			new FieldSchema("Recipient", 255, Value<Recipient>.Schema),
			new FieldSchema("ProcessIdentifier", 255, Value<uint>.Schema),
			new FieldSchema("IssueConfirmedNotifications", 255, Value<bool>.Schema),
			new FieldSchema("Transitions", 255, Value<EventTransitionBits>.Schema));

		public static Destination Load(IValueStream stream)
		{
			stream.EnterSequence();
			var validDays = Value<DaysOfWeek>.Load(stream);
			var fromTime = Value<Time>.Load(stream);
			var toTime = Value<Time>.Load(stream);
			var recipient = Value<Recipient>.Load(stream);
			var processIdentifier = Value<uint>.Load(stream);
			var issueConfirmedNotifications = Value<bool>.Load(stream);
			var transitions = Value<EventTransitionBits>.Load(stream);
			stream.LeaveSequence();
			return new Destination(validDays, fromTime, toTime, recipient, processIdentifier, issueConfirmedNotifications, transitions);
		}

		public static void Save(IValueSink sink, Destination value)
		{
			sink.EnterSequence();
			Value<DaysOfWeek>.Save(sink, value.ValidDays);
			Value<Time>.Save(sink, value.FromTime);
			Value<Time>.Save(sink, value.ToTime);
			Value<Recipient>.Save(sink, value.Recipient);
			Value<uint>.Save(sink, value.ProcessIdentifier);
			Value<bool>.Save(sink, value.IssueConfirmedNotifications);
			Value<EventTransitionBits>.Save(sink, value.Transitions);
			sink.LeaveSequence();
		}
	}
}
