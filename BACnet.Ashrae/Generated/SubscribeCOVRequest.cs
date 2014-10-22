using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class SubscribeCOVRequest
	{
		public uint SubscriberProcessIdentifier { get; private set; }

		public ObjectId MonitoredObjectIdentifier { get; private set; }

		public Option<bool> IssueConfirmedNotifications { get; private set; }

		public Option<uint> Lifetime { get; private set; }

		public SubscribeCOVRequest(uint subscriberProcessIdentifier, ObjectId monitoredObjectIdentifier, Option<bool> issueConfirmedNotifications, Option<uint> lifetime)
		{
			this.SubscriberProcessIdentifier = subscriberProcessIdentifier;
			this.MonitoredObjectIdentifier = monitoredObjectIdentifier;
			this.IssueConfirmedNotifications = issueConfirmedNotifications;
			this.Lifetime = lifetime;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("SubscriberProcessIdentifier", 0, Value<uint>.Schema),
			new FieldSchema("MonitoredObjectIdentifier", 1, Value<ObjectId>.Schema),
			new FieldSchema("IssueConfirmedNotifications", 2, Value<Option<bool>>.Schema),
			new FieldSchema("Lifetime", 3, Value<Option<uint>>.Schema));

		public static SubscribeCOVRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var subscriberProcessIdentifier = Value<uint>.Load(stream);
			var monitoredObjectIdentifier = Value<ObjectId>.Load(stream);
			var issueConfirmedNotifications = Value<Option<bool>>.Load(stream);
			var lifetime = Value<Option<uint>>.Load(stream);
			stream.LeaveSequence();
			return new SubscribeCOVRequest(subscriberProcessIdentifier, monitoredObjectIdentifier, issueConfirmedNotifications, lifetime);
		}

		public static void Save(IValueSink sink, SubscribeCOVRequest value)
		{
			sink.EnterSequence();
			Value<uint>.Save(sink, value.SubscriberProcessIdentifier);
			Value<ObjectId>.Save(sink, value.MonitoredObjectIdentifier);
			Value<Option<bool>>.Save(sink, value.IssueConfirmedNotifications);
			Value<Option<uint>>.Save(sink, value.Lifetime);
			sink.LeaveSequence();
		}
	}
}
