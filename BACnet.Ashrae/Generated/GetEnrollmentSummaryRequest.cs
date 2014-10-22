using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class GetEnrollmentSummaryRequest
	{
		public AcknowledgmentFilterType AcknowledgmentFilter { get; private set; }

		public Option<RecipientProcess> EnrollmentFilter { get; private set; }

		public Option<EventStateFilterType> EventStateFilter { get; private set; }

		public Option<EventType> EventTypeFilter { get; private set; }

		public Option<PriorityFilterType> PriorityFilter { get; private set; }

		public Option<uint> NotificationClassFilter { get; private set; }

		public GetEnrollmentSummaryRequest(AcknowledgmentFilterType acknowledgmentFilter, Option<RecipientProcess> enrollmentFilter, Option<EventStateFilterType> eventStateFilter, Option<EventType> eventTypeFilter, Option<PriorityFilterType> priorityFilter, Option<uint> notificationClassFilter)
		{
			this.AcknowledgmentFilter = acknowledgmentFilter;
			this.EnrollmentFilter = enrollmentFilter;
			this.EventStateFilter = eventStateFilter;
			this.EventTypeFilter = eventTypeFilter;
			this.PriorityFilter = priorityFilter;
			this.NotificationClassFilter = notificationClassFilter;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("AcknowledgmentFilter", 0, Value<AcknowledgmentFilterType>.Schema),
			new FieldSchema("EnrollmentFilter", 1, Value<Option<RecipientProcess>>.Schema),
			new FieldSchema("EventStateFilter", 2, Value<Option<EventStateFilterType>>.Schema),
			new FieldSchema("EventTypeFilter", 3, Value<Option<EventType>>.Schema),
			new FieldSchema("PriorityFilter", 4, Value<Option<PriorityFilterType>>.Schema),
			new FieldSchema("NotificationClassFilter", 5, Value<Option<uint>>.Schema));

		public static GetEnrollmentSummaryRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var acknowledgmentFilter = Value<AcknowledgmentFilterType>.Load(stream);
			var enrollmentFilter = Value<Option<RecipientProcess>>.Load(stream);
			var eventStateFilter = Value<Option<EventStateFilterType>>.Load(stream);
			var eventTypeFilter = Value<Option<EventType>>.Load(stream);
			var priorityFilter = Value<Option<PriorityFilterType>>.Load(stream);
			var notificationClassFilter = Value<Option<uint>>.Load(stream);
			stream.LeaveSequence();
			return new GetEnrollmentSummaryRequest(acknowledgmentFilter, enrollmentFilter, eventStateFilter, eventTypeFilter, priorityFilter, notificationClassFilter);
		}

		public static void Save(IValueSink sink, GetEnrollmentSummaryRequest value)
		{
			sink.EnterSequence();
			Value<AcknowledgmentFilterType>.Save(sink, value.AcknowledgmentFilter);
			Value<Option<RecipientProcess>>.Save(sink, value.EnrollmentFilter);
			Value<Option<EventStateFilterType>>.Save(sink, value.EventStateFilter);
			Value<Option<EventType>>.Save(sink, value.EventTypeFilter);
			Value<Option<PriorityFilterType>>.Save(sink, value.PriorityFilter);
			Value<Option<uint>>.Save(sink, value.NotificationClassFilter);
			sink.LeaveSequence();
		}
		public enum AcknowledgmentFilterType : uint
		{
			All = 0,
			Acked = 1,
			NotAcked = 2
		}
		public enum EventStateFilterType : uint
		{
			Offnormal = 0,
			Fault = 1,
			Normal = 2,
			All = 3,
			Active = 4
		}
		public  partial class PriorityFilterType
		{
			public byte MinPriority { get; private set; }

			public byte MaxPriority { get; private set; }

			public PriorityFilterType(byte minPriority, byte maxPriority)
			{
				this.MinPriority = minPriority;
				this.MaxPriority = maxPriority;
			}

			public static readonly ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("MinPriority", 0, Value<byte>.Schema),
				new FieldSchema("MaxPriority", 1, Value<byte>.Schema));

			public static PriorityFilterType Load(IValueStream stream)
			{
				stream.EnterSequence();
				var minPriority = Value<byte>.Load(stream);
				var maxPriority = Value<byte>.Load(stream);
				stream.LeaveSequence();
				return new PriorityFilterType(minPriority, maxPriority);
			}

			public static void Save(IValueSink sink, PriorityFilterType value)
			{
				sink.EnterSequence();
				Value<byte>.Save(sink, value.MinPriority);
				Value<byte>.Save(sink, value.MaxPriority);
				sink.LeaveSequence();
			}
		}
	}
}
