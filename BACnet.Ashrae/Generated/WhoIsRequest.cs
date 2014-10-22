using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class WhoIsRequest
	{
		public Option<uint> DeviceInstanceRangeLowLimit { get; private set; }

		public Option<uint> DeviceInstanceRangeHighLimit { get; private set; }

		public WhoIsRequest(Option<uint> deviceInstanceRangeLowLimit, Option<uint> deviceInstanceRangeHighLimit)
		{
			this.DeviceInstanceRangeLowLimit = deviceInstanceRangeLowLimit;
			this.DeviceInstanceRangeHighLimit = deviceInstanceRangeHighLimit;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("DeviceInstanceRangeLowLimit", 0, Value<Option<uint>>.Schema),
			new FieldSchema("DeviceInstanceRangeHighLimit", 1, Value<Option<uint>>.Schema));

		public static WhoIsRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var deviceInstanceRangeLowLimit = Value<Option<uint>>.Load(stream);
			var deviceInstanceRangeHighLimit = Value<Option<uint>>.Load(stream);
			stream.LeaveSequence();
			return new WhoIsRequest(deviceInstanceRangeLowLimit, deviceInstanceRangeHighLimit);
		}

		public static void Save(IValueSink sink, WhoIsRequest value)
		{
			sink.EnterSequence();
			Value<Option<uint>>.Save(sink, value.DeviceInstanceRangeLowLimit);
			Value<Option<uint>>.Save(sink, value.DeviceInstanceRangeHighLimit);
			sink.LeaveSequence();
		}
	}
}
