using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public abstract  partial class NotificationParameters
	{
		public abstract Tags Tag { get; }

		public bool IsChangeOfBitstring { get { return this.Tag == Tags.ChangeOfBitstring; } }

		public ChangeOfBitstring AsChangeOfBitstring { get { return (ChangeOfBitstring)this; } }

		public static NotificationParameters NewChangeOfBitstring(BitString56 referencedBitstring, StatusFlags statusFlags)
		{
			return new ChangeOfBitstring(referencedBitstring, statusFlags);
		}

		public bool IsChangeOfState { get { return this.Tag == Tags.ChangeOfState; } }

		public ChangeOfState AsChangeOfState { get { return (ChangeOfState)this; } }

		public static NotificationParameters NewChangeOfState(PropertyStates newState, StatusFlags statusFlags)
		{
			return new ChangeOfState(newState, statusFlags);
		}

		public bool IsChangeOfValue { get { return this.Tag == Tags.ChangeOfValue; } }

		public ChangeOfValue AsChangeOfValue { get { return (ChangeOfValue)this; } }

		public static NotificationParameters NewChangeOfValue(NewValue newValue, StatusFlags statusFlags)
		{
			return new ChangeOfValue(newValue, statusFlags);
		}

		public bool IsCommandFailure { get { return this.Tag == Tags.CommandFailure; } }

		public CommandFailure AsCommandFailure { get { return (CommandFailure)this; } }

		public static NotificationParameters NewCommandFailure(GenericValue commandValue, StatusFlags statusFlags, GenericValue feedbackValue)
		{
			return new CommandFailure(commandValue, statusFlags, feedbackValue);
		}

		public bool IsFloatingLimit { get { return this.Tag == Tags.FloatingLimit; } }

		public FloatingLimit AsFloatingLimit { get { return (FloatingLimit)this; } }

		public static NotificationParameters NewFloatingLimit(float referenceValue, StatusFlags statusFlags, float setpointValue, float errorLimit)
		{
			return new FloatingLimit(referenceValue, statusFlags, setpointValue, errorLimit);
		}

		public bool IsOutOfRange { get { return this.Tag == Tags.OutOfRange; } }

		public OutOfRange AsOutOfRange { get { return (OutOfRange)this; } }

		public static NotificationParameters NewOutOfRange(float exceedingValue, StatusFlags statusFlags, float deadband, float exceededLimit)
		{
			return new OutOfRange(exceedingValue, statusFlags, deadband, exceededLimit);
		}

		public bool IsComplexEventType { get { return this.Tag == Tags.ComplexEventType; } }

		public ReadOnlyArray<PropertyValue> AsComplexEventType { get { return ((ComplexEventTypeWrapper)this).Item; } }

		public static NotificationParameters NewComplexEventType(ReadOnlyArray<PropertyValue> complexEventType)
		{
			return new ComplexEventTypeWrapper(complexEventType);
		}

		public bool IsChangeOfLifeSafety { get { return this.Tag == Tags.ChangeOfLifeSafety; } }

		public ChangeOfLifeSafety AsChangeOfLifeSafety { get { return (ChangeOfLifeSafety)this; } }

		public static NotificationParameters NewChangeOfLifeSafety(LifeSafetyState newState, LifeSafetyMode newMode, StatusFlags statusFlags, LifeSafetyOperation operationExpected)
		{
			return new ChangeOfLifeSafety(newState, newMode, statusFlags, operationExpected);
		}

		public bool IsExtended { get { return this.Tag == Tags.Extended; } }

		public Extended AsExtended { get { return (Extended)this; } }

		public static NotificationParameters NewExtended(uint vendorId, uint extendedEventType, ReadOnlyArray<ExtendedParameter> parameters)
		{
			return new Extended(vendorId, extendedEventType, parameters);
		}

		public bool IsBufferReady { get { return this.Tag == Tags.BufferReady; } }

		public BufferReady AsBufferReady { get { return (BufferReady)this; } }

		public static NotificationParameters NewBufferReady(DeviceObjectPropertyReference bufferProperty, uint previousNotification, uint currentNotification)
		{
			return new BufferReady(bufferProperty, previousNotification, currentNotification);
		}

		public bool IsUnsignedRange { get { return this.Tag == Tags.UnsignedRange; } }

		public UnsignedRange AsUnsignedRange { get { return (UnsignedRange)this; } }

		public static NotificationParameters NewUnsignedRange(uint exceedingValue, StatusFlags statusFlags, uint exceededLimit)
		{
			return new UnsignedRange(exceedingValue, statusFlags, exceededLimit);
		}

		public static readonly ISchema Schema = new ChoiceSchema(false, 
			new FieldSchema("ChangeOfBitstring", 0, Value<ChangeOfBitstring>.Schema),
			new FieldSchema("ChangeOfState", 1, Value<ChangeOfState>.Schema),
			new FieldSchema("ChangeOfValue", 2, Value<ChangeOfValue>.Schema),
			new FieldSchema("CommandFailure", 3, Value<CommandFailure>.Schema),
			new FieldSchema("FloatingLimit", 4, Value<FloatingLimit>.Schema),
			new FieldSchema("OutOfRange", 5, Value<OutOfRange>.Schema),
			new FieldSchema("ComplexEventType", 6, Value<ReadOnlyArray<PropertyValue>>.Schema),
			new FieldSchema("ChangeOfLifeSafety", 8, Value<ChangeOfLifeSafety>.Schema),
			new FieldSchema("Extended", 9, Value<Extended>.Schema),
			new FieldSchema("BufferReady", 10, Value<BufferReady>.Schema),
			new FieldSchema("UnsignedRange", 11, Value<UnsignedRange>.Schema));

		public static NotificationParameters Load(IValueStream stream)
		{
			NotificationParameters ret = null;
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
				case Tags.ComplexEventType:
					ret = Value<ComplexEventTypeWrapper>.Load(stream);
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

		public static void Save(IValueSink sink, NotificationParameters value)
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
				case Tags.ComplexEventType:
					Value<ComplexEventTypeWrapper>.Save(sink, (ComplexEventTypeWrapper)value);
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
			ComplexEventType = 6,
			ChangeOfLifeSafety = 7,
			Extended = 8,
			BufferReady = 9,
			UnsignedRange = 10
		}

		public  partial class ChangeOfBitstringWrapper : NotificationParameters
		{
			public override Tags Tag { get { return Tags.ChangeOfBitstring; } }

			public ChangeOfBitstring Item { get; private set; }

			public ChangeOfBitstringWrapper(ChangeOfBitstring item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ChangeOfBitstring>.Schema;

			public static new ChangeOfBitstringWrapper Load(IValueStream stream)
			{
				var temp = Value<ChangeOfBitstring>.Load(stream);
				return new ChangeOfBitstringWrapper(temp);
			}

			public static void Save(IValueSink sink, ChangeOfBitstringWrapper value)
			{
				Value<ChangeOfBitstring>.Save(sink, value.Item);
			}

		}

		public  partial class ChangeOfStateWrapper : NotificationParameters
		{
			public override Tags Tag { get { return Tags.ChangeOfState; } }

			public ChangeOfState Item { get; private set; }

			public ChangeOfStateWrapper(ChangeOfState item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ChangeOfState>.Schema;

			public static new ChangeOfStateWrapper Load(IValueStream stream)
			{
				var temp = Value<ChangeOfState>.Load(stream);
				return new ChangeOfStateWrapper(temp);
			}

			public static void Save(IValueSink sink, ChangeOfStateWrapper value)
			{
				Value<ChangeOfState>.Save(sink, value.Item);
			}

		}

		public  partial class ChangeOfValueWrapper : NotificationParameters
		{
			public override Tags Tag { get { return Tags.ChangeOfValue; } }

			public ChangeOfValue Item { get; private set; }

			public ChangeOfValueWrapper(ChangeOfValue item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ChangeOfValue>.Schema;

			public static new ChangeOfValueWrapper Load(IValueStream stream)
			{
				var temp = Value<ChangeOfValue>.Load(stream);
				return new ChangeOfValueWrapper(temp);
			}

			public static void Save(IValueSink sink, ChangeOfValueWrapper value)
			{
				Value<ChangeOfValue>.Save(sink, value.Item);
			}

		}

		public  partial class CommandFailureWrapper : NotificationParameters
		{
			public override Tags Tag { get { return Tags.CommandFailure; } }

			public CommandFailure Item { get; private set; }

			public CommandFailureWrapper(CommandFailure item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<CommandFailure>.Schema;

			public static new CommandFailureWrapper Load(IValueStream stream)
			{
				var temp = Value<CommandFailure>.Load(stream);
				return new CommandFailureWrapper(temp);
			}

			public static void Save(IValueSink sink, CommandFailureWrapper value)
			{
				Value<CommandFailure>.Save(sink, value.Item);
			}

		}

		public  partial class FloatingLimitWrapper : NotificationParameters
		{
			public override Tags Tag { get { return Tags.FloatingLimit; } }

			public FloatingLimit Item { get; private set; }

			public FloatingLimitWrapper(FloatingLimit item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<FloatingLimit>.Schema;

			public static new FloatingLimitWrapper Load(IValueStream stream)
			{
				var temp = Value<FloatingLimit>.Load(stream);
				return new FloatingLimitWrapper(temp);
			}

			public static void Save(IValueSink sink, FloatingLimitWrapper value)
			{
				Value<FloatingLimit>.Save(sink, value.Item);
			}

		}

		public  partial class OutOfRangeWrapper : NotificationParameters
		{
			public override Tags Tag { get { return Tags.OutOfRange; } }

			public OutOfRange Item { get; private set; }

			public OutOfRangeWrapper(OutOfRange item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<OutOfRange>.Schema;

			public static new OutOfRangeWrapper Load(IValueStream stream)
			{
				var temp = Value<OutOfRange>.Load(stream);
				return new OutOfRangeWrapper(temp);
			}

			public static void Save(IValueSink sink, OutOfRangeWrapper value)
			{
				Value<OutOfRange>.Save(sink, value.Item);
			}

		}

		public  partial class ComplexEventTypeWrapper : NotificationParameters
		{
			public override Tags Tag { get { return Tags.ComplexEventType; } }

			public ReadOnlyArray<PropertyValue> Item { get; private set; }

			public ComplexEventTypeWrapper(ReadOnlyArray<PropertyValue> item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ReadOnlyArray<PropertyValue>>.Schema;

			public static new ComplexEventTypeWrapper Load(IValueStream stream)
			{
				var temp = Value<ReadOnlyArray<PropertyValue>>.Load(stream);
				return new ComplexEventTypeWrapper(temp);
			}

			public static void Save(IValueSink sink, ComplexEventTypeWrapper value)
			{
				Value<ReadOnlyArray<PropertyValue>>.Save(sink, value.Item);
			}

		}

		public  partial class ChangeOfLifeSafetyWrapper : NotificationParameters
		{
			public override Tags Tag { get { return Tags.ChangeOfLifeSafety; } }

			public ChangeOfLifeSafety Item { get; private set; }

			public ChangeOfLifeSafetyWrapper(ChangeOfLifeSafety item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ChangeOfLifeSafety>.Schema;

			public static new ChangeOfLifeSafetyWrapper Load(IValueStream stream)
			{
				var temp = Value<ChangeOfLifeSafety>.Load(stream);
				return new ChangeOfLifeSafetyWrapper(temp);
			}

			public static void Save(IValueSink sink, ChangeOfLifeSafetyWrapper value)
			{
				Value<ChangeOfLifeSafety>.Save(sink, value.Item);
			}

		}

		public  partial class ExtendedWrapper : NotificationParameters
		{
			public override Tags Tag { get { return Tags.Extended; } }

			public Extended Item { get; private set; }

			public ExtendedWrapper(Extended item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Extended>.Schema;

			public static new ExtendedWrapper Load(IValueStream stream)
			{
				var temp = Value<Extended>.Load(stream);
				return new ExtendedWrapper(temp);
			}

			public static void Save(IValueSink sink, ExtendedWrapper value)
			{
				Value<Extended>.Save(sink, value.Item);
			}

		}

		public  partial class BufferReadyWrapper : NotificationParameters
		{
			public override Tags Tag { get { return Tags.BufferReady; } }

			public BufferReady Item { get; private set; }

			public BufferReadyWrapper(BufferReady item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<BufferReady>.Schema;

			public static new BufferReadyWrapper Load(IValueStream stream)
			{
				var temp = Value<BufferReady>.Load(stream);
				return new BufferReadyWrapper(temp);
			}

			public static void Save(IValueSink sink, BufferReadyWrapper value)
			{
				Value<BufferReady>.Save(sink, value.Item);
			}

		}

		public  partial class UnsignedRangeWrapper : NotificationParameters
		{
			public override Tags Tag { get { return Tags.UnsignedRange; } }

			public UnsignedRange Item { get; private set; }

			public UnsignedRangeWrapper(UnsignedRange item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<UnsignedRange>.Schema;

			public static new UnsignedRangeWrapper Load(IValueStream stream)
			{
				var temp = Value<UnsignedRange>.Load(stream);
				return new UnsignedRangeWrapper(temp);
			}

			public static void Save(IValueSink sink, UnsignedRangeWrapper value)
			{
				Value<UnsignedRange>.Save(sink, value.Item);
			}

		}
		public  partial class ChangeOfBitstring : NotificationParameters
		{
			public override Tags Tag { get { return Tags.ChangeOfBitstring; } }

			public BitString56 ReferencedBitstring { get; private set; }

			public StatusFlags StatusFlags { get; private set; }

			public ChangeOfBitstring(BitString56 referencedBitstring, StatusFlags statusFlags)
			{
				this.ReferencedBitstring = referencedBitstring;
				this.StatusFlags = statusFlags;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("ReferencedBitstring", 0, Value<BitString56>.Schema),
				new FieldSchema("StatusFlags", 1, Value<StatusFlags>.Schema));

			public static new ChangeOfBitstring Load(IValueStream stream)
			{
				stream.EnterSequence();
				var referencedBitstring = Value<BitString56>.Load(stream);
				var statusFlags = Value<StatusFlags>.Load(stream);
				stream.LeaveSequence();
				return new ChangeOfBitstring(referencedBitstring, statusFlags);
			}

			public static void Save(IValueSink sink, ChangeOfBitstring value)
			{
				sink.EnterSequence();
				Value<BitString56>.Save(sink, value.ReferencedBitstring);
				Value<StatusFlags>.Save(sink, value.StatusFlags);
				sink.LeaveSequence();
			}
		}
		public  partial class ChangeOfState : NotificationParameters
		{
			public override Tags Tag { get { return Tags.ChangeOfState; } }

			public PropertyStates NewState { get; private set; }

			public StatusFlags StatusFlags { get; private set; }

			public ChangeOfState(PropertyStates newState, StatusFlags statusFlags)
			{
				this.NewState = newState;
				this.StatusFlags = statusFlags;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("NewState", 0, Value<PropertyStates>.Schema),
				new FieldSchema("StatusFlags", 1, Value<StatusFlags>.Schema));

			public static new ChangeOfState Load(IValueStream stream)
			{
				stream.EnterSequence();
				var newState = Value<PropertyStates>.Load(stream);
				var statusFlags = Value<StatusFlags>.Load(stream);
				stream.LeaveSequence();
				return new ChangeOfState(newState, statusFlags);
			}

			public static void Save(IValueSink sink, ChangeOfState value)
			{
				sink.EnterSequence();
				Value<PropertyStates>.Save(sink, value.NewState);
				Value<StatusFlags>.Save(sink, value.StatusFlags);
				sink.LeaveSequence();
			}
		}
		public  partial class ChangeOfValue : NotificationParameters
		{
			public override Tags Tag { get { return Tags.ChangeOfValue; } }

			public NewValue NewValue { get; private set; }

			public StatusFlags StatusFlags { get; private set; }

			public ChangeOfValue(NewValue newValue, StatusFlags statusFlags)
			{
				this.NewValue = newValue;
				this.StatusFlags = statusFlags;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("NewValue", 0, Value<NewValue>.Schema),
				new FieldSchema("StatusFlags", 1, Value<StatusFlags>.Schema));

			public static new ChangeOfValue Load(IValueStream stream)
			{
				stream.EnterSequence();
				var newValue = Value<NewValue>.Load(stream);
				var statusFlags = Value<StatusFlags>.Load(stream);
				stream.LeaveSequence();
				return new ChangeOfValue(newValue, statusFlags);
			}

			public static void Save(IValueSink sink, ChangeOfValue value)
			{
				sink.EnterSequence();
				Value<NewValue>.Save(sink, value.NewValue);
				Value<StatusFlags>.Save(sink, value.StatusFlags);
				sink.LeaveSequence();
			}
		}
		public  partial class CommandFailure : NotificationParameters
		{
			public override Tags Tag { get { return Tags.CommandFailure; } }

			public GenericValue CommandValue { get; private set; }

			public StatusFlags StatusFlags { get; private set; }

			public GenericValue FeedbackValue { get; private set; }

			public CommandFailure(GenericValue commandValue, StatusFlags statusFlags, GenericValue feedbackValue)
			{
				this.CommandValue = commandValue;
				this.StatusFlags = statusFlags;
				this.FeedbackValue = feedbackValue;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("CommandValue", 0, Value<GenericValue>.Schema),
				new FieldSchema("StatusFlags", 1, Value<StatusFlags>.Schema),
				new FieldSchema("FeedbackValue", 2, Value<GenericValue>.Schema));

			public static new CommandFailure Load(IValueStream stream)
			{
				stream.EnterSequence();
				var commandValue = Value<GenericValue>.Load(stream);
				var statusFlags = Value<StatusFlags>.Load(stream);
				var feedbackValue = Value<GenericValue>.Load(stream);
				stream.LeaveSequence();
				return new CommandFailure(commandValue, statusFlags, feedbackValue);
			}

			public static void Save(IValueSink sink, CommandFailure value)
			{
				sink.EnterSequence();
				Value<GenericValue>.Save(sink, value.CommandValue);
				Value<StatusFlags>.Save(sink, value.StatusFlags);
				Value<GenericValue>.Save(sink, value.FeedbackValue);
				sink.LeaveSequence();
			}
		}
		public  partial class FloatingLimit : NotificationParameters
		{
			public override Tags Tag { get { return Tags.FloatingLimit; } }

			public float ReferenceValue { get; private set; }

			public StatusFlags StatusFlags { get; private set; }

			public float SetpointValue { get; private set; }

			public float ErrorLimit { get; private set; }

			public FloatingLimit(float referenceValue, StatusFlags statusFlags, float setpointValue, float errorLimit)
			{
				this.ReferenceValue = referenceValue;
				this.StatusFlags = statusFlags;
				this.SetpointValue = setpointValue;
				this.ErrorLimit = errorLimit;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("ReferenceValue", 0, Value<float>.Schema),
				new FieldSchema("StatusFlags", 1, Value<StatusFlags>.Schema),
				new FieldSchema("SetpointValue", 2, Value<float>.Schema),
				new FieldSchema("ErrorLimit", 3, Value<float>.Schema));

			public static new FloatingLimit Load(IValueStream stream)
			{
				stream.EnterSequence();
				var referenceValue = Value<float>.Load(stream);
				var statusFlags = Value<StatusFlags>.Load(stream);
				var setpointValue = Value<float>.Load(stream);
				var errorLimit = Value<float>.Load(stream);
				stream.LeaveSequence();
				return new FloatingLimit(referenceValue, statusFlags, setpointValue, errorLimit);
			}

			public static void Save(IValueSink sink, FloatingLimit value)
			{
				sink.EnterSequence();
				Value<float>.Save(sink, value.ReferenceValue);
				Value<StatusFlags>.Save(sink, value.StatusFlags);
				Value<float>.Save(sink, value.SetpointValue);
				Value<float>.Save(sink, value.ErrorLimit);
				sink.LeaveSequence();
			}
		}
		public  partial class OutOfRange : NotificationParameters
		{
			public override Tags Tag { get { return Tags.OutOfRange; } }

			public float ExceedingValue { get; private set; }

			public StatusFlags StatusFlags { get; private set; }

			public float Deadband { get; private set; }

			public float ExceededLimit { get; private set; }

			public OutOfRange(float exceedingValue, StatusFlags statusFlags, float deadband, float exceededLimit)
			{
				this.ExceedingValue = exceedingValue;
				this.StatusFlags = statusFlags;
				this.Deadband = deadband;
				this.ExceededLimit = exceededLimit;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("ExceedingValue", 0, Value<float>.Schema),
				new FieldSchema("StatusFlags", 1, Value<StatusFlags>.Schema),
				new FieldSchema("Deadband", 2, Value<float>.Schema),
				new FieldSchema("ExceededLimit", 3, Value<float>.Schema));

			public static new OutOfRange Load(IValueStream stream)
			{
				stream.EnterSequence();
				var exceedingValue = Value<float>.Load(stream);
				var statusFlags = Value<StatusFlags>.Load(stream);
				var deadband = Value<float>.Load(stream);
				var exceededLimit = Value<float>.Load(stream);
				stream.LeaveSequence();
				return new OutOfRange(exceedingValue, statusFlags, deadband, exceededLimit);
			}

			public static void Save(IValueSink sink, OutOfRange value)
			{
				sink.EnterSequence();
				Value<float>.Save(sink, value.ExceedingValue);
				Value<StatusFlags>.Save(sink, value.StatusFlags);
				Value<float>.Save(sink, value.Deadband);
				Value<float>.Save(sink, value.ExceededLimit);
				sink.LeaveSequence();
			}
		}
		public  partial class ChangeOfLifeSafety : NotificationParameters
		{
			public override Tags Tag { get { return Tags.ChangeOfLifeSafety; } }

			public LifeSafetyState NewState { get; private set; }

			public LifeSafetyMode NewMode { get; private set; }

			public StatusFlags StatusFlags { get; private set; }

			public LifeSafetyOperation OperationExpected { get; private set; }

			public ChangeOfLifeSafety(LifeSafetyState newState, LifeSafetyMode newMode, StatusFlags statusFlags, LifeSafetyOperation operationExpected)
			{
				this.NewState = newState;
				this.NewMode = newMode;
				this.StatusFlags = statusFlags;
				this.OperationExpected = operationExpected;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("NewState", 0, Value<LifeSafetyState>.Schema),
				new FieldSchema("NewMode", 1, Value<LifeSafetyMode>.Schema),
				new FieldSchema("StatusFlags", 2, Value<StatusFlags>.Schema),
				new FieldSchema("OperationExpected", 3, Value<LifeSafetyOperation>.Schema));

			public static new ChangeOfLifeSafety Load(IValueStream stream)
			{
				stream.EnterSequence();
				var newState = Value<LifeSafetyState>.Load(stream);
				var newMode = Value<LifeSafetyMode>.Load(stream);
				var statusFlags = Value<StatusFlags>.Load(stream);
				var operationExpected = Value<LifeSafetyOperation>.Load(stream);
				stream.LeaveSequence();
				return new ChangeOfLifeSafety(newState, newMode, statusFlags, operationExpected);
			}

			public static void Save(IValueSink sink, ChangeOfLifeSafety value)
			{
				sink.EnterSequence();
				Value<LifeSafetyState>.Save(sink, value.NewState);
				Value<LifeSafetyMode>.Save(sink, value.NewMode);
				Value<StatusFlags>.Save(sink, value.StatusFlags);
				Value<LifeSafetyOperation>.Save(sink, value.OperationExpected);
				sink.LeaveSequence();
			}
		}
		public  partial class Extended : NotificationParameters
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
		public  partial class BufferReady : NotificationParameters
		{
			public override Tags Tag { get { return Tags.BufferReady; } }

			public DeviceObjectPropertyReference BufferProperty { get; private set; }

			public uint PreviousNotification { get; private set; }

			public uint CurrentNotification { get; private set; }

			public BufferReady(DeviceObjectPropertyReference bufferProperty, uint previousNotification, uint currentNotification)
			{
				this.BufferProperty = bufferProperty;
				this.PreviousNotification = previousNotification;
				this.CurrentNotification = currentNotification;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("BufferProperty", 0, Value<DeviceObjectPropertyReference>.Schema),
				new FieldSchema("PreviousNotification", 1, Value<uint>.Schema),
				new FieldSchema("CurrentNotification", 2, Value<uint>.Schema));

			public static new BufferReady Load(IValueStream stream)
			{
				stream.EnterSequence();
				var bufferProperty = Value<DeviceObjectPropertyReference>.Load(stream);
				var previousNotification = Value<uint>.Load(stream);
				var currentNotification = Value<uint>.Load(stream);
				stream.LeaveSequence();
				return new BufferReady(bufferProperty, previousNotification, currentNotification);
			}

			public static void Save(IValueSink sink, BufferReady value)
			{
				sink.EnterSequence();
				Value<DeviceObjectPropertyReference>.Save(sink, value.BufferProperty);
				Value<uint>.Save(sink, value.PreviousNotification);
				Value<uint>.Save(sink, value.CurrentNotification);
				sink.LeaveSequence();
			}
		}
		public  partial class UnsignedRange : NotificationParameters
		{
			public override Tags Tag { get { return Tags.UnsignedRange; } }

			public uint ExceedingValue { get; private set; }

			public StatusFlags StatusFlags { get; private set; }

			public uint ExceededLimit { get; private set; }

			public UnsignedRange(uint exceedingValue, StatusFlags statusFlags, uint exceededLimit)
			{
				this.ExceedingValue = exceedingValue;
				this.StatusFlags = statusFlags;
				this.ExceededLimit = exceededLimit;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("ExceedingValue", 0, Value<uint>.Schema),
				new FieldSchema("StatusFlags", 1, Value<StatusFlags>.Schema),
				new FieldSchema("ExceededLimit", 2, Value<uint>.Schema));

			public static new UnsignedRange Load(IValueStream stream)
			{
				stream.EnterSequence();
				var exceedingValue = Value<uint>.Load(stream);
				var statusFlags = Value<StatusFlags>.Load(stream);
				var exceededLimit = Value<uint>.Load(stream);
				stream.LeaveSequence();
				return new UnsignedRange(exceedingValue, statusFlags, exceededLimit);
			}

			public static void Save(IValueSink sink, UnsignedRange value)
			{
				sink.EnterSequence();
				Value<uint>.Save(sink, value.ExceedingValue);
				Value<StatusFlags>.Save(sink, value.StatusFlags);
				Value<uint>.Save(sink, value.ExceededLimit);
				sink.LeaveSequence();
			}
		}
	}
}
