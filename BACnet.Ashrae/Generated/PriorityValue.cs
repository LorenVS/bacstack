using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public abstract  partial class PriorityValue
	{
		public abstract Tags Tag { get; }

		public bool IsNull { get { return this.Tag == Tags.Null; } }

		public Null AsNull { get { return ((NullWrapper)this).Item; } }

		public static PriorityValue NewNull(Null @null)
		{
			return new NullWrapper(@null);
		}

		public bool IsReal { get { return this.Tag == Tags.Real; } }

		public float AsReal { get { return ((RealWrapper)this).Item; } }

		public static PriorityValue NewReal(float real)
		{
			return new RealWrapper(real);
		}

		public bool IsBinary { get { return this.Tag == Tags.Binary; } }

		public BinaryPV AsBinary { get { return ((BinaryWrapper)this).Item; } }

		public static PriorityValue NewBinary(BinaryPV binary)
		{
			return new BinaryWrapper(binary);
		}

		public bool IsInteger { get { return this.Tag == Tags.Integer; } }

		public uint AsInteger { get { return ((IntegerWrapper)this).Item; } }

		public static PriorityValue NewInteger(uint integer)
		{
			return new IntegerWrapper(integer);
		}

		public bool IsConstructedValue { get { return this.Tag == Tags.ConstructedValue; } }

		public GenericValue AsConstructedValue { get { return ((ConstructedValueWrapper)this).Item; } }

		public static PriorityValue NewConstructedValue(GenericValue constructedValue)
		{
			return new ConstructedValueWrapper(constructedValue);
		}

		public static readonly ISchema Schema = new ChoiceSchema(false, 
			new FieldSchema("Null", 255, Value<Null>.Schema),
			new FieldSchema("Real", 255, Value<float>.Schema),
			new FieldSchema("Binary", 255, Value<BinaryPV>.Schema),
			new FieldSchema("Integer", 255, Value<uint>.Schema),
			new FieldSchema("ConstructedValue", 0, Value<GenericValue>.Schema));

		public static PriorityValue Load(IValueStream stream)
		{
			PriorityValue ret = null;
			Tags tag = (Tags)stream.EnterChoice();
			switch(tag)
			{
				case Tags.Null:
					ret = Value<NullWrapper>.Load(stream);
					break;
				case Tags.Real:
					ret = Value<RealWrapper>.Load(stream);
					break;
				case Tags.Binary:
					ret = Value<BinaryWrapper>.Load(stream);
					break;
				case Tags.Integer:
					ret = Value<IntegerWrapper>.Load(stream);
					break;
				case Tags.ConstructedValue:
					ret = Value<ConstructedValueWrapper>.Load(stream);
					break;
				default:
					throw new Exception();
			}
			stream.LeaveChoice();
			return ret;
		}

		public static void Save(IValueSink sink, PriorityValue value)
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
				case Tags.Binary:
					Value<BinaryWrapper>.Save(sink, (BinaryWrapper)value);
					break;
				case Tags.Integer:
					Value<IntegerWrapper>.Save(sink, (IntegerWrapper)value);
					break;
				case Tags.ConstructedValue:
					Value<ConstructedValueWrapper>.Save(sink, (ConstructedValueWrapper)value);
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
			Binary = 2,
			Integer = 3,
			ConstructedValue = 4
		}

		public  partial class NullWrapper : PriorityValue
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

		public  partial class RealWrapper : PriorityValue
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

		public  partial class BinaryWrapper : PriorityValue
		{
			public override Tags Tag { get { return Tags.Binary; } }

			public BinaryPV Item { get; private set; }

			public BinaryWrapper(BinaryPV item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<BinaryPV>.Schema;

			public static new BinaryWrapper Load(IValueStream stream)
			{
				var temp = Value<BinaryPV>.Load(stream);
				return new BinaryWrapper(temp);
			}

			public static void Save(IValueSink sink, BinaryWrapper value)
			{
				Value<BinaryPV>.Save(sink, value.Item);
			}

		}

		public  partial class IntegerWrapper : PriorityValue
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

		public  partial class ConstructedValueWrapper : PriorityValue
		{
			public override Tags Tag { get { return Tags.ConstructedValue; } }

			public GenericValue Item { get; private set; }

			public ConstructedValueWrapper(GenericValue item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<GenericValue>.Schema;

			public static new ConstructedValueWrapper Load(IValueStream stream)
			{
				var temp = Value<GenericValue>.Load(stream);
				return new ConstructedValueWrapper(temp);
			}

			public static void Save(IValueSink sink, ConstructedValueWrapper value)
			{
				Value<GenericValue>.Save(sink, value.Item);
			}

		}
	}
}
