using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class GetEventInformationAck
	{
		public ReadOnlyArray<ListOfEventSummariesType> ListOfEventSummaries { get; private set; }

		public bool MoreEvents { get; private set; }

		public GetEventInformationAck(ReadOnlyArray<ListOfEventSummariesType> listOfEventSummaries, bool moreEvents)
		{
			this.ListOfEventSummaries = listOfEventSummaries;
			this.MoreEvents = moreEvents;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ListOfEventSummaries", 0, Value<ReadOnlyArray<ListOfEventSummariesType>>.Schema),
			new FieldSchema("MoreEvents", 1, Value<bool>.Schema));

		public static GetEventInformationAck Load(IValueStream stream)
		{
			stream.EnterSequence();
			var listOfEventSummaries = Value<ReadOnlyArray<ListOfEventSummariesType>>.Load(stream);
			var moreEvents = Value<bool>.Load(stream);
			stream.LeaveSequence();
			return new GetEventInformationAck(listOfEventSummaries, moreEvents);
		}

		public static void Save(IValueSink sink, GetEventInformationAck value)
		{
			sink.EnterSequence();
			Value<ReadOnlyArray<ListOfEventSummariesType>>.Save(sink, value.ListOfEventSummaries);
			Value<bool>.Save(sink, value.MoreEvents);
			sink.LeaveSequence();
		}
		public  partial class ListOfEventSummariesType
		{
			public ObjectId ObjectIdentifier { get; private set; }

			public EventState EventState { get; private set; }

			public EventTransitionBits AcknowledgedTransitions { get; private set; }

			public ReadOnlyArray<TimeStamp> EventTimeStamps { get; private set; }

			public NotifyType NotifyType { get; private set; }

			public EventTransitionBits EventEnable { get; private set; }

			public ReadOnlyArray<uint> EventPriorities { get; private set; }

			public ListOfEventSummariesType(ObjectId objectIdentifier, EventState eventState, EventTransitionBits acknowledgedTransitions, ReadOnlyArray<TimeStamp> eventTimeStamps, NotifyType notifyType, EventTransitionBits eventEnable, ReadOnlyArray<uint> eventPriorities)
			{
				this.ObjectIdentifier = objectIdentifier;
				this.EventState = eventState;
				this.AcknowledgedTransitions = acknowledgedTransitions;
				this.EventTimeStamps = eventTimeStamps;
				this.NotifyType = notifyType;
				this.EventEnable = eventEnable;
				this.EventPriorities = eventPriorities;
			}

			public static readonly ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("ObjectIdentifier", 0, Value<ObjectId>.Schema),
				new FieldSchema("EventState", 1, Value<EventState>.Schema),
				new FieldSchema("AcknowledgedTransitions", 2, Value<EventTransitionBits>.Schema),
				new FieldSchema("EventTimeStamps", 3, Value<ReadOnlyArray<TimeStamp>>.Schema),
				new FieldSchema("NotifyType", 4, Value<NotifyType>.Schema),
				new FieldSchema("EventEnable", 5, Value<EventTransitionBits>.Schema),
				new FieldSchema("EventPriorities", 6, Value<ReadOnlyArray<uint>>.Schema));

			public static ListOfEventSummariesType Load(IValueStream stream)
			{
				stream.EnterSequence();
				var objectIdentifier = Value<ObjectId>.Load(stream);
				var eventState = Value<EventState>.Load(stream);
				var acknowledgedTransitions = Value<EventTransitionBits>.Load(stream);
				var eventTimeStamps = Value<ReadOnlyArray<TimeStamp>>.Load(stream);
				var notifyType = Value<NotifyType>.Load(stream);
				var eventEnable = Value<EventTransitionBits>.Load(stream);
				var eventPriorities = Value<ReadOnlyArray<uint>>.Load(stream);
				stream.LeaveSequence();
				return new ListOfEventSummariesType(objectIdentifier, eventState, acknowledgedTransitions, eventTimeStamps, notifyType, eventEnable, eventPriorities);
			}

			public static void Save(IValueSink sink, ListOfEventSummariesType value)
			{
				sink.EnterSequence();
				Value<ObjectId>.Save(sink, value.ObjectIdentifier);
				Value<EventState>.Save(sink, value.EventState);
				Value<EventTransitionBits>.Save(sink, value.AcknowledgedTransitions);
				Value<ReadOnlyArray<TimeStamp>>.Save(sink, value.EventTimeStamps);
				Value<NotifyType>.Save(sink, value.NotifyType);
				Value<EventTransitionBits>.Save(sink, value.EventEnable);
				Value<ReadOnlyArray<uint>>.Save(sink, value.EventPriorities);
				sink.LeaveSequence();
			}
		}
	}
}
