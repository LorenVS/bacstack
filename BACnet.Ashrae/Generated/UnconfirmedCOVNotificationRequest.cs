using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class UnconfirmedCOVNotificationRequest
	{
		public uint SubscriberProcessIdentifier { get; private set; }

		public ObjectId InitiatingDeviceIdentifier { get; private set; }

		public ObjectId MonitoredObjectIdentifier { get; private set; }

		public uint TimeRemaining { get; private set; }

		public ReadOnlyArray<PropertyValue> ListOfValues { get; private set; }

		public UnconfirmedCOVNotificationRequest(uint subscriberProcessIdentifier, ObjectId initiatingDeviceIdentifier, ObjectId monitoredObjectIdentifier, uint timeRemaining, ReadOnlyArray<PropertyValue> listOfValues)
		{
			this.SubscriberProcessIdentifier = subscriberProcessIdentifier;
			this.InitiatingDeviceIdentifier = initiatingDeviceIdentifier;
			this.MonitoredObjectIdentifier = monitoredObjectIdentifier;
			this.TimeRemaining = timeRemaining;
			this.ListOfValues = listOfValues;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("SubscriberProcessIdentifier", 0, Value<uint>.Schema),
			new FieldSchema("InitiatingDeviceIdentifier", 1, Value<ObjectId>.Schema),
			new FieldSchema("MonitoredObjectIdentifier", 2, Value<ObjectId>.Schema),
			new FieldSchema("TimeRemaining", 3, Value<uint>.Schema),
			new FieldSchema("ListOfValues", 4, Value<ReadOnlyArray<PropertyValue>>.Schema));

		public static UnconfirmedCOVNotificationRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var subscriberProcessIdentifier = Value<uint>.Load(stream);
			var initiatingDeviceIdentifier = Value<ObjectId>.Load(stream);
			var monitoredObjectIdentifier = Value<ObjectId>.Load(stream);
			var timeRemaining = Value<uint>.Load(stream);
			var listOfValues = Value<ReadOnlyArray<PropertyValue>>.Load(stream);
			stream.LeaveSequence();
			return new UnconfirmedCOVNotificationRequest(subscriberProcessIdentifier, initiatingDeviceIdentifier, monitoredObjectIdentifier, timeRemaining, listOfValues);
		}

		public static void Save(IValueSink sink, UnconfirmedCOVNotificationRequest value)
		{
			sink.EnterSequence();
			Value<uint>.Save(sink, value.SubscriberProcessIdentifier);
			Value<ObjectId>.Save(sink, value.InitiatingDeviceIdentifier);
			Value<ObjectId>.Save(sink, value.MonitoredObjectIdentifier);
			Value<uint>.Save(sink, value.TimeRemaining);
			Value<ReadOnlyArray<PropertyValue>>.Save(sink, value.ListOfValues);
			sink.LeaveSequence();
		}
	}
}
