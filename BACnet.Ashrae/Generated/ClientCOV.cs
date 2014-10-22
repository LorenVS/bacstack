using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public abstract  partial class ClientCOV
	{
		public abstract Tags Tag { get; }

		public bool IsRealIncrement { get { return this.Tag == Tags.RealIncrement; } }

		public float AsRealIncrement { get { return ((RealIncrementWrapper)this).Item; } }

		public static ClientCOV NewRealIncrement(float realIncrement)
		{
			return new RealIncrementWrapper(realIncrement);
		}

		public bool IsDefaultIncrement { get { return this.Tag == Tags.DefaultIncrement; } }

		public Null AsDefaultIncrement { get { return ((DefaultIncrementWrapper)this).Item; } }

		public static ClientCOV NewDefaultIncrement(Null defaultIncrement)
		{
			return new DefaultIncrementWrapper(defaultIncrement);
		}

		public static readonly ISchema Schema = new ChoiceSchema(false, 
			new FieldSchema("RealIncrement", 255, Value<float>.Schema),
			new FieldSchema("DefaultIncrement", 255, Value<Null>.Schema));

		public static ClientCOV Load(IValueStream stream)
		{
			ClientCOV ret = null;
			Tags tag = (Tags)stream.EnterChoice();
			switch(tag)
			{
				case Tags.RealIncrement:
					ret = Value<RealIncrementWrapper>.Load(stream);
					break;
				case Tags.DefaultIncrement:
					ret = Value<DefaultIncrementWrapper>.Load(stream);
					break;
				default:
					throw new Exception();
			}
			stream.LeaveChoice();
			return ret;
		}

		public static void Save(IValueSink sink, ClientCOV value)
		{
			sink.EnterChoice((byte)value.Tag);
			switch(value.Tag)
			{
				case Tags.RealIncrement:
					Value<RealIncrementWrapper>.Save(sink, (RealIncrementWrapper)value);
					break;
				case Tags.DefaultIncrement:
					Value<DefaultIncrementWrapper>.Save(sink, (DefaultIncrementWrapper)value);
					break;
				default:
					throw new Exception();
			}
			sink.LeaveChoice();
		}

		public enum Tags : byte
		{
			RealIncrement = 0,
			DefaultIncrement = 1
		}

		public  partial class RealIncrementWrapper : ClientCOV
		{
			public override Tags Tag { get { return Tags.RealIncrement; } }

			public float Item { get; private set; }

			public RealIncrementWrapper(float item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<float>.Schema;

			public static new RealIncrementWrapper Load(IValueStream stream)
			{
				var temp = Value<float>.Load(stream);
				return new RealIncrementWrapper(temp);
			}

			public static void Save(IValueSink sink, RealIncrementWrapper value)
			{
				Value<float>.Save(sink, value.Item);
			}

		}

		public  partial class DefaultIncrementWrapper : ClientCOV
		{
			public override Tags Tag { get { return Tags.DefaultIncrement; } }

			public Null Item { get; private set; }

			public DefaultIncrementWrapper(Null item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Null>.Schema;

			public static new DefaultIncrementWrapper Load(IValueStream stream)
			{
				var temp = Value<Null>.Load(stream);
				return new DefaultIncrementWrapper(temp);
			}

			public static void Save(IValueSink sink, DefaultIncrementWrapper value)
			{
				Value<Null>.Save(sink, value.Item);
			}

		}
	}
}
