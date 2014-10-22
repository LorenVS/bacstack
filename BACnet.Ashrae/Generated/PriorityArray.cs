using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class PriorityArray
	{
		public ReadOnlyArray<PriorityValue> Item { get; private set; }

		public PriorityArray(ReadOnlyArray<PriorityValue> item)
		{
			this.Item = item;
		}

		public static readonly ISchema Schema = Value<ReadOnlyArray<PriorityValue>>.Schema;

		public static PriorityArray Load(IValueStream stream)
		{
			var temp = Value<ReadOnlyArray<PriorityValue>>.Load(stream);
			return new PriorityArray(temp);
		}

		public static void Save(IValueSink sink, PriorityArray value)
		{
			Value<ReadOnlyArray<PriorityValue>>.Save(sink, value.Item);
		}

	}
}
