using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class SubscribeCOVPropertyRequest
	{
		public uint SubscriberProcessIdentifier { get; private set; }

		public ObjectId MonitoredObjectIdentifier { get; private set; }

		public Option<bool> IssueConfirmedNotifications { get; private set; }

		public Option<uint> Lifetime { get; private set; }

		public PropertyReference MonitoredPropertyIdentifier { get; private set; }

		public Option<float> CovIncrement { get; private set; }

		public SubscribeCOVPropertyRequest(uint subscriberProcessIdentifier, ObjectId monitoredObjectIdentifier, Option<bool> issueConfirmedNotifications, Option<uint> lifetime, PropertyReference monitoredPropertyIdentifier, Option<float> covIncrement)
		{
			this.SubscriberProcessIdentifier = subscriberProcessIdentifier;
			this.MonitoredObjectIdentifier = monitoredObjectIdentifier;
			this.IssueConfirmedNotifications = issueConfirmedNotifications;
			this.Lifetime = lifetime;
			this.MonitoredPropertyIdentifier = monitoredPropertyIdentifier;
			this.CovIncrement = covIncrement;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("SubscriberProcessIdentifier", 0, Value<uint>.Schema),
			new FieldSchema("MonitoredObjectIdentifier", 1, Value<ObjectId>.Schema),
			new FieldSchema("IssueConfirmedNotifications", 2, Value<Option<bool>>.Schema),
			new FieldSchema("Lifetime", 3, Value<Option<uint>>.Schema),
			new FieldSchema("MonitoredPropertyIdentifier", 4, Value<PropertyReference>.Schema),
			new FieldSchema("CovIncrement", 5, Value<Option<float>>.Schema));

		public static SubscribeCOVPropertyRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var subscriberProcessIdentifier = Value<uint>.Load(stream);
			var monitoredObjectIdentifier = Value<ObjectId>.Load(stream);
			var issueConfirmedNotifications = Value<Option<bool>>.Load(stream);
			var lifetime = Value<Option<uint>>.Load(stream);
			var monitoredPropertyIdentifier = Value<PropertyReference>.Load(stream);
			var covIncrement = Value<Option<float>>.Load(stream);
			stream.LeaveSequence();
			return new SubscribeCOVPropertyRequest(subscriberProcessIdentifier, monitoredObjectIdentifier, issueConfirmedNotifications, lifetime, monitoredPropertyIdentifier, covIncrement);
		}

		public static void Save(IValueSink sink, SubscribeCOVPropertyRequest value)
		{
			sink.EnterSequence();
			Value<uint>.Save(sink, value.SubscriberProcessIdentifier);
			Value<ObjectId>.Save(sink, value.MonitoredObjectIdentifier);
			Value<Option<bool>>.Save(sink, value.IssueConfirmedNotifications);
			Value<Option<uint>>.Save(sink, value.Lifetime);
			Value<PropertyReference>.Save(sink, value.MonitoredPropertyIdentifier);
			Value<Option<float>>.Save(sink, value.CovIncrement);
			sink.LeaveSequence();
		}
	}
}
