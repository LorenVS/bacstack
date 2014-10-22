using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public abstract  partial class COVCriteria
	{
		public abstract Tags Tag { get; }

		public bool IsBitmask { get { return this.Tag == Tags.Bitmask; } }

		public BitString56 AsBitmask { get { return ((BitmaskWrapper)this).Item; } }

		public static COVCriteria NewBitmask(BitString56 bitmask)
		{
			return new BitmaskWrapper(bitmask);
		}

		public bool IsReferencedPropertyIncrement { get { return this.Tag == Tags.ReferencedPropertyIncrement; } }

		public float AsReferencedPropertyIncrement { get { return ((ReferencedPropertyIncrementWrapper)this).Item; } }

		public static COVCriteria NewReferencedPropertyIncrement(float referencedPropertyIncrement)
		{
			return new ReferencedPropertyIncrementWrapper(referencedPropertyIncrement);
		}

		public static readonly ISchema Schema = new ChoiceSchema(false, 
			new FieldSchema("Bitmask", 0, Value<BitString56>.Schema),
			new FieldSchema("ReferencedPropertyIncrement", 1, Value<float>.Schema));

		public static COVCriteria Load(IValueStream stream)
		{
			COVCriteria ret = null;
			Tags tag = (Tags)stream.EnterChoice();
			switch(tag)
			{
				case Tags.Bitmask:
					ret = Value<BitmaskWrapper>.Load(stream);
					break;
				case Tags.ReferencedPropertyIncrement:
					ret = Value<ReferencedPropertyIncrementWrapper>.Load(stream);
					break;
				default:
					throw new Exception();
			}
			stream.LeaveChoice();
			return ret;
		}

		public static void Save(IValueSink sink, COVCriteria value)
		{
			sink.EnterChoice((byte)value.Tag);
			switch(value.Tag)
			{
				case Tags.Bitmask:
					Value<BitmaskWrapper>.Save(sink, (BitmaskWrapper)value);
					break;
				case Tags.ReferencedPropertyIncrement:
					Value<ReferencedPropertyIncrementWrapper>.Save(sink, (ReferencedPropertyIncrementWrapper)value);
					break;
				default:
					throw new Exception();
			}
			sink.LeaveChoice();
		}

		public enum Tags : byte
		{
			Bitmask = 0,
			ReferencedPropertyIncrement = 1
		}

		public  partial class BitmaskWrapper : COVCriteria
		{
			public override Tags Tag { get { return Tags.Bitmask; } }

			public BitString56 Item { get; private set; }

			public BitmaskWrapper(BitString56 item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<BitString56>.Schema;

			public static new BitmaskWrapper Load(IValueStream stream)
			{
				var temp = Value<BitString56>.Load(stream);
				return new BitmaskWrapper(temp);
			}

			public static void Save(IValueSink sink, BitmaskWrapper value)
			{
				Value<BitString56>.Save(sink, value.Item);
			}

		}

		public  partial class ReferencedPropertyIncrementWrapper : COVCriteria
		{
			public override Tags Tag { get { return Tags.ReferencedPropertyIncrement; } }

			public float Item { get; private set; }

			public ReferencedPropertyIncrementWrapper(float item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<float>.Schema;

			public static new ReferencedPropertyIncrementWrapper Load(IValueStream stream)
			{
				var temp = Value<float>.Load(stream);
				return new ReferencedPropertyIncrementWrapper(temp);
			}

			public static void Save(IValueSink sink, ReferencedPropertyIncrementWrapper value)
			{
				Value<float>.Save(sink, value.Item);
			}

		}
	}
}
