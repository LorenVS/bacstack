using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class DeviceCommunicationControlRequest
	{
		public Option<ushort> TimeDuration { get; private set; }

		public EnableDisableType EnableDisable { get; private set; }

		public Option<string> Password { get; private set; }

		public DeviceCommunicationControlRequest(Option<ushort> timeDuration, EnableDisableType enableDisable, Option<string> password)
		{
			this.TimeDuration = timeDuration;
			this.EnableDisable = enableDisable;
			this.Password = password;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("TimeDuration", 0, Value<Option<ushort>>.Schema),
			new FieldSchema("EnableDisable", 1, Value<EnableDisableType>.Schema),
			new FieldSchema("Password", 2, Value<Option<string>>.Schema));

		public static DeviceCommunicationControlRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var timeDuration = Value<Option<ushort>>.Load(stream);
			var enableDisable = Value<EnableDisableType>.Load(stream);
			var password = Value<Option<string>>.Load(stream);
			stream.LeaveSequence();
			return new DeviceCommunicationControlRequest(timeDuration, enableDisable, password);
		}

		public static void Save(IValueSink sink, DeviceCommunicationControlRequest value)
		{
			sink.EnterSequence();
			Value<Option<ushort>>.Save(sink, value.TimeDuration);
			Value<EnableDisableType>.Save(sink, value.EnableDisable);
			Value<Option<string>>.Save(sink, value.Password);
			sink.LeaveSequence();
		}
		public enum EnableDisableType : uint
		{
			Enable = 0,
			Disable = 1,
			DisableInitiation = 2
		}
	}
}
