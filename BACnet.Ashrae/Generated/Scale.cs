using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public abstract  partial class Scale
	{
		public abstract Tags Tag { get; }

		public bool IsFloatScale { get { return this.Tag == Tags.FloatScale; } }

		public float AsFloatScale { get { return ((FloatScaleWrapper)this).Item; } }

		public static Scale NewFloatScale(float floatScale)
		{
			return new FloatScaleWrapper(floatScale);
		}

		public bool IsIntegerScale { get { return this.Tag == Tags.IntegerScale; } }

		public int AsIntegerScale { get { return ((IntegerScaleWrapper)this).Item; } }

		public static Scale NewIntegerScale(int integerScale)
		{
			return new IntegerScaleWrapper(integerScale);
		}

		public static readonly ISchema Schema = new ChoiceSchema(false, 
			new FieldSchema("FloatScale", 0, Value<float>.Schema),
			new FieldSchema("IntegerScale", 1, Value<int>.Schema));

		public static Scale Load(IValueStream stream)
		{
			Scale ret = null;
			Tags tag = (Tags)stream.EnterChoice();
			switch(tag)
			{
				case Tags.FloatScale:
					ret = Value<FloatScaleWrapper>.Load(stream);
					break;
				case Tags.IntegerScale:
					ret = Value<IntegerScaleWrapper>.Load(stream);
					break;
				default:
					throw new Exception();
			}
			stream.LeaveChoice();
			return ret;
		}

		public static void Save(IValueSink sink, Scale value)
		{
			sink.EnterChoice((byte)value.Tag);
			switch(value.Tag)
			{
				case Tags.FloatScale:
					Value<FloatScaleWrapper>.Save(sink, (FloatScaleWrapper)value);
					break;
				case Tags.IntegerScale:
					Value<IntegerScaleWrapper>.Save(sink, (IntegerScaleWrapper)value);
					break;
				default:
					throw new Exception();
			}
			sink.LeaveChoice();
		}

		public enum Tags : byte
		{
			FloatScale = 0,
			IntegerScale = 1
		}

		public  partial class FloatScaleWrapper : Scale
		{
			public override Tags Tag { get { return Tags.FloatScale; } }

			public float Item { get; private set; }

			public FloatScaleWrapper(float item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<float>.Schema;

			public static new FloatScaleWrapper Load(IValueStream stream)
			{
				var temp = Value<float>.Load(stream);
				return new FloatScaleWrapper(temp);
			}

			public static void Save(IValueSink sink, FloatScaleWrapper value)
			{
				Value<float>.Save(sink, value.Item);
			}

		}

		public  partial class IntegerScaleWrapper : Scale
		{
			public override Tags Tag { get { return Tags.IntegerScale; } }

			public int Item { get; private set; }

			public IntegerScaleWrapper(int item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<int>.Schema;

			public static new IntegerScaleWrapper Load(IValueStream stream)
			{
				var temp = Value<int>.Load(stream);
				return new IntegerScaleWrapper(temp);
			}

			public static void Save(IValueSink sink, IntegerScaleWrapper value)
			{
				Value<int>.Save(sink, value.Item);
			}

		}
	}
}
