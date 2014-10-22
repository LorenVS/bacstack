using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public abstract  partial class EventParameter
	{
		public abstract Tags Tag { get; }

		public bool IsChangeOfBitstring { get { return this.Tag == Tags.ChangeOfBitstring; } }

		public ChangeOfBitstring AsChangeOfBitstring { get { return (ChangeOfBitstring)this; } }

		public static EventParameter NewChangeOfBitstring(uint timeDelay, BitString56 bitmask, ReadOnlyArray<BitString56> listOfBitstringValues)
		{
			return new ChangeOfBitstring(timeDelay, bitmask, listOfBitstringValues);
		}

		public bool IsChangeOfState { get { return this.Tag == Tags.ChangeOfState; } }

		public ChangeOfState AsChangeOfState { get { return (ChangeOfState)this; } }

		public static EventParameter NewChangeOfState(uint timeDelay, ReadOnlyArray<PropertyStates> listOfValues)
		{
			return new ChangeOfState(timeDelay, listOfValues);
		}

		public bool IsChangeOfValue { get { return this.Tag == Tags.ChangeOfValue; } }

		public ChangeOfValue AsChangeOfValue { get { return (ChangeOfValue)this; } }

		public static EventParameter NewChangeOfValue(uint timeDelay, COVCriteria covCriteria)
		{
			return new ChangeOfValue(timeDelay, covCriteria);
		}

		public bool IsCommandFailure { get { return this.Tag == Tags.CommandFailure; } }

		public CommandFailure AsCommandFailure { get { return (CommandFailure)this; } }

		public static EventParameter NewCommandFailure(uint timeDelay, DeviceObjectPropertyReference feedbackPropertyReference)
		{
			return new CommandFailure(timeDelay, feedbackPropertyReference);
		}

		public bool IsFloatingLimit { get { return this.Tag == Tags.FloatingLimit; } }

		public FloatingLimit AsFloatingLimit { get { return (FloatingLimit)this; } }

		public static EventParameter NewFloatingLimit(uint timeDelay, DeviceObjectPropertyReference setpointReference, float lowDiffLimit, float highDiffLimit, float deadband)
		{
			return new FloatingLimit(timeDelay, setpointReference, lowDiffLimit, highDiffLimit, deadband);
		}

		public bool IsOutOfRange { get { return this.Tag == Tags.OutOfRange; } }

		public OutOfRange AsOutOfRange { get { return (OutOfRange)this; } }

		public static EventParameter NewOutOfRange(uint timeDelay, float lowLimit, float highLimit, float deadband)
		{
			return new OutOfRange(timeDelay, lowLimit, highLimit, deadband);
		}

		public bool IsChangeOfLifeSafety { get { return this.Tag == Tags.ChangeOfLifeSafety; } }

		public ChangeOfLifeSafety AsChangeOfLifeSafety { get { return (ChangeOfLifeSafety)this; } }

		public static EventParameter NewChangeOfLifeSafety(uint timeDelay, ReadOnlyArray<LifeSafetyState> listOfLifeSafetyAlarmValues, ReadOnlyArray<LifeSafetyState> listOfAlarmValues, DeviceObjectPropertyReference modePropertyReference)
		{
			return new ChangeOfLifeSafety(timeDelay, listOfLifeSafetyAlarmValues, listOfAlarmValues, modePropertyReference);
		}

		public bool IsExtended { get { return this.Tag == Tags.Extended; } }

		public Extended AsExtended { get { return (Extended)this; } }

		public static EventParameter NewExtended(uint vendorId, uint extendedEventType, ReadOnlyArray<ExtendedParameter> parameters)
		{
			return new Extended(vendorId, extendedEventType, parameters);
		}

		public bool IsBufferReady { get { return this.Tag == Tags.BufferReady; } }

		public BufferReady AsBufferReady { get { return (BufferReady)this; } }

		public static EventParameter NewBufferReady(uint notificationThreshold, uint previousNotificationCount)
		{
			return new BufferReady(notificationThreshold, previousNotificationCount);
		}

		public bool IsUnsignedRange { get { return this.Tag == Tags.UnsignedRange; } }

		public UnsignedRange AsUnsignedRange { get { return (UnsignedRange)this; } }

		public static EventParameter NewUnsignedRange(uint timeDelay, uint lowLimit, uint highLimit)
		{
			return new UnsignedRange(timeDelay, lowLimit, highLimit);
		}

		public static readonly ISchema Schema = new ChoiceSchema(false, 
			new FieldSchema("ChangeOfBitstring", 0, Value<ChangeOfBitstring>.Schema),
			new FieldSchema("ChangeOfState", 1, Value<ChangeOfState>.Schema),
			new FieldSchema("ChangeOfValue", 2, Value<ChangeOfValue>.Schema),
			new FieldSchema("CommandFailure", 3, Value<CommandFailure>.Schema),
			new FieldSchema("FloatingLimit", 4, Value<FloatingLimit>.Schema),
			new FieldSchema("OutOfRange", 5, Value<OutOfRange>.Schema),
			new FieldSchema("ChangeOfLifeSafety", 8, Value<ChangeOfLifeSafety>.Schema),
			new FieldSchema("Extended", 9, Value<Extended>.Schema),
			new FieldSchema("BufferReady", 10, Value<BufferReady>.Schema),
			new FieldSchema("UnsignedRange", 11, Value<UnsignedRange>.Schema));

		public static EventParameter Load(IValueStream stream)
		{
			EventParameter ret = null;
			Tags tag = (Tags)stream.EnterChoice();
			switch(tag)
			{
				case Tags.ChangeOfBitstring:
					ret = Value<ChangeOfBitstring>.Load(stream);
					break;
				case Tags.ChangeOfState:
					ret = Value<ChangeOfState>.Load(stream);
					break;
				case Tags.ChangeOfValue:
					ret = Value<ChangeOfValue>.Load(stream);
					break;
				case Tags.CommandFailure:
					ret = Value<CommandFailure>.Load(stream);
					break;
				case Tags.FloatingLimit:
					ret = Value<FloatingLimit>.Load(stream);
					break;
				case Tags.OutOfRange:
					ret = Value<OutOfRange>.Load(stream);
					break;
				case Tags.ChangeOfLifeSafety:
					ret = Value<ChangeOfLifeSafety>.Load(stream);
					break;
				case Tags.Extended:
					ret = Value<Extended>.Load(stream);
					break;
				case Tags.BufferReady:
					ret = Value<BufferReady>.Load(stream);
					break;
				case Tags.UnsignedRange:
					ret = Value<UnsignedRange>.Load(stream);
					break;
				default:
					throw new Exception();
			}
			stream.LeaveChoice();
			return ret;
		}

		public static void Save(IValueSink sink, EventParameter value)
		{
			sink.EnterChoice((byte)value.Tag);
			switch(value.Tag)
			{
				case Tags.ChangeOfBitstring:
					Value<ChangeOfBitstring>.Save(sink, (ChangeOfBitstring)value);
					break;
				case Tags.ChangeOfState:
					Value<ChangeOfState>.Save(sink, (ChangeOfState)value);
					break;
				case Tags.ChangeOfValue:
					Value<ChangeOfValue>.Save(sink, (ChangeOfValue)value);
					break;
				case Tags.CommandFailure:
					Value<CommandFailure>.Save(sink, (CommandFailure)value);
					break;
				case Tags.FloatingLimit:
					Value<FloatingLimit>.Save(sink, (FloatingLimit)value);
					break;
				case Tags.OutOfRange:
					Value<OutOfRange>.Save(sink, (OutOfRange)value);
					break;
				case Tags.ChangeOfLifeSafety:
					Value<ChangeOfLifeSafety>.Save(sink, (ChangeOfLifeSafety)value);
					break;
				case Tags.Extended:
					Value<Extended>.Save(sink, (Extended)value);
					break;
				case Tags.BufferReady:
					Value<BufferReady>.Save(sink, (BufferReady)value);
					break;
				case Tags.UnsignedRange:
					Value<UnsignedRange>.Save(sink, (UnsignedRange)value);
					break;
				default:
					throw new Exception();
			}
			sink.LeaveChoice();
		}

		public enum Tags : byte
		{
			ChangeOfBitstring = 0,
			ChangeOfState = 1,
			ChangeOfValue = 2,
			CommandFailure = 3,
			FloatingLimit = 4,
			OutOfRange = 5,
			ChangeOfLifeSafety = 6,
			Extended = 7,
			BufferReady = 8,
			UnsignedRange = 9
		}
		public  partial class ChangeOfBitstring : EventParameter
		{
			public override Tags Tag { get { return Tags.ChangeOfBitstring; } }

			public uint TimeDelay { get; private set; }

			public BitString56 Bitmask { get; private set; }

			public ReadOnlyArray<BitString56> ListOfBitstringValues { get; private set; }

			public ChangeOfBitstring(uint timeDelay, BitString56 bitmask, ReadOnlyArray<BitString56> listOfBitstringValues)
			{
				this.TimeDelay = timeDelay;
				this.Bitmask = bitmask;
				this.ListOfBitstringValues = listOfBitstringValues;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("TimeDelay", 0, Value<uint>.Schema),
				new FieldSchema("Bitmask", 1, Value<BitString56>.Schema),
				new FieldSchema("ListOfBitstringValues", 2, Value<ReadOnlyArray<BitString56>>.Schema));

			public static new ChangeOfBitstring Load(IValueStream stream)
			{
				stream.EnterSequence();
				var timeDelay = Value<uint>.Load(stream);
				var bitmask = Value<BitString56>.Load(stream);
				var listOfBitstringValues = Value<ReadOnlyArray<BitString56>>.Load(stream);
				stream.LeaveSequence();
				return new ChangeOfBitstring(timeDelay, bitmask, listOfBitstringValues);
			}

			public static void Save(IValueSink sink, ChangeOfBitstring value)
			{
				sink.EnterSequence();
				Value<uint>.Save(sink, value.TimeDelay);
				Value<BitString56>.Save(sink, value.Bitmask);
				Value<ReadOnlyArray<BitString56>>.Save(sink, value.ListOfBitstringValues);
				sink.LeaveSequence();
			}
		}
		public  partial class ChangeOfState : EventParameter
		{
			public override Tags Tag { get { return Tags.ChangeOfState; } }

			public uint TimeDelay { get; private set; }

			public ReadOnlyArray<PropertyStates> ListOfValues { get; private set; }

			public ChangeOfState(uint timeDelay, ReadOnlyArray<PropertyStates> listOfValues)
			{
				this.TimeDelay = timeDelay;
				this.ListOfValues = listOfValues;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("TimeDelay", 0, Value<uint>.Schema),
				new FieldSchema("ListOfValues", 1, Value<ReadOnlyArray<PropertyStates>>.Schema));

			public static new ChangeOfState Load(IValueStream stream)
			{
				stream.EnterSequence();
				var timeDelay = Value<uint>.Load(stream);
				var listOfValues = Value<ReadOnlyArray<PropertyStates>>.Load(stream);
				stream.LeaveSequence();
				return new ChangeOfState(timeDelay, listOfValues);
			}

			public static void Save(IValueSink sink, ChangeOfState value)
			{
				sink.EnterSequence();
				Value<uint>.Save(sink, value.TimeDelay);
				Value<ReadOnlyArray<PropertyStates>>.Save(sink, value.ListOfValues);
				sink.LeaveSequence();
			}
		}
		public  partial class ChangeOfValue : EventParameter
		{
			public override Tags Tag { get { return Tags.ChangeOfValue; } }

			public uint TimeDelay { get; private set; }

			public COVCriteria CovCriteria { get; private set; }

			public ChangeOfValue(uint timeDelay, COVCriteria covCriteria)
			{
				this.TimeDelay = timeDelay;
				this.CovCriteria = covCriteria;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("TimeDelay", 0, Value<uint>.Schema),
				new FieldSchema("CovCriteria", 1, Value<COVCriteria>.Schema));

			public static new ChangeOfValue Load(IValueStream stream)
			{
				stream.EnterSequence();
				var timeDelay = Value<uint>.Load(stream);
				var covCriteria = Value<COVCriteria>.Load(stream);
				stream.LeaveSequence();
				return new ChangeOfValue(timeDelay, covCriteria);
			}

			public static void Save(IValueSink sink, ChangeOfValue value)
			{
				sink.EnterSequence();
				Value<uint>.Save(sink, value.TimeDelay);
				Value<COVCriteria>.Save(sink, value.CovCriteria);
				sink.LeaveSequence();
			}
		}
		public  partial class CommandFailure : EventParameter
		{
			public override Tags Tag { get { return Tags.CommandFailure; } }

			public uint TimeDelay { get; private set; }

			public DeviceObjectPropertyReference FeedbackPropertyReference { get; private set; }

			public CommandFailure(uint timeDelay, DeviceObjectPropertyReference feedbackPropertyReference)
			{
				this.TimeDelay = timeDelay;
				this.FeedbackPropertyReference = feedbackPropertyReference;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("TimeDelay", 0, Value<uint>.Schema),
				new FieldSchema("FeedbackPropertyReference", 1, Value<DeviceObjectPropertyReference>.Schema));

			public static new CommandFailure Load(IValueStream stream)
			{
				stream.EnterSequence();
				var timeDelay = Value<uint>.Load(stream);
				var feedbackPropertyReference = Value<DeviceObjectPropertyReference>.Load(stream);
				stream.LeaveSequence();
				return new CommandFailure(timeDelay, feedbackPropertyReference);
			}

			public static void Save(IValueSink sink, CommandFailure value)
			{
				sink.EnterSequence();
				Value<uint>.Save(sink, value.TimeDelay);
				Value<DeviceObjectPropertyReference>.Save(sink, value.FeedbackPropertyReference);
				sink.LeaveSequence();
			}
		}
		public  partial class FloatingLimit : EventParameter
		{
			public override Tags Tag { get { return Tags.FloatingLimit; } }

			public uint TimeDelay { get; private set; }

			public DeviceObjectPropertyReference SetpointReference { get; private set; }

			public float LowDiffLimit { get; private set; }

			public float HighDiffLimit { get; private set; }

			public float Deadband { get; private set; }

			public FloatingLimit(uint timeDelay, DeviceObjectPropertyReference setpointReference, float lowDiffLimit, float highDiffLimit, float deadband)
			{
				this.TimeDelay = timeDelay;
				this.SetpointReference = setpointReference;
				this.LowDiffLimit = lowDiffLimit;
				this.HighDiffLimit = highDiffLimit;
				this.Deadband = deadband;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("TimeDelay", 0, Value<uint>.Schema),
				new FieldSchema("SetpointReference", 1, Value<DeviceObjectPropertyReference>.Schema),
				new FieldSchema("LowDiffLimit", 2, Value<float>.Schema),
				new FieldSchema("HighDiffLimit", 3, Value<float>.Schema),
				new FieldSchema("Deadband", 4, Value<float>.Schema));

			public static new FloatingLimit Load(IValueStream stream)
			{
				stream.EnterSequence();
				var timeDelay = Value<uint>.Load(stream);
				var setpointReference = Value<DeviceObjectPropertyReference>.Load(stream);
				var lowDiffLimit = Value<float>.Load(stream);
				var highDiffLimit = Value<float>.Load(stream);
				var deadband = Value<float>.Load(stream);
				stream.LeaveSequence();
				return new FloatingLimit(timeDelay, setpointReference, lowDiffLimit, highDiffLimit, deadband);
			}

			public static void Save(IValueSink sink, FloatingLimit value)
			{
				sink.EnterSequence();
				Value<uint>.Save(sink, value.TimeDelay);
				Value<DeviceObjectPropertyReference>.Save(sink, value.SetpointReference);
				Value<float>.Save(sink, value.LowDiffLimit);
				Value<float>.Save(sink, value.HighDiffLimit);
				Value<float>.Save(sink, value.Deadband);
				sink.LeaveSequence();
			}
		}
		public  partial class OutOfRange : EventParameter
		{
			public override Tags Tag { get { return Tags.OutOfRange; } }

			public uint TimeDelay { get; private set; }

			public float LowLimit { get; private set; }

			public float HighLimit { get; private set; }

			public float Deadband { get; private set; }

			public OutOfRange(uint timeDelay, float lowLimit, float highLimit, float deadband)
			{
				this.TimeDelay = timeDelay;
				this.LowLimit = lowLimit;
				this.HighLimit = highLimit;
				this.Deadband = deadband;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("TimeDelay", 0, Value<uint>.Schema),
				new FieldSchema("LowLimit", 1, Value<float>.Schema),
				new FieldSchema("HighLimit", 2, Value<float>.Schema),
				new FieldSchema("Deadband", 3, Value<float>.Schema));

			public static new OutOfRange Load(IValueStream stream)
			{
				stream.EnterSequence();
				var timeDelay = Value<uint>.Load(stream);
				var lowLimit = Value<float>.Load(stream);
				var highLimit = Value<float>.Load(stream);
				var deadband = Value<float>.Load(stream);
				stream.LeaveSequence();
				return new OutOfRange(timeDelay, lowLimit, highLimit, deadband);
			}

			public static void Save(IValueSink sink, OutOfRange value)
			{
				sink.EnterSequence();
				Value<uint>.Save(sink, value.TimeDelay);
				Value<float>.Save(sink, value.LowLimit);
				Value<float>.Save(sink, value.HighLimit);
				Value<float>.Save(sink, value.Deadband);
				sink.LeaveSequence();
			}
		}
		public  partial class ChangeOfLifeSafety : EventParameter
		{
			public override Tags Tag { get { return Tags.ChangeOfLifeSafety; } }

			public uint TimeDelay { get; private set; }

			public ReadOnlyArray<LifeSafetyState> ListOfLifeSafetyAlarmValues { get; private set; }

			public ReadOnlyArray<LifeSafetyState> ListOfAlarmValues { get; private set; }

			public DeviceObjectPropertyReference ModePropertyReference { get; private set; }

			public ChangeOfLifeSafety(uint timeDelay, ReadOnlyArray<LifeSafetyState> listOfLifeSafetyAlarmValues, ReadOnlyArray<LifeSafetyState> listOfAlarmValues, DeviceObjectPropertyReference modePropertyReference)
			{
				this.TimeDelay = timeDelay;
				this.ListOfLifeSafetyAlarmValues = listOfLifeSafetyAlarmValues;
				this.ListOfAlarmValues = listOfAlarmValues;
				this.ModePropertyReference = modePropertyReference;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("TimeDelay", 0, Value<uint>.Schema),
				new FieldSchema("ListOfLifeSafetyAlarmValues", 1, Value<ReadOnlyArray<LifeSafetyState>>.Schema),
				new FieldSchema("ListOfAlarmValues", 2, Value<ReadOnlyArray<LifeSafetyState>>.Schema),
				new FieldSchema("ModePropertyReference", 3, Value<DeviceObjectPropertyReference>.Schema));

			public static new ChangeOfLifeSafety Load(IValueStream stream)
			{
				stream.EnterSequence();
				var timeDelay = Value<uint>.Load(stream);
				var listOfLifeSafetyAlarmValues = Value<ReadOnlyArray<LifeSafetyState>>.Load(stream);
				var listOfAlarmValues = Value<ReadOnlyArray<LifeSafetyState>>.Load(stream);
				var modePropertyReference = Value<DeviceObjectPropertyReference>.Load(stream);
				stream.LeaveSequence();
				return new ChangeOfLifeSafety(timeDelay, listOfLifeSafetyAlarmValues, listOfAlarmValues, modePropertyReference);
			}

			public static void Save(IValueSink sink, ChangeOfLifeSafety value)
			{
				sink.EnterSequence();
				Value<uint>.Save(sink, value.TimeDelay);
				Value<ReadOnlyArray<LifeSafetyState>>.Save(sink, value.ListOfLifeSafetyAlarmValues);
				Value<ReadOnlyArray<LifeSafetyState>>.Save(sink, value.ListOfAlarmValues);
				Value<DeviceObjectPropertyReference>.Save(sink, value.ModePropertyReference);
				sink.LeaveSequence();
			}
		}
		public  partial class Extended : EventParameter
		{
			public override Tags Tag { get { return Tags.Extended; } }

			public uint VendorId { get; private set; }

			public uint ExtendedEventType { get; private set; }

			public ReadOnlyArray<ExtendedParameter> Parameters { get; private set; }

			public Extended(uint vendorId, uint extendedEventType, ReadOnlyArray<ExtendedParameter> parameters)
			{
				this.VendorId = vendorId;
				this.ExtendedEventType = extendedEventType;
				this.Parameters = parameters;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("VendorId", 0, Value<uint>.Schema),
				new FieldSchema("ExtendedEventType", 1, Value<uint>.Schema),
				new FieldSchema("Parameters", 2, Value<ReadOnlyArray<ExtendedParameter>>.Schema));

			public static new Extended Load(IValueStream stream)
			{
				stream.EnterSequence();
				var vendorId = Value<uint>.Load(stream);
				var extendedEventType = Value<uint>.Load(stream);
				var parameters = Value<ReadOnlyArray<ExtendedParameter>>.Load(stream);
				stream.LeaveSequence();
				return new Extended(vendorId, extendedEventType, parameters);
			}

			public static void Save(IValueSink sink, Extended value)
			{
				sink.EnterSequence();
				Value<uint>.Save(sink, value.VendorId);
				Value<uint>.Save(sink, value.ExtendedEventType);
				Value<ReadOnlyArray<ExtendedParameter>>.Save(sink, value.Parameters);
				sink.LeaveSequence();
			}
		}
		public  partial class BufferReady : EventParameter
		{
			public override Tags Tag { get { return Tags.BufferReady; } }

			public uint NotificationThreshold { get; private set; }

			public uint PreviousNotificationCount { get; private set; }

			public BufferReady(uint notificationThreshold, uint previousNotificationCount)
			{
				this.NotificationThreshold = notificationThreshold;
				this.PreviousNotificationCount = previousNotificationCount;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("NotificationThreshold", 0, Value<uint>.Schema),
				new FieldSchema("PreviousNotificationCount", 1, Value<uint>.Schema));

			public static new BufferReady Load(IValueStream stream)
			{
				stream.EnterSequence();
				var notificationThreshold = Value<uint>.Load(stream);
				var previousNotificationCount = Value<uint>.Load(stream);
				stream.LeaveSequence();
				return new BufferReady(notificationThreshold, previousNotificationCount);
			}

			public static void Save(IValueSink sink, BufferReady value)
			{
				sink.EnterSequence();
				Value<uint>.Save(sink, value.NotificationThreshold);
				Value<uint>.Save(sink, value.PreviousNotificationCount);
				sink.LeaveSequence();
			}
		}
		public  partial class UnsignedRange : EventParameter
		{
			public override Tags Tag { get { return Tags.UnsignedRange; } }

			public uint TimeDelay { get; private set; }

			public uint LowLimit { get; private set; }

			public uint HighLimit { get; private set; }

			public UnsignedRange(uint timeDelay, uint lowLimit, uint highLimit)
			{
				this.TimeDelay = timeDelay;
				this.LowLimit = lowLimit;
				this.HighLimit = highLimit;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("TimeDelay", 0, Value<uint>.Schema),
				new FieldSchema("LowLimit", 1, Value<uint>.Schema),
				new FieldSchema("HighLimit", 2, Value<uint>.Schema));

			public static new UnsignedRange Load(IValueStream stream)
			{
				stream.EnterSequence();
				var timeDelay = Value<uint>.Load(stream);
				var lowLimit = Value<uint>.Load(stream);
				var highLimit = Value<uint>.Load(stream);
				stream.LeaveSequence();
				return new UnsignedRange(timeDelay, lowLimit, highLimit);
			}

			public static void Save(IValueSink sink, UnsignedRange value)
			{
				sink.EnterSequence();
				Value<uint>.Save(sink, value.TimeDelay);
				Value<uint>.Save(sink, value.LowLimit);
				Value<uint>.Save(sink, value.HighLimit);
				sink.LeaveSequence();
			}
		}
	}
}
