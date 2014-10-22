using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class WhoHasRequest
	{
		public Option<LimitsType> Limits { get; private set; }

		public ObjectType Object { get; private set; }

		public WhoHasRequest(Option<LimitsType> limits, ObjectType @object)
		{
			this.Limits = limits;
			this.Object = @object;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("Limits", 255, Value<Option<LimitsType>>.Schema),
			new FieldSchema("Object", 255, Value<ObjectType>.Schema));

		public static WhoHasRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var limits = Value<Option<LimitsType>>.Load(stream);
			var @object = Value<ObjectType>.Load(stream);
			stream.LeaveSequence();
			return new WhoHasRequest(limits, @object);
		}

		public static void Save(IValueSink sink, WhoHasRequest value)
		{
			sink.EnterSequence();
			Value<Option<LimitsType>>.Save(sink, value.Limits);
			Value<ObjectType>.Save(sink, value.Object);
			sink.LeaveSequence();
		}
		public  partial class LimitsType
		{
			public uint DeviceInstanceRangeLowLimit { get; private set; }

			public uint DeviceInstanceRangeHighLimit { get; private set; }

			public LimitsType(uint deviceInstanceRangeLowLimit, uint deviceInstanceRangeHighLimit)
			{
				this.DeviceInstanceRangeLowLimit = deviceInstanceRangeLowLimit;
				this.DeviceInstanceRangeHighLimit = deviceInstanceRangeHighLimit;
			}

			public static readonly ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("DeviceInstanceRangeLowLimit", 0, Value<uint>.Schema),
				new FieldSchema("DeviceInstanceRangeHighLimit", 1, Value<uint>.Schema));

			public static LimitsType Load(IValueStream stream)
			{
				stream.EnterSequence();
				var deviceInstanceRangeLowLimit = Value<uint>.Load(stream);
				var deviceInstanceRangeHighLimit = Value<uint>.Load(stream);
				stream.LeaveSequence();
				return new LimitsType(deviceInstanceRangeLowLimit, deviceInstanceRangeHighLimit);
			}

			public static void Save(IValueSink sink, LimitsType value)
			{
				sink.EnterSequence();
				Value<uint>.Save(sink, value.DeviceInstanceRangeLowLimit);
				Value<uint>.Save(sink, value.DeviceInstanceRangeHighLimit);
				sink.LeaveSequence();
			}
		}

		public enum Tags : byte
		{
			ObjectIdentifier = 0,
			ObjectName = 1
		}

		public abstract  partial class ObjectType
		{
			public abstract Tags Tag { get; }

			public bool IsObjectIdentifier { get { return this.Tag == Tags.ObjectIdentifier; } }

			public ObjectId AsObjectIdentifier { get { return ((ObjectIdentifierWrapper)this).Item; } }

			public static ObjectType NewObjectIdentifier(ObjectId objectIdentifier)
			{
				return new ObjectIdentifierWrapper(objectIdentifier);
			}

			public bool IsObjectName { get { return this.Tag == Tags.ObjectName; } }

			public string AsObjectName { get { return ((ObjectNameWrapper)this).Item; } }

			public static ObjectType NewObjectName(string objectName)
			{
				return new ObjectNameWrapper(objectName);
			}

			public static readonly ISchema Schema = new ChoiceSchema(false, 
				new FieldSchema("ObjectIdentifier", 2, Value<ObjectId>.Schema),
				new FieldSchema("ObjectName", 3, Value<string>.Schema));

			public static ObjectType Load(IValueStream stream)
			{
				ObjectType ret = null;
				Tags tag = (Tags)stream.EnterChoice();
				switch(tag)
				{
					case Tags.ObjectIdentifier:
						ret = Value<ObjectIdentifierWrapper>.Load(stream);
						break;
					case Tags.ObjectName:
						ret = Value<ObjectNameWrapper>.Load(stream);
						break;
					default:
						throw new Exception();
				}
				stream.LeaveChoice();
				return ret;
			}

			public static void Save(IValueSink sink, ObjectType value)
			{
				sink.EnterChoice((byte)value.Tag);
				switch(value.Tag)
				{
					case Tags.ObjectIdentifier:
						Value<ObjectIdentifierWrapper>.Save(sink, (ObjectIdentifierWrapper)value);
						break;
					case Tags.ObjectName:
						Value<ObjectNameWrapper>.Save(sink, (ObjectNameWrapper)value);
						break;
					default:
						throw new Exception();
				}
				sink.LeaveChoice();
			}
		}

		public  partial class ObjectIdentifierWrapper : ObjectType
		{
			public override Tags Tag { get { return Tags.ObjectIdentifier; } }

			public ObjectId Item { get; private set; }

			public ObjectIdentifierWrapper(ObjectId item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ObjectId>.Schema;

			public static new ObjectIdentifierWrapper Load(IValueStream stream)
			{
				var temp = Value<ObjectId>.Load(stream);
				return new ObjectIdentifierWrapper(temp);
			}

			public static void Save(IValueSink sink, ObjectIdentifierWrapper value)
			{
				Value<ObjectId>.Save(sink, value.Item);
			}

		}

		public  partial class ObjectNameWrapper : ObjectType
		{
			public override Tags Tag { get { return Tags.ObjectName; } }

			public string Item { get; private set; }

			public ObjectNameWrapper(string item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<string>.Schema;

			public static new ObjectNameWrapper Load(IValueStream stream)
			{
				var temp = Value<string>.Load(stream);
				return new ObjectNameWrapper(temp);
			}

			public static void Save(IValueSink sink, ObjectNameWrapper value)
			{
				Value<string>.Save(sink, value.Item);
			}

		}
	}
}
