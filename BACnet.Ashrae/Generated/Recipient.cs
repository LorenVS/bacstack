using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public abstract  partial class Recipient
	{
		public abstract Tags Tag { get; }

		public bool IsDevice { get { return this.Tag == Tags.Device; } }

		public ObjectId AsDevice { get { return ((DeviceWrapper)this).Item; } }

		public static Recipient NewDevice(ObjectId device)
		{
			return new DeviceWrapper(device);
		}

		public bool IsAddress { get { return this.Tag == Tags.Address; } }

		public NetworkAddress AsAddress { get { return ((AddressWrapper)this).Item; } }

		public static Recipient NewAddress(NetworkAddress address)
		{
			return new AddressWrapper(address);
		}

		public static readonly ISchema Schema = new ChoiceSchema(false, 
			new FieldSchema("Device", 0, Value<ObjectId>.Schema),
			new FieldSchema("Address", 1, Value<NetworkAddress>.Schema));

		public static Recipient Load(IValueStream stream)
		{
			Recipient ret = null;
			Tags tag = (Tags)stream.EnterChoice();
			switch(tag)
			{
				case Tags.Device:
					ret = Value<DeviceWrapper>.Load(stream);
					break;
				case Tags.Address:
					ret = Value<AddressWrapper>.Load(stream);
					break;
				default:
					throw new Exception();
			}
			stream.LeaveChoice();
			return ret;
		}

		public static void Save(IValueSink sink, Recipient value)
		{
			sink.EnterChoice((byte)value.Tag);
			switch(value.Tag)
			{
				case Tags.Device:
					Value<DeviceWrapper>.Save(sink, (DeviceWrapper)value);
					break;
				case Tags.Address:
					Value<AddressWrapper>.Save(sink, (AddressWrapper)value);
					break;
				default:
					throw new Exception();
			}
			sink.LeaveChoice();
		}

		public enum Tags : byte
		{
			Device = 0,
			Address = 1
		}

		public  partial class DeviceWrapper : Recipient
		{
			public override Tags Tag { get { return Tags.Device; } }

			public ObjectId Item { get; private set; }

			public DeviceWrapper(ObjectId item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ObjectId>.Schema;

			public static new DeviceWrapper Load(IValueStream stream)
			{
				var temp = Value<ObjectId>.Load(stream);
				return new DeviceWrapper(temp);
			}

			public static void Save(IValueSink sink, DeviceWrapper value)
			{
				Value<ObjectId>.Save(sink, value.Item);
			}

		}

		public  partial class AddressWrapper : Recipient
		{
			public override Tags Tag { get { return Tags.Address; } }

			public NetworkAddress Item { get; private set; }

			public AddressWrapper(NetworkAddress item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<NetworkAddress>.Schema;

			public static new AddressWrapper Load(IValueStream stream)
			{
				var temp = Value<NetworkAddress>.Load(stream);
				return new AddressWrapper(temp);
			}

			public static void Save(IValueSink sink, AddressWrapper value)
			{
				Value<NetworkAddress>.Save(sink, value.Item);
			}

		}
	}
}
