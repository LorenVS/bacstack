using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class AcknowledgeAlarmRequest
	{
		public uint AcknowledgingProcessIdentifier { get; private set; }

		public ObjectId EventObjectIdentifier { get; private set; }

		public EventState EventStateAcknowledged { get; private set; }

		public TimeStamp TimeStamp { get; private set; }

		public string AcknowledgmentSource { get; private set; }

		public TimeStamp TimeOfAcknowledgment { get; private set; }

		public AcknowledgeAlarmRequest(uint acknowledgingProcessIdentifier, ObjectId eventObjectIdentifier, EventState eventStateAcknowledged, TimeStamp timeStamp, string acknowledgmentSource, TimeStamp timeOfAcknowledgment)
		{
			this.AcknowledgingProcessIdentifier = acknowledgingProcessIdentifier;
			this.EventObjectIdentifier = eventObjectIdentifier;
			this.EventStateAcknowledged = eventStateAcknowledged;
			this.TimeStamp = timeStamp;
			this.AcknowledgmentSource = acknowledgmentSource;
			this.TimeOfAcknowledgment = timeOfAcknowledgment;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("AcknowledgingProcessIdentifier", 0, Value<uint>.Schema),
			new FieldSchema("EventObjectIdentifier", 1, Value<ObjectId>.Schema),
			new FieldSchema("EventStateAcknowledged", 2, Value<EventState>.Schema),
			new FieldSchema("TimeStamp", 3, Value<TimeStamp>.Schema),
			new FieldSchema("AcknowledgmentSource", 4, Value<string>.Schema),
			new FieldSchema("TimeOfAcknowledgment", 5, Value<TimeStamp>.Schema));

		public static AcknowledgeAlarmRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var acknowledgingProcessIdentifier = Value<uint>.Load(stream);
			var eventObjectIdentifier = Value<ObjectId>.Load(stream);
			var eventStateAcknowledged = Value<EventState>.Load(stream);
			var timeStamp = Value<TimeStamp>.Load(stream);
			var acknowledgmentSource = Value<string>.Load(stream);
			var timeOfAcknowledgment = Value<TimeStamp>.Load(stream);
			stream.LeaveSequence();
			return new AcknowledgeAlarmRequest(acknowledgingProcessIdentifier, eventObjectIdentifier, eventStateAcknowledged, timeStamp, acknowledgmentSource, timeOfAcknowledgment);
		}

		public static void Save(IValueSink sink, AcknowledgeAlarmRequest value)
		{
			sink.EnterSequence();
			Value<uint>.Save(sink, value.AcknowledgingProcessIdentifier);
			Value<ObjectId>.Save(sink, value.EventObjectIdentifier);
			Value<EventState>.Save(sink, value.EventStateAcknowledged);
			Value<TimeStamp>.Save(sink, value.TimeStamp);
			Value<string>.Save(sink, value.AcknowledgmentSource);
			Value<TimeStamp>.Save(sink, value.TimeOfAcknowledgment);
			sink.LeaveSequence();
		}
	}
}
