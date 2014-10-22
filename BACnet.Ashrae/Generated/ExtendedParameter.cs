using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public abstract  partial class ExtendedParameter
	{
		public abstract Tags Tag { get; }

		public bool IsNull { get { return this.Tag == Tags.Null; } }

		public Null AsNull { get { return ((NullWrapper)this).Item; } }

		public static ExtendedParameter NewNull(Null @null)
		{
			return new NullWrapper(@null);
		}

		public bool IsReal { get { return this.Tag == Tags.Real; } }

		public float AsReal { get { return ((RealWrapper)this).Item; } }

		public static ExtendedParameter NewReal(float real)
		{
			return new RealWrapper(real);
		}

		public bool IsInteger { get { return this.Tag == Tags.Integer; } }

		public uint AsInteger { get { return ((IntegerWrapper)this).Item; } }

		public static ExtendedParameter NewInteger(uint integer)
		{
			return new IntegerWrapper(integer);
		}

		public bool IsBoolean { get { return this.Tag == Tags.Boolean; } }

		public bool AsBoolean { get { return ((BooleanWrapper)this).Item; } }

		public static ExtendedParameter NewBoolean(bool boolean)
		{
			return new BooleanWrapper(boolean);
		}

		public bool IsDouble { get { return this.Tag == Tags.Double; } }

		public double AsDouble { get { return ((DoubleWrapper)this).Item; } }

		public static ExtendedParameter NewDouble(double @double)
		{
			return new DoubleWrapper(@double);
		}

		public bool IsOctet { get { return this.Tag == Tags.Octet; } }

		public byte[] AsOctet { get { return ((OctetWrapper)this).Item; } }

		public static ExtendedParameter NewOctet(byte[] octet)
		{
			return new OctetWrapper(octet);
		}

		public bool IsBitstring { get { return this.Tag == Tags.Bitstring; } }

		public BitString56 AsBitstring { get { return ((BitstringWrapper)this).Item; } }

		public static ExtendedParameter NewBitstring(BitString56 bitstring)
		{
			return new BitstringWrapper(bitstring);
		}

		public bool IsEnum { get { return this.Tag == Tags.Enum; } }

		public Enumerated AsEnum { get { return ((EnumWrapper)this).Item; } }

		public static ExtendedParameter NewEnum(Enumerated @enum)
		{
			return new EnumWrapper(@enum);
		}

		public bool IsReference { get { return this.Tag == Tags.Reference; } }

		public DeviceObjectPropertyReference AsReference { get { return ((ReferenceWrapper)this).Item; } }

		public static ExtendedParameter NewReference(DeviceObjectPropertyReference reference)
		{
			return new ReferenceWrapper(reference);
		}

		public static readonly ISchema Schema = new ChoiceSchema(false, 
			new FieldSchema("Null", 255, Value<Null>.Schema),
			new FieldSchema("Real", 255, Value<float>.Schema),
			new FieldSchema("Integer", 255, Value<uint>.Schema),
			new FieldSchema("Boolean", 255, Value<bool>.Schema),
			new FieldSchema("Double", 255, Value<double>.Schema),
			new FieldSchema("Octet", 255, Value<byte[]>.Schema),
			new FieldSchema("Bitstring", 255, Value<BitString56>.Schema),
			new FieldSchema("Enum", 255, Value<Enumerated>.Schema),
			new FieldSchema("Reference", 0, Value<DeviceObjectPropertyReference>.Schema));

		public static ExtendedParameter Load(IValueStream stream)
		{
			ExtendedParameter ret = null;
			Tags tag = (Tags)stream.EnterChoice();
			switch(tag)
			{
				case Tags.Null:
					ret = Value<NullWrapper>.Load(stream);
					break;
				case Tags.Real:
					ret = Value<RealWrapper>.Load(stream);
					break;
				case Tags.Integer:
					ret = Value<IntegerWrapper>.Load(stream);
					break;
				case Tags.Boolean:
					ret = Value<BooleanWrapper>.Load(stream);
					break;
				case Tags.Double:
					ret = Value<DoubleWrapper>.Load(stream);
					break;
				case Tags.Octet:
					ret = Value<OctetWrapper>.Load(stream);
					break;
				case Tags.Bitstring:
					ret = Value<BitstringWrapper>.Load(stream);
					break;
				case Tags.Enum:
					ret = Value<EnumWrapper>.Load(stream);
					break;
				case Tags.Reference:
					ret = Value<ReferenceWrapper>.Load(stream);
					break;
				default:
					throw new Exception();
			}
			stream.LeaveChoice();
			return ret;
		}

		public static void Save(IValueSink sink, ExtendedParameter value)
		{
			sink.EnterChoice((byte)value.Tag);
			switch(value.Tag)
			{
				case Tags.Null:
					Value<NullWrapper>.Save(sink, (NullWrapper)value);
					break;
				case Tags.Real:
					Value<RealWrapper>.Save(sink, (RealWrapper)value);
					break;
				case Tags.Integer:
					Value<IntegerWrapper>.Save(sink, (IntegerWrapper)value);
					break;
				case Tags.Boolean:
					Value<BooleanWrapper>.Save(sink, (BooleanWrapper)value);
					break;
				case Tags.Double:
					Value<DoubleWrapper>.Save(sink, (DoubleWrapper)value);
					break;
				case Tags.Octet:
					Value<OctetWrapper>.Save(sink, (OctetWrapper)value);
					break;
				case Tags.Bitstring:
					Value<BitstringWrapper>.Save(sink, (BitstringWrapper)value);
					break;
				case Tags.Enum:
					Value<EnumWrapper>.Save(sink, (EnumWrapper)value);
					break;
				case Tags.Reference:
					Value<ReferenceWrapper>.Save(sink, (ReferenceWrapper)value);
					break;
				default:
					throw new Exception();
			}
			sink.LeaveChoice();
		}

		public enum Tags : byte
		{
			Null = 0,
			Real = 1,
			Integer = 2,
			Boolean = 3,
			Double = 4,
			Octet = 5,
			Bitstring = 6,
			Enum = 7,
			Reference = 8
		}

		public  partial class NullWrapper : ExtendedParameter
		{
			public override Tags Tag { get { return Tags.Null; } }

			public Null Item { get; private set; }

			public NullWrapper(Null item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Null>.Schema;

			public static new NullWrapper Load(IValueStream stream)
			{
				var temp = Value<Null>.Load(stream);
				return new NullWrapper(temp);
			}

			public static void Save(IValueSink sink, NullWrapper value)
			{
				Value<Null>.Save(sink, value.Item);
			}

		}

		public  partial class RealWrapper : ExtendedParameter
		{
			public override Tags Tag { get { return Tags.Real; } }

			public float Item { get; private set; }

			public RealWrapper(float item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<float>.Schema;

			public static new RealWrapper Load(IValueStream stream)
			{
				var temp = Value<float>.Load(stream);
				return new RealWrapper(temp);
			}

			public static void Save(IValueSink sink, RealWrapper value)
			{
				Value<float>.Save(sink, value.Item);
			}

		}

		public  partial class IntegerWrapper : ExtendedParameter
		{
			public override Tags Tag { get { return Tags.Integer; } }

			public uint Item { get; private set; }

			public IntegerWrapper(uint item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<uint>.Schema;

			public static new IntegerWrapper Load(IValueStream stream)
			{
				var temp = Value<uint>.Load(stream);
				return new IntegerWrapper(temp);
			}

			public static void Save(IValueSink sink, IntegerWrapper value)
			{
				Value<uint>.Save(sink, value.Item);
			}

		}

		public  partial class BooleanWrapper : ExtendedParameter
		{
			public override Tags Tag { get { return Tags.Boolean; } }

			public bool Item { get; private set; }

			public BooleanWrapper(bool item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<bool>.Schema;

			public static new BooleanWrapper Load(IValueStream stream)
			{
				var temp = Value<bool>.Load(stream);
				return new BooleanWrapper(temp);
			}

			public static void Save(IValueSink sink, BooleanWrapper value)
			{
				Value<bool>.Save(sink, value.Item);
			}

		}

		public  partial class DoubleWrapper : ExtendedParameter
		{
			public override Tags Tag { get { return Tags.Double; } }

			public double Item { get; private set; }

			public DoubleWrapper(double item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<double>.Schema;

			public static new DoubleWrapper Load(IValueStream stream)
			{
				var temp = Value<double>.Load(stream);
				return new DoubleWrapper(temp);
			}

			public static void Save(IValueSink sink, DoubleWrapper value)
			{
				Value<double>.Save(sink, value.Item);
			}

		}

		public  partial class OctetWrapper : ExtendedParameter
		{
			public override Tags Tag { get { return Tags.Octet; } }

			public byte[] Item { get; private set; }

			public OctetWrapper(byte[] item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<byte[]>.Schema;

			public static new OctetWrapper Load(IValueStream stream)
			{
				var temp = Value<byte[]>.Load(stream);
				return new OctetWrapper(temp);
			}

			public static void Save(IValueSink sink, OctetWrapper value)
			{
				Value<byte[]>.Save(sink, value.Item);
			}

		}

		public  partial class BitstringWrapper : ExtendedParameter
		{
			public override Tags Tag { get { return Tags.Bitstring; } }

			public BitString56 Item { get; private set; }

			public BitstringWrapper(BitString56 item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<BitString56>.Schema;

			public static new BitstringWrapper Load(IValueStream stream)
			{
				var temp = Value<BitString56>.Load(stream);
				return new BitstringWrapper(temp);
			}

			public static void Save(IValueSink sink, BitstringWrapper value)
			{
				Value<BitString56>.Save(sink, value.Item);
			}

		}

		public  partial class EnumWrapper : ExtendedParameter
		{
			public override Tags Tag { get { return Tags.Enum; } }

			public Enumerated Item { get; private set; }

			public EnumWrapper(Enumerated item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Enumerated>.Schema;

			public static new EnumWrapper Load(IValueStream stream)
			{
				var temp = Value<Enumerated>.Load(stream);
				return new EnumWrapper(temp);
			}

			public static void Save(IValueSink sink, EnumWrapper value)
			{
				Value<Enumerated>.Save(sink, value.Item);
			}

		}

		public  partial class ReferenceWrapper : ExtendedParameter
		{
			public override Tags Tag { get { return Tags.Reference; } }

			public DeviceObjectPropertyReference Item { get; private set; }

			public ReferenceWrapper(DeviceObjectPropertyReference item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<DeviceObjectPropertyReference>.Schema;

			public static new ReferenceWrapper Load(IValueStream stream)
			{
				var temp = Value<DeviceObjectPropertyReference>.Load(stream);
				return new ReferenceWrapper(temp);
			}

			public static void Save(IValueSink sink, ReferenceWrapper value)
			{
				Value<DeviceObjectPropertyReference>.Save(sink, value.Item);
			}

		}
	}
}
