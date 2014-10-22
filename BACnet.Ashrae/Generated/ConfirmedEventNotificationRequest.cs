using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class ConfirmedEventNotificationRequest
	{
		public uint ProcessIdentifier { get; private set; }

		public ObjectId InitiatingDeviceIdentifier { get; private set; }

		public ObjectId EventObjectIdentifier { get; private set; }

		public TimeStamp TimeStamp { get; private set; }

		public uint NotificationClass { get; private set; }

		public byte Priority { get; private set; }

		public EventType EventType { get; private set; }

		public Option<string> MessageText { get; private set; }

		public NotifyType NotifyType { get; private set; }

		public Option<bool> AckRequired { get; private set; }

		public Option<EventState> FromState { get; private set; }

		public EventState ToState { get; private set; }

		public Option<NotificationParameters> EventValues { get; private set; }

		public ConfirmedEventNotificationRequest(uint processIdentifier, ObjectId initiatingDeviceIdentifier, ObjectId eventObjectIdentifier, TimeStamp timeStamp, uint notificationClass, byte priority, EventType eventType, Option<string> messageText, NotifyType notifyType, Option<bool> ackRequired, Option<EventState> fromState, EventState toState, Option<NotificationParameters> eventValues)
		{
			this.ProcessIdentifier = processIdentifier;
			this.InitiatingDeviceIdentifier = initiatingDeviceIdentifier;
			this.EventObjectIdentifier = eventObjectIdentifier;
			this.TimeStamp = timeStamp;
			this.NotificationClass = notificationClass;
			this.Priority = priority;
			this.EventType = eventType;
			this.MessageText = messageText;
			this.NotifyType = notifyType;
			this.AckRequired = ackRequired;
			this.FromState = fromState;
			this.ToState = toState;
			this.EventValues = eventValues;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ProcessIdentifier", 0, Value<uint>.Schema),
			new FieldSchema("InitiatingDeviceIdentifier", 1, Value<ObjectId>.Schema),
			new FieldSchema("EventObjectIdentifier", 2, Value<ObjectId>.Schema),
			new FieldSchema("TimeStamp", 3, Value<TimeStamp>.Schema),
			new FieldSchema("NotificationClass", 4, Value<uint>.Schema),
			new FieldSchema("Priority", 5, Value<byte>.Schema),
			new FieldSchema("EventType", 6, Value<EventType>.Schema),
			new FieldSchema("MessageText", 7, Value<Option<string>>.Schema),
			new FieldSchema("NotifyType", 8, Value<NotifyType>.Schema),
			new FieldSchema("AckRequired", 9, Value<Option<bool>>.Schema),
			new FieldSchema("FromState", 10, Value<Option<EventState>>.Schema),
			new FieldSchema("ToState", 11, Value<EventState>.Schema),
			new FieldSchema("EventValues", 12, Value<Option<NotificationParameters>>.Schema));

		public static ConfirmedEventNotificationRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var processIdentifier = Value<uint>.Load(stream);
			var initiatingDeviceIdentifier = Value<ObjectId>.Load(stream);
			var eventObjectIdentifier = Value<ObjectId>.Load(stream);
			var timeStamp = Value<TimeStamp>.Load(stream);
			var notificationClass = Value<uint>.Load(stream);
			var priority = Value<byte>.Load(stream);
			var eventType = Value<EventType>.Load(stream);
			var messageText = Value<Option<string>>.Load(stream);
			var notifyType = Value<NotifyType>.Load(stream);
			var ackRequired = Value<Option<bool>>.Load(stream);
			var fromState = Value<Option<EventState>>.Load(stream);
			var toState = Value<EventState>.Load(stream);
			var eventValues = Value<Option<NotificationParameters>>.Load(stream);
			stream.LeaveSequence();
			return new ConfirmedEventNotificationRequest(processIdentifier, initiatingDeviceIdentifier, eventObjectIdentifier, timeStamp, notificationClass, priority, eventType, messageText, notifyType, ackRequired, fromState, toState, eventValues);
		}

		public static void Save(IValueSink sink, ConfirmedEventNotificationRequest value)
		{
			sink.EnterSequence();
			Value<uint>.Save(sink, value.ProcessIdentifier);
			Value<ObjectId>.Save(sink, value.InitiatingDeviceIdentifier);
			Value<ObjectId>.Save(sink, value.EventObjectIdentifier);
			Value<TimeStamp>.Save(sink, value.TimeStamp);
			Value<uint>.Save(sink, value.NotificationClass);
			Value<byte>.Save(sink, value.Priority);
			Value<EventType>.Save(sink, value.EventType);
			Value<Option<string>>.Save(sink, value.MessageText);
			Value<NotifyType>.Save(sink, value.NotifyType);
			Value<Option<bool>>.Save(sink, value.AckRequired);
			Value<Option<EventState>>.Save(sink, value.FromState);
			Value<EventState>.Save(sink, value.ToState);
			Value<Option<NotificationParameters>>.Save(sink, value.EventValues);
			sink.LeaveSequence();
		}
	}
}
