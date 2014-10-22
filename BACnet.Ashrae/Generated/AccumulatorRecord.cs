using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class AccumulatorRecord
	{
		public DateAndTime Timestamp { get; private set; }

		public uint PresentValue { get; private set; }

		public uint AccumulatedValue { get; private set; }

		public AccumulatorStatusType AccumulatorStatus { get; private set; }

		public AccumulatorRecord(DateAndTime timestamp, uint presentValue, uint accumulatedValue, AccumulatorStatusType accumulatorStatus)
		{
			this.Timestamp = timestamp;
			this.PresentValue = presentValue;
			this.AccumulatedValue = accumulatedValue;
			this.AccumulatorStatus = accumulatorStatus;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("Timestamp", 0, Value<DateAndTime>.Schema),
			new FieldSchema("PresentValue", 1, Value<uint>.Schema),
			new FieldSchema("AccumulatedValue", 2, Value<uint>.Schema),
			new FieldSchema("AccumulatorStatus", 3, Value<AccumulatorStatusType>.Schema));

		public static AccumulatorRecord Load(IValueStream stream)
		{
			stream.EnterSequence();
			var timestamp = Value<DateAndTime>.Load(stream);
			var presentValue = Value<uint>.Load(stream);
			var accumulatedValue = Value<uint>.Load(stream);
			var accumulatorStatus = Value<AccumulatorStatusType>.Load(stream);
			stream.LeaveSequence();
			return new AccumulatorRecord(timestamp, presentValue, accumulatedValue, accumulatorStatus);
		}

		public static void Save(IValueSink sink, AccumulatorRecord value)
		{
			sink.EnterSequence();
			Value<DateAndTime>.Save(sink, value.Timestamp);
			Value<uint>.Save(sink, value.PresentValue);
			Value<uint>.Save(sink, value.AccumulatedValue);
			Value<AccumulatorStatusType>.Save(sink, value.AccumulatorStatus);
			sink.LeaveSequence();
		}
		public enum AccumulatorStatusType : uint
		{
			Normal = 0,
			Starting = 1,
			Recovered = 2,
			Abnormal = 3,
			Failed = 4
		}
	}
}
