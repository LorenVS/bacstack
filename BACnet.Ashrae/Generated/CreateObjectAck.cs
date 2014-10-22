using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class CreateObjectAck
	{
		public ObjectId Item { get; private set; }

		public CreateObjectAck(ObjectId item)
		{
			this.Item = item;
		}

		public static readonly ISchema Schema = Value<ObjectId>.Schema;

		public static CreateObjectAck Load(IValueStream stream)
		{
			var temp = Value<ObjectId>.Load(stream);
			return new CreateObjectAck(temp);
		}

		public static void Save(IValueSink sink, CreateObjectAck value)
		{
			Value<ObjectId>.Save(sink, value.Item);
		}

	}
}
