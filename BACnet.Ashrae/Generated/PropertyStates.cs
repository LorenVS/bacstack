using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public abstract  partial class PropertyStates
	{
		public abstract Tags Tag { get; }

		public bool IsBooleanValue { get { return this.Tag == Tags.BooleanValue; } }

		public bool AsBooleanValue { get { return ((BooleanValueWrapper)this).Item; } }

		public static PropertyStates NewBooleanValue(bool booleanValue)
		{
			return new BooleanValueWrapper(booleanValue);
		}

		public bool IsBinaryValue { get { return this.Tag == Tags.BinaryValue; } }

		public BinaryPV AsBinaryValue { get { return ((BinaryValueWrapper)this).Item; } }

		public static PropertyStates NewBinaryValue(BinaryPV binaryValue)
		{
			return new BinaryValueWrapper(binaryValue);
		}

		public bool IsEventType { get { return this.Tag == Tags.EventType; } }

		public EventType AsEventType { get { return ((EventTypeWrapper)this).Item; } }

		public static PropertyStates NewEventType(EventType eventType)
		{
			return new EventTypeWrapper(eventType);
		}

		public bool IsPolarity { get { return this.Tag == Tags.Polarity; } }

		public Polarity AsPolarity { get { return ((PolarityWrapper)this).Item; } }

		public static PropertyStates NewPolarity(Polarity polarity)
		{
			return new PolarityWrapper(polarity);
		}

		public bool IsProgramChange { get { return this.Tag == Tags.ProgramChange; } }

		public ProgramRequest AsProgramChange { get { return ((ProgramChangeWrapper)this).Item; } }

		public static PropertyStates NewProgramChange(ProgramRequest programChange)
		{
			return new ProgramChangeWrapper(programChange);
		}

		public bool IsProgramState { get { return this.Tag == Tags.ProgramState; } }

		public ProgramState AsProgramState { get { return ((ProgramStateWrapper)this).Item; } }

		public static PropertyStates NewProgramState(ProgramState programState)
		{
			return new ProgramStateWrapper(programState);
		}

		public bool IsReasonForHalt { get { return this.Tag == Tags.ReasonForHalt; } }

		public ProgramError AsReasonForHalt { get { return ((ReasonForHaltWrapper)this).Item; } }

		public static PropertyStates NewReasonForHalt(ProgramError reasonForHalt)
		{
			return new ReasonForHaltWrapper(reasonForHalt);
		}

		public bool IsReliability { get { return this.Tag == Tags.Reliability; } }

		public Reliability AsReliability { get { return ((ReliabilityWrapper)this).Item; } }

		public static PropertyStates NewReliability(Reliability reliability)
		{
			return new ReliabilityWrapper(reliability);
		}

		public bool IsState { get { return this.Tag == Tags.State; } }

		public EventState AsState { get { return ((StateWrapper)this).Item; } }

		public static PropertyStates NewState(EventState state)
		{
			return new StateWrapper(state);
		}

		public bool IsSystemStatus { get { return this.Tag == Tags.SystemStatus; } }

		public DeviceStatus AsSystemStatus { get { return ((SystemStatusWrapper)this).Item; } }

		public static PropertyStates NewSystemStatus(DeviceStatus systemStatus)
		{
			return new SystemStatusWrapper(systemStatus);
		}

		public bool IsUnits { get { return this.Tag == Tags.Units; } }

		public EngineeringUnits AsUnits { get { return ((UnitsWrapper)this).Item; } }

		public static PropertyStates NewUnits(EngineeringUnits units)
		{
			return new UnitsWrapper(units);
		}

		public bool IsUnsignedValue { get { return this.Tag == Tags.UnsignedValue; } }

		public uint AsUnsignedValue { get { return ((UnsignedValueWrapper)this).Item; } }

		public static PropertyStates NewUnsignedValue(uint unsignedValue)
		{
			return new UnsignedValueWrapper(unsignedValue);
		}

		public bool IsLifeSafetyMode { get { return this.Tag == Tags.LifeSafetyMode; } }

		public LifeSafetyMode AsLifeSafetyMode { get { return ((LifeSafetyModeWrapper)this).Item; } }

		public static PropertyStates NewLifeSafetyMode(LifeSafetyMode lifeSafetyMode)
		{
			return new LifeSafetyModeWrapper(lifeSafetyMode);
		}

		public bool IsLifeSafetyState { get { return this.Tag == Tags.LifeSafetyState; } }

		public LifeSafetyState AsLifeSafetyState { get { return ((LifeSafetyStateWrapper)this).Item; } }

		public static PropertyStates NewLifeSafetyState(LifeSafetyState lifeSafetyState)
		{
			return new LifeSafetyStateWrapper(lifeSafetyState);
		}

		public static readonly ISchema Schema = new ChoiceSchema(false, 
			new FieldSchema("BooleanValue", 0, Value<bool>.Schema),
			new FieldSchema("BinaryValue", 1, Value<BinaryPV>.Schema),
			new FieldSchema("EventType", 2, Value<EventType>.Schema),
			new FieldSchema("Polarity", 3, Value<Polarity>.Schema),
			new FieldSchema("ProgramChange", 4, Value<ProgramRequest>.Schema),
			new FieldSchema("ProgramState", 5, Value<ProgramState>.Schema),
			new FieldSchema("ReasonForHalt", 6, Value<ProgramError>.Schema),
			new FieldSchema("Reliability", 7, Value<Reliability>.Schema),
			new FieldSchema("State", 8, Value<EventState>.Schema),
			new FieldSchema("SystemStatus", 9, Value<DeviceStatus>.Schema),
			new FieldSchema("Units", 10, Value<EngineeringUnits>.Schema),
			new FieldSchema("UnsignedValue", 11, Value<uint>.Schema),
			new FieldSchema("LifeSafetyMode", 12, Value<LifeSafetyMode>.Schema),
			new FieldSchema("LifeSafetyState", 13, Value<LifeSafetyState>.Schema));

		public static PropertyStates Load(IValueStream stream)
		{
			PropertyStates ret = null;
			Tags tag = (Tags)stream.EnterChoice();
			switch(tag)
			{
				case Tags.BooleanValue:
					ret = Value<BooleanValueWrapper>.Load(stream);
					break;
				case Tags.BinaryValue:
					ret = Value<BinaryValueWrapper>.Load(stream);
					break;
				case Tags.EventType:
					ret = Value<EventTypeWrapper>.Load(stream);
					break;
				case Tags.Polarity:
					ret = Value<PolarityWrapper>.Load(stream);
					break;
				case Tags.ProgramChange:
					ret = Value<ProgramChangeWrapper>.Load(stream);
					break;
				case Tags.ProgramState:
					ret = Value<ProgramStateWrapper>.Load(stream);
					break;
				case Tags.ReasonForHalt:
					ret = Value<ReasonForHaltWrapper>.Load(stream);
					break;
				case Tags.Reliability:
					ret = Value<ReliabilityWrapper>.Load(stream);
					break;
				case Tags.State:
					ret = Value<StateWrapper>.Load(stream);
					break;
				case Tags.SystemStatus:
					ret = Value<SystemStatusWrapper>.Load(stream);
					break;
				case Tags.Units:
					ret = Value<UnitsWrapper>.Load(stream);
					break;
				case Tags.UnsignedValue:
					ret = Value<UnsignedValueWrapper>.Load(stream);
					break;
				case Tags.LifeSafetyMode:
					ret = Value<LifeSafetyModeWrapper>.Load(stream);
					break;
				case Tags.LifeSafetyState:
					ret = Value<LifeSafetyStateWrapper>.Load(stream);
					break;
				default:
					throw new Exception();
			}
			stream.LeaveChoice();
			return ret;
		}

		public static void Save(IValueSink sink, PropertyStates value)
		{
			sink.EnterChoice((byte)value.Tag);
			switch(value.Tag)
			{
				case Tags.BooleanValue:
					Value<BooleanValueWrapper>.Save(sink, (BooleanValueWrapper)value);
					break;
				case Tags.BinaryValue:
					Value<BinaryValueWrapper>.Save(sink, (BinaryValueWrapper)value);
					break;
				case Tags.EventType:
					Value<EventTypeWrapper>.Save(sink, (EventTypeWrapper)value);
					break;
				case Tags.Polarity:
					Value<PolarityWrapper>.Save(sink, (PolarityWrapper)value);
					break;
				case Tags.ProgramChange:
					Value<ProgramChangeWrapper>.Save(sink, (ProgramChangeWrapper)value);
					break;
				case Tags.ProgramState:
					Value<ProgramStateWrapper>.Save(sink, (ProgramStateWrapper)value);
					break;
				case Tags.ReasonForHalt:
					Value<ReasonForHaltWrapper>.Save(sink, (ReasonForHaltWrapper)value);
					break;
				case Tags.Reliability:
					Value<ReliabilityWrapper>.Save(sink, (ReliabilityWrapper)value);
					break;
				case Tags.State:
					Value<StateWrapper>.Save(sink, (StateWrapper)value);
					break;
				case Tags.SystemStatus:
					Value<SystemStatusWrapper>.Save(sink, (SystemStatusWrapper)value);
					break;
				case Tags.Units:
					Value<UnitsWrapper>.Save(sink, (UnitsWrapper)value);
					break;
				case Tags.UnsignedValue:
					Value<UnsignedValueWrapper>.Save(sink, (UnsignedValueWrapper)value);
					break;
				case Tags.LifeSafetyMode:
					Value<LifeSafetyModeWrapper>.Save(sink, (LifeSafetyModeWrapper)value);
					break;
				case Tags.LifeSafetyState:
					Value<LifeSafetyStateWrapper>.Save(sink, (LifeSafetyStateWrapper)value);
					break;
				default:
					throw new Exception();
			}
			sink.LeaveChoice();
		}

		public enum Tags : byte
		{
			BooleanValue = 0,
			BinaryValue = 1,
			EventType = 2,
			Polarity = 3,
			ProgramChange = 4,
			ProgramState = 5,
			ReasonForHalt = 6,
			Reliability = 7,
			State = 8,
			SystemStatus = 9,
			Units = 10,
			UnsignedValue = 11,
			LifeSafetyMode = 12,
			LifeSafetyState = 13
		}

		public  partial class BooleanValueWrapper : PropertyStates
		{
			public override Tags Tag { get { return Tags.BooleanValue; } }

			public bool Item { get; private set; }

			public BooleanValueWrapper(bool item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<bool>.Schema;

			public static new BooleanValueWrapper Load(IValueStream stream)
			{
				var temp = Value<bool>.Load(stream);
				return new BooleanValueWrapper(temp);
			}

			public static void Save(IValueSink sink, BooleanValueWrapper value)
			{
				Value<bool>.Save(sink, value.Item);
			}

		}

		public  partial class BinaryValueWrapper : PropertyStates
		{
			public override Tags Tag { get { return Tags.BinaryValue; } }

			public BinaryPV Item { get; private set; }

			public BinaryValueWrapper(BinaryPV item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<BinaryPV>.Schema;

			public static new BinaryValueWrapper Load(IValueStream stream)
			{
				var temp = Value<BinaryPV>.Load(stream);
				return new BinaryValueWrapper(temp);
			}

			public static void Save(IValueSink sink, BinaryValueWrapper value)
			{
				Value<BinaryPV>.Save(sink, value.Item);
			}

		}

		public  partial class EventTypeWrapper : PropertyStates
		{
			public override Tags Tag { get { return Tags.EventType; } }

			public EventType Item { get; private set; }

			public EventTypeWrapper(EventType item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<EventType>.Schema;

			public static new EventTypeWrapper Load(IValueStream stream)
			{
				var temp = Value<EventType>.Load(stream);
				return new EventTypeWrapper(temp);
			}

			public static void Save(IValueSink sink, EventTypeWrapper value)
			{
				Value<EventType>.Save(sink, value.Item);
			}

		}

		public  partial class PolarityWrapper : PropertyStates
		{
			public override Tags Tag { get { return Tags.Polarity; } }

			public Polarity Item { get; private set; }

			public PolarityWrapper(Polarity item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Polarity>.Schema;

			public static new PolarityWrapper Load(IValueStream stream)
			{
				var temp = Value<Polarity>.Load(stream);
				return new PolarityWrapper(temp);
			}

			public static void Save(IValueSink sink, PolarityWrapper value)
			{
				Value<Polarity>.Save(sink, value.Item);
			}

		}

		public  partial class ProgramChangeWrapper : PropertyStates
		{
			public override Tags Tag { get { return Tags.ProgramChange; } }

			public ProgramRequest Item { get; private set; }

			public ProgramChangeWrapper(ProgramRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ProgramRequest>.Schema;

			public static new ProgramChangeWrapper Load(IValueStream stream)
			{
				var temp = Value<ProgramRequest>.Load(stream);
				return new ProgramChangeWrapper(temp);
			}

			public static void Save(IValueSink sink, ProgramChangeWrapper value)
			{
				Value<ProgramRequest>.Save(sink, value.Item);
			}

		}

		public  partial class ProgramStateWrapper : PropertyStates
		{
			public override Tags Tag { get { return Tags.ProgramState; } }

			public ProgramState Item { get; private set; }

			public ProgramStateWrapper(ProgramState item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ProgramState>.Schema;

			public static new ProgramStateWrapper Load(IValueStream stream)
			{
				var temp = Value<ProgramState>.Load(stream);
				return new ProgramStateWrapper(temp);
			}

			public static void Save(IValueSink sink, ProgramStateWrapper value)
			{
				Value<ProgramState>.Save(sink, value.Item);
			}

		}

		public  partial class ReasonForHaltWrapper : PropertyStates
		{
			public override Tags Tag { get { return Tags.ReasonForHalt; } }

			public ProgramError Item { get; private set; }

			public ReasonForHaltWrapper(ProgramError item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ProgramError>.Schema;

			public static new ReasonForHaltWrapper Load(IValueStream stream)
			{
				var temp = Value<ProgramError>.Load(stream);
				return new ReasonForHaltWrapper(temp);
			}

			public static void Save(IValueSink sink, ReasonForHaltWrapper value)
			{
				Value<ProgramError>.Save(sink, value.Item);
			}

		}

		public  partial class ReliabilityWrapper : PropertyStates
		{
			public override Tags Tag { get { return Tags.Reliability; } }

			public Reliability Item { get; private set; }

			public ReliabilityWrapper(Reliability item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Reliability>.Schema;

			public static new ReliabilityWrapper Load(IValueStream stream)
			{
				var temp = Value<Reliability>.Load(stream);
				return new ReliabilityWrapper(temp);
			}

			public static void Save(IValueSink sink, ReliabilityWrapper value)
			{
				Value<Reliability>.Save(sink, value.Item);
			}

		}

		public  partial class StateWrapper : PropertyStates
		{
			public override Tags Tag { get { return Tags.State; } }

			public EventState Item { get; private set; }

			public StateWrapper(EventState item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<EventState>.Schema;

			public static new StateWrapper Load(IValueStream stream)
			{
				var temp = Value<EventState>.Load(stream);
				return new StateWrapper(temp);
			}

			public static void Save(IValueSink sink, StateWrapper value)
			{
				Value<EventState>.Save(sink, value.Item);
			}

		}

		public  partial class SystemStatusWrapper : PropertyStates
		{
			public override Tags Tag { get { return Tags.SystemStatus; } }

			public DeviceStatus Item { get; private set; }

			public SystemStatusWrapper(DeviceStatus item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<DeviceStatus>.Schema;

			public static new SystemStatusWrapper Load(IValueStream stream)
			{
				var temp = Value<DeviceStatus>.Load(stream);
				return new SystemStatusWrapper(temp);
			}

			public static void Save(IValueSink sink, SystemStatusWrapper value)
			{
				Value<DeviceStatus>.Save(sink, value.Item);
			}

		}

		public  partial class UnitsWrapper : PropertyStates
		{
			public override Tags Tag { get { return Tags.Units; } }

			public EngineeringUnits Item { get; private set; }

			public UnitsWrapper(EngineeringUnits item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<EngineeringUnits>.Schema;

			public static new UnitsWrapper Load(IValueStream stream)
			{
				var temp = Value<EngineeringUnits>.Load(stream);
				return new UnitsWrapper(temp);
			}

			public static void Save(IValueSink sink, UnitsWrapper value)
			{
				Value<EngineeringUnits>.Save(sink, value.Item);
			}

		}

		public  partial class UnsignedValueWrapper : PropertyStates
		{
			public override Tags Tag { get { return Tags.UnsignedValue; } }

			public uint Item { get; private set; }

			public UnsignedValueWrapper(uint item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<uint>.Schema;

			public static new UnsignedValueWrapper Load(IValueStream stream)
			{
				var temp = Value<uint>.Load(stream);
				return new UnsignedValueWrapper(temp);
			}

			public static void Save(IValueSink sink, UnsignedValueWrapper value)
			{
				Value<uint>.Save(sink, value.Item);
			}

		}

		public  partial class LifeSafetyModeWrapper : PropertyStates
		{
			public override Tags Tag { get { return Tags.LifeSafetyMode; } }

			public LifeSafetyMode Item { get; private set; }

			public LifeSafetyModeWrapper(LifeSafetyMode item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<LifeSafetyMode>.Schema;

			public static new LifeSafetyModeWrapper Load(IValueStream stream)
			{
				var temp = Value<LifeSafetyMode>.Load(stream);
				return new LifeSafetyModeWrapper(temp);
			}

			public static void Save(IValueSink sink, LifeSafetyModeWrapper value)
			{
				Value<LifeSafetyMode>.Save(sink, value.Item);
			}

		}

		public  partial class LifeSafetyStateWrapper : PropertyStates
		{
			public override Tags Tag { get { return Tags.LifeSafetyState; } }

			public LifeSafetyState Item { get; private set; }

			public LifeSafetyStateWrapper(LifeSafetyState item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<LifeSafetyState>.Schema;

			public static new LifeSafetyStateWrapper Load(IValueStream stream)
			{
				var temp = Value<LifeSafetyState>.Load(stream);
				return new LifeSafetyStateWrapper(temp);
			}

			public static void Save(IValueSink sink, LifeSafetyStateWrapper value)
			{
				Value<LifeSafetyState>.Save(sink, value.Item);
			}

		}
	}
}
