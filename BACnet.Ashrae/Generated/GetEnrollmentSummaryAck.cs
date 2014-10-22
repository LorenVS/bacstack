using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class GetEnrollmentSummaryAck
	{
		public ReadOnlyArray<Element> Item { get; private set; }

		public GetEnrollmentSummaryAck(ReadOnlyArray<Element> item)
		{
			this.Item = item;
		}

		public static readonly ISchema Schema = Value<ReadOnlyArray<Element>>.Schema;

		public static GetEnrollmentSummaryAck Load(IValueStream stream)
		{
			var temp = Value<ReadOnlyArray<Element>>.Load(stream);
			return new GetEnrollmentSummaryAck(temp);
		}

		public static void Save(IValueSink sink, GetEnrollmentSummaryAck value)
		{
			Value<ReadOnlyArray<Element>>.Save(sink, value.Item);
		}

		public  partial class Element
		{
			public ObjectId ObjectIdentifier { get; private set; }

			public EventType EventType { get; private set; }

			public EventState EventState { get; private set; }

			public byte Priority { get; private set; }

			public Option<uint> NotificationClass { get; private set; }

			public Element(ObjectId objectIdentifier, EventType eventType, EventState eventState, byte priority, Option<uint> notificationClass)
			{
				this.ObjectIdentifier = objectIdentifier;
				this.EventType = eventType;
				this.EventState = eventState;
				this.Priority = priority;
				this.NotificationClass = notificationClass;
			}

			public static readonly ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("ObjectIdentifier", 255, Value<ObjectId>.Schema),
				new FieldSchema("EventType", 255, Value<EventType>.Schema),
				new FieldSchema("EventState", 255, Value<EventState>.Schema),
				new FieldSchema("Priority", 255, Value<byte>.Schema),
				new FieldSchema("NotificationClass", 255, Value<Option<uint>>.Schema));

			public static Element Load(IValueStream stream)
			{
				stream.EnterSequence();
				var objectIdentifier = Value<ObjectId>.Load(stream);
				var eventType = Value<EventType>.Load(stream);
				var eventState = Value<EventState>.Load(stream);
				var priority = Value<byte>.Load(stream);
				var notificationClass = Value<Option<uint>>.Load(stream);
				stream.LeaveSequence();
				return new Element(objectIdentifier, eventType, eventState, priority, notificationClass);
			}

			public static void Save(IValueSink sink, Element value)
			{
				sink.EnterSequence();
				Value<ObjectId>.Save(sink, value.ObjectIdentifier);
				Value<EventType>.Save(sink, value.EventType);
				Value<EventState>.Save(sink, value.EventState);
				Value<byte>.Save(sink, value.Priority);
				Value<Option<uint>>.Save(sink, value.NotificationClass);
				sink.LeaveSequence();
			}
		}
	}
}
