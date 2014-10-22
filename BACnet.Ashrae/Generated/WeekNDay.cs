using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class WeekNDay
	{
		public byte[] Item { get; private set; }

		public WeekNDay(byte[] item)
		{
			this.Item = item;
		}

		public static readonly ISchema Schema = Value<byte[]>.Schema;

		public static WeekNDay Load(IValueStream stream)
		{
			var temp = Value<byte[]>.Load(stream);
			return new WeekNDay(temp);
		}

		public static void Save(IValueSink sink, WeekNDay value)
		{
			Value<byte[]>.Save(sink, value.Item);
		}

	}
}
