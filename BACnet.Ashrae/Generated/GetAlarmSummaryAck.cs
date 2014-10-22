using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class GetAlarmSummaryAck
	{
		public ReadOnlyArray<Element> Item { get; private set; }

		public GetAlarmSummaryAck(ReadOnlyArray<Element> item)
		{
			this.Item = item;
		}

		public static readonly ISchema Schema = Value<ReadOnlyArray<Element>>.Schema;

		public static GetAlarmSummaryAck Load(IValueStream stream)
		{
			var temp = Value<ReadOnlyArray<Element>>.Load(stream);
			return new GetAlarmSummaryAck(temp);
		}

		public static void Save(IValueSink sink, GetAlarmSummaryAck value)
		{
			Value<ReadOnlyArray<Element>>.Save(sink, value.Item);
		}

		public  partial class Element
		{
			public ObjectId ObjectIdentifier { get; private set; }

			public EventState AlarmState { get; private set; }

			public EventTransitionBits AcknowledgedTransitions { get; private set; }

			public Element(ObjectId objectIdentifier, EventState alarmState, EventTransitionBits acknowledgedTransitions)
			{
				this.ObjectIdentifier = objectIdentifier;
				this.AlarmState = alarmState;
				this.AcknowledgedTransitions = acknowledgedTransitions;
			}

			public static readonly ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("ObjectIdentifier", 255, Value<ObjectId>.Schema),
				new FieldSchema("AlarmState", 255, Value<EventState>.Schema),
				new FieldSchema("AcknowledgedTransitions", 255, Value<EventTransitionBits>.Schema));

			public static Element Load(IValueStream stream)
			{
				stream.EnterSequence();
				var objectIdentifier = Value<ObjectId>.Load(stream);
				var alarmState = Value<EventState>.Load(stream);
				var acknowledgedTransitions = Value<EventTransitionBits>.Load(stream);
				stream.LeaveSequence();
				return new Element(objectIdentifier, alarmState, acknowledgedTransitions);
			}

			public static void Save(IValueSink sink, Element value)
			{
				sink.EnterSequence();
				Value<ObjectId>.Save(sink, value.ObjectIdentifier);
				Value<EventState>.Save(sink, value.AlarmState);
				Value<EventTransitionBits>.Save(sink, value.AcknowledgedTransitions);
				sink.LeaveSequence();
			}
		}
	}
}
