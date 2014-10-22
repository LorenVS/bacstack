using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public abstract  partial class NewValue
	{
		public abstract Tags Tag { get; }

		public bool IsChangedBits { get { return this.Tag == Tags.ChangedBits; } }

		public BitString56 AsChangedBits { get { return ((ChangedBitsWrapper)this).Item; } }

		public static NewValue NewChangedBits(BitString56 changedBits)
		{
			return new ChangedBitsWrapper(changedBits);
		}

		public bool IsChangedValue { get { return this.Tag == Tags.ChangedValue; } }

		public float AsChangedValue { get { return ((ChangedValueWrapper)this).Item; } }

		public static NewValue NewChangedValue(float changedValue)
		{
			return new ChangedValueWrapper(changedValue);
		}

		public static readonly ISchema Schema = new ChoiceSchema(false, 
			new FieldSchema("ChangedBits", 0, Value<BitString56>.Schema),
			new FieldSchema("ChangedValue", 1, Value<float>.Schema));

		public static NewValue Load(IValueStream stream)
		{
			NewValue ret = null;
			Tags tag = (Tags)stream.EnterChoice();
			switch(tag)
			{
				case Tags.ChangedBits:
					ret = Value<ChangedBitsWrapper>.Load(stream);
					break;
				case Tags.ChangedValue:
					ret = Value<ChangedValueWrapper>.Load(stream);
					break;
				default:
					throw new Exception();
			}
			stream.LeaveChoice();
			return ret;
		}

		public static void Save(IValueSink sink, NewValue value)
		{
			sink.EnterChoice((byte)value.Tag);
			switch(value.Tag)
			{
				case Tags.ChangedBits:
					Value<ChangedBitsWrapper>.Save(sink, (ChangedBitsWrapper)value);
					break;
				case Tags.ChangedValue:
					Value<ChangedValueWrapper>.Save(sink, (ChangedValueWrapper)value);
					break;
				default:
					throw new Exception();
			}
			sink.LeaveChoice();
		}

		public enum Tags : byte
		{
			ChangedBits = 0,
			ChangedValue = 1
		}

		public  partial class ChangedBitsWrapper : NewValue
		{
			public override Tags Tag { get { return Tags.ChangedBits; } }

			public BitString56 Item { get; private set; }

			public ChangedBitsWrapper(BitString56 item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<BitString56>.Schema;

			public static new ChangedBitsWrapper Load(IValueStream stream)
			{
				var temp = Value<BitString56>.Load(stream);
				return new ChangedBitsWrapper(temp);
			}

			public static void Save(IValueSink sink, ChangedBitsWrapper value)
			{
				Value<BitString56>.Save(sink, value.Item);
			}

		}

		public  partial class ChangedValueWrapper : NewValue
		{
			public override Tags Tag { get { return Tags.ChangedValue; } }

			public float Item { get; private set; }

			public ChangedValueWrapper(float item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<float>.Schema;

			public static new ChangedValueWrapper Load(IValueStream stream)
			{
				var temp = Value<float>.Load(stream);
				return new ChangedValueWrapper(temp);
			}

			public static void Save(IValueSink sink, ChangedValueWrapper value)
			{
				Value<float>.Save(sink, value.Item);
			}

		}
	}
}
