using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class LogRecord
	{
		public DateAndTime Timestamp { get; private set; }

		public LogDatumType LogDatum { get; private set; }

		public Option<StatusFlags> StatusFlags { get; private set; }

		public LogRecord(DateAndTime timestamp, LogDatumType logDatum, Option<StatusFlags> statusFlags)
		{
			this.Timestamp = timestamp;
			this.LogDatum = logDatum;
			this.StatusFlags = statusFlags;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("Timestamp", 0, Value<DateAndTime>.Schema),
			new FieldSchema("LogDatum", 1, Value<LogDatumType>.Schema),
			new FieldSchema("StatusFlags", 2, Value<Option<StatusFlags>>.Schema));

		public static LogRecord Load(IValueStream stream)
		{
			stream.EnterSequence();
			var timestamp = Value<DateAndTime>.Load(stream);
			var logDatum = Value<LogDatumType>.Load(stream);
			var statusFlags = Value<Option<StatusFlags>>.Load(stream);
			stream.LeaveSequence();
			return new LogRecord(timestamp, logDatum, statusFlags);
		}

		public static void Save(IValueSink sink, LogRecord value)
		{
			sink.EnterSequence();
			Value<DateAndTime>.Save(sink, value.Timestamp);
			Value<LogDatumType>.Save(sink, value.LogDatum);
			Value<Option<StatusFlags>>.Save(sink, value.StatusFlags);
			sink.LeaveSequence();
		}

		public enum Tags : byte
		{
			LogStatus = 0,
			BooleanValue = 1,
			RealValue = 2,
			EnumValue = 3,
			UnsignedValue = 4,
			SignedValue = 5,
			BitstringValue = 6,
			NullValue = 7,
			Failure = 8,
			TimeChange = 9,
			AnyValue = 10
		}

		public abstract  partial class LogDatumType
		{
			public abstract Tags Tag { get; }

			public bool IsLogStatus { get { return this.Tag == Tags.LogStatus; } }

			public LogStatus AsLogStatus { get { return ((LogStatusWrapper)this).Item; } }

			public static LogDatumType NewLogStatus(LogStatus logStatus)
			{
				return new LogStatusWrapper(logStatus);
			}

			public bool IsBooleanValue { get { return this.Tag == Tags.BooleanValue; } }

			public bool AsBooleanValue { get { return ((BooleanValueWrapper)this).Item; } }

			public static LogDatumType NewBooleanValue(bool booleanValue)
			{
				return new BooleanValueWrapper(booleanValue);
			}

			public bool IsRealValue { get { return this.Tag == Tags.RealValue; } }

			public float AsRealValue { get { return ((RealValueWrapper)this).Item; } }

			public static LogDatumType NewRealValue(float realValue)
			{
				return new RealValueWrapper(realValue);
			}

			public bool IsEnumValue { get { return this.Tag == Tags.EnumValue; } }

			public Enumerated AsEnumValue { get { return ((EnumValueWrapper)this).Item; } }

			public static LogDatumType NewEnumValue(Enumerated enumValue)
			{
				return new EnumValueWrapper(enumValue);
			}

			public bool IsUnsignedValue { get { return this.Tag == Tags.UnsignedValue; } }

			public uint AsUnsignedValue { get { return ((UnsignedValueWrapper)this).Item; } }

			public static LogDatumType NewUnsignedValue(uint unsignedValue)
			{
				return new UnsignedValueWrapper(unsignedValue);
			}

			public bool IsSignedValue { get { return this.Tag == Tags.SignedValue; } }

			public int AsSignedValue { get { return ((SignedValueWrapper)this).Item; } }

			public static LogDatumType NewSignedValue(int signedValue)
			{
				return new SignedValueWrapper(signedValue);
			}

			public bool IsBitstringValue { get { return this.Tag == Tags.BitstringValue; } }

			public BitString56 AsBitstringValue { get { return ((BitstringValueWrapper)this).Item; } }

			public static LogDatumType NewBitstringValue(BitString56 bitstringValue)
			{
				return new BitstringValueWrapper(bitstringValue);
			}

			public bool IsNullValue { get { return this.Tag == Tags.NullValue; } }

			public Null AsNullValue { get { return ((NullValueWrapper)this).Item; } }

			public static LogDatumType NewNullValue(Null nullValue)
			{
				return new NullValueWrapper(nullValue);
			}

			public bool IsFailure { get { return this.Tag == Tags.Failure; } }

			public Error AsFailure { get { return ((FailureWrapper)this).Item; } }

			public static LogDatumType NewFailure(Error failure)
			{
				return new FailureWrapper(failure);
			}

			public bool IsTimeChange { get { return this.Tag == Tags.TimeChange; } }

			public float AsTimeChange { get { return ((TimeChangeWrapper)this).Item; } }

			public static LogDatumType NewTimeChange(float timeChange)
			{
				return new TimeChangeWrapper(timeChange);
			}

			public bool IsAnyValue { get { return this.Tag == Tags.AnyValue; } }

			public GenericValue AsAnyValue { get { return ((AnyValueWrapper)this).Item; } }

			public static LogDatumType NewAnyValue(GenericValue anyValue)
			{
				return new AnyValueWrapper(anyValue);
			}

			public static readonly ISchema Schema = new ChoiceSchema(false, 
				new FieldSchema("LogStatus", 0, Value<LogStatus>.Schema),
				new FieldSchema("BooleanValue", 1, Value<bool>.Schema),
				new FieldSchema("RealValue", 2, Value<float>.Schema),
				new FieldSchema("EnumValue", 3, Value<Enumerated>.Schema),
				new FieldSchema("UnsignedValue", 4, Value<uint>.Schema),
				new FieldSchema("SignedValue", 5, Value<int>.Schema),
				new FieldSchema("BitstringValue", 6, Value<BitString56>.Schema),
				new FieldSchema("NullValue", 7, Value<Null>.Schema),
				new FieldSchema("Failure", 8, Value<Error>.Schema),
				new FieldSchema("TimeChange", 9, Value<float>.Schema),
				new FieldSchema("AnyValue", 10, Value<GenericValue>.Schema));

			public static LogDatumType Load(IValueStream stream)
			{
				LogDatumType ret = null;
				Tags tag = (Tags)stream.EnterChoice();
				switch(tag)
				{
					case Tags.LogStatus:
						ret = Value<LogStatusWrapper>.Load(stream);
						break;
					case Tags.BooleanValue:
						ret = Value<BooleanValueWrapper>.Load(stream);
						break;
					case Tags.RealValue:
						ret = Value<RealValueWrapper>.Load(stream);
						break;
					case Tags.EnumValue:
						ret = Value<EnumValueWrapper>.Load(stream);
						break;
					case Tags.UnsignedValue:
						ret = Value<UnsignedValueWrapper>.Load(stream);
						break;
					case Tags.SignedValue:
						ret = Value<SignedValueWrapper>.Load(stream);
						break;
					case Tags.BitstringValue:
						ret = Value<BitstringValueWrapper>.Load(stream);
						break;
					case Tags.NullValue:
						ret = Value<NullValueWrapper>.Load(stream);
						break;
					case Tags.Failure:
						ret = Value<FailureWrapper>.Load(stream);
						break;
					case Tags.TimeChange:
						ret = Value<TimeChangeWrapper>.Load(stream);
						break;
					case Tags.AnyValue:
						ret = Value<AnyValueWrapper>.Load(stream);
						break;
					default:
						throw new Exception();
				}
				stream.LeaveChoice();
				return ret;
			}

			public static void Save(IValueSink sink, LogDatumType value)
			{
				sink.EnterChoice((byte)value.Tag);
				switch(value.Tag)
				{
					case Tags.LogStatus:
						Value<LogStatusWrapper>.Save(sink, (LogStatusWrapper)value);
						break;
					case Tags.BooleanValue:
						Value<BooleanValueWrapper>.Save(sink, (BooleanValueWrapper)value);
						break;
					case Tags.RealValue:
						Value<RealValueWrapper>.Save(sink, (RealValueWrapper)value);
						break;
					case Tags.EnumValue:
						Value<EnumValueWrapper>.Save(sink, (EnumValueWrapper)value);
						break;
					case Tags.UnsignedValue:
						Value<UnsignedValueWrapper>.Save(sink, (UnsignedValueWrapper)value);
						break;
					case Tags.SignedValue:
						Value<SignedValueWrapper>.Save(sink, (SignedValueWrapper)value);
						break;
					case Tags.BitstringValue:
						Value<BitstringValueWrapper>.Save(sink, (BitstringValueWrapper)value);
						break;
					case Tags.NullValue:
						Value<NullValueWrapper>.Save(sink, (NullValueWrapper)value);
						break;
					case Tags.Failure:
						Value<FailureWrapper>.Save(sink, (FailureWrapper)value);
						break;
					case Tags.TimeChange:
						Value<TimeChangeWrapper>.Save(sink, (TimeChangeWrapper)value);
						break;
					case Tags.AnyValue:
						Value<AnyValueWrapper>.Save(sink, (AnyValueWrapper)value);
						break;
					default:
						throw new Exception();
				}
				sink.LeaveChoice();
			}
		}

		public  partial class LogStatusWrapper : LogDatumType
		{
			public override Tags Tag { get { return Tags.LogStatus; } }

			public LogStatus Item { get; private set; }

			public LogStatusWrapper(LogStatus item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<LogStatus>.Schema;

			public static new LogStatusWrapper Load(IValueStream stream)
			{
				var temp = Value<LogStatus>.Load(stream);
				return new LogStatusWrapper(temp);
			}

			public static void Save(IValueSink sink, LogStatusWrapper value)
			{
				Value<LogStatus>.Save(sink, value.Item);
			}

		}

		public  partial class BooleanValueWrapper : LogDatumType
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

		public  partial class RealValueWrapper : LogDatumType
		{
			public override Tags Tag { get { return Tags.RealValue; } }

			public float Item { get; private set; }

			public RealValueWrapper(float item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<float>.Schema;

			public static new RealValueWrapper Load(IValueStream stream)
			{
				var temp = Value<float>.Load(stream);
				return new RealValueWrapper(temp);
			}

			public static void Save(IValueSink sink, RealValueWrapper value)
			{
				Value<float>.Save(sink, value.Item);
			}

		}

		public  partial class EnumValueWrapper : LogDatumType
		{
			public override Tags Tag { get { return Tags.EnumValue; } }

			public Enumerated Item { get; private set; }

			public EnumValueWrapper(Enumerated item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Enumerated>.Schema;

			public static new EnumValueWrapper Load(IValueStream stream)
			{
				var temp = Value<Enumerated>.Load(stream);
				return new EnumValueWrapper(temp);
			}

			public static void Save(IValueSink sink, EnumValueWrapper value)
			{
				Value<Enumerated>.Save(sink, value.Item);
			}

		}

		public  partial class UnsignedValueWrapper : LogDatumType
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

		public  partial class SignedValueWrapper : LogDatumType
		{
			public override Tags Tag { get { return Tags.SignedValue; } }

			public int Item { get; private set; }

			public SignedValueWrapper(int item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<int>.Schema;

			public static new SignedValueWrapper Load(IValueStream stream)
			{
				var temp = Value<int>.Load(stream);
				return new SignedValueWrapper(temp);
			}

			public static void Save(IValueSink sink, SignedValueWrapper value)
			{
				Value<int>.Save(sink, value.Item);
			}

		}

		public  partial class BitstringValueWrapper : LogDatumType
		{
			public override Tags Tag { get { return Tags.BitstringValue; } }

			public BitString56 Item { get; private set; }

			public BitstringValueWrapper(BitString56 item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<BitString56>.Schema;

			public static new BitstringValueWrapper Load(IValueStream stream)
			{
				var temp = Value<BitString56>.Load(stream);
				return new BitstringValueWrapper(temp);
			}

			public static void Save(IValueSink sink, BitstringValueWrapper value)
			{
				Value<BitString56>.Save(sink, value.Item);
			}

		}

		public  partial class NullValueWrapper : LogDatumType
		{
			public override Tags Tag { get { return Tags.NullValue; } }

			public Null Item { get; private set; }

			public NullValueWrapper(Null item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Null>.Schema;

			public static new NullValueWrapper Load(IValueStream stream)
			{
				var temp = Value<Null>.Load(stream);
				return new NullValueWrapper(temp);
			}

			public static void Save(IValueSink sink, NullValueWrapper value)
			{
				Value<Null>.Save(sink, value.Item);
			}

		}

		public  partial class FailureWrapper : LogDatumType
		{
			public override Tags Tag { get { return Tags.Failure; } }

			public Error Item { get; private set; }

			public FailureWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new FailureWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new FailureWrapper(temp);
			}

			public static void Save(IValueSink sink, FailureWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class TimeChangeWrapper : LogDatumType
		{
			public override Tags Tag { get { return Tags.TimeChange; } }

			public float Item { get; private set; }

			public TimeChangeWrapper(float item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<float>.Schema;

			public static new TimeChangeWrapper Load(IValueStream stream)
			{
				var temp = Value<float>.Load(stream);
				return new TimeChangeWrapper(temp);
			}

			public static void Save(IValueSink sink, TimeChangeWrapper value)
			{
				Value<float>.Save(sink, value.Item);
			}

		}

		public  partial class AnyValueWrapper : LogDatumType
		{
			public override Tags Tag { get { return Tags.AnyValue; } }

			public GenericValue Item { get; private set; }

			public AnyValueWrapper(GenericValue item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<GenericValue>.Schema;

			public static new AnyValueWrapper Load(IValueStream stream)
			{
				var temp = Value<GenericValue>.Load(stream);
				return new AnyValueWrapper(temp);
			}

			public static void Save(IValueSink sink, AnyValueWrapper value)
			{
				Value<GenericValue>.Save(sink, value.Item);
			}

		}
	}
}
