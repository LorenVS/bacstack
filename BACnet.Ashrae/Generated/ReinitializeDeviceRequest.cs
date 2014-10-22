using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class ReinitializeDeviceRequest
	{
		public ReinitializedStateOfDeviceType ReinitializedStateOfDevice { get; private set; }

		public Option<string> Password { get; private set; }

		public ReinitializeDeviceRequest(ReinitializedStateOfDeviceType reinitializedStateOfDevice, Option<string> password)
		{
			this.ReinitializedStateOfDevice = reinitializedStateOfDevice;
			this.Password = password;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ReinitializedStateOfDevice", 0, Value<ReinitializedStateOfDeviceType>.Schema),
			new FieldSchema("Password", 1, Value<Option<string>>.Schema));

		public static ReinitializeDeviceRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var reinitializedStateOfDevice = Value<ReinitializedStateOfDeviceType>.Load(stream);
			var password = Value<Option<string>>.Load(stream);
			stream.LeaveSequence();
			return new ReinitializeDeviceRequest(reinitializedStateOfDevice, password);
		}

		public static void Save(IValueSink sink, ReinitializeDeviceRequest value)
		{
			sink.EnterSequence();
			Value<ReinitializedStateOfDeviceType>.Save(sink, value.ReinitializedStateOfDevice);
			Value<Option<string>>.Save(sink, value.Password);
			sink.LeaveSequence();
		}
		public enum ReinitializedStateOfDeviceType : uint
		{
			Coldstart = 0,
			Warmstart = 1,
			Startbackup = 2,
			Endbackup = 3,
			Startrestore = 4,
			Endrestore = 5,
			Abortrestore = 6
		}
	}
}
