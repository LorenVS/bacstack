using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class CreateObjectRequest
	{
		public ObjectSpecifierType ObjectSpecifier { get; private set; }

		public Option<ReadOnlyArray<PropertyValue>> ListOfInitialValues { get; private set; }

		public CreateObjectRequest(ObjectSpecifierType objectSpecifier, Option<ReadOnlyArray<PropertyValue>> listOfInitialValues)
		{
			this.ObjectSpecifier = objectSpecifier;
			this.ListOfInitialValues = listOfInitialValues;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ObjectSpecifier", 0, Value<ObjectSpecifierType>.Schema),
			new FieldSchema("ListOfInitialValues", 1, Value<Option<ReadOnlyArray<PropertyValue>>>.Schema));

		public static CreateObjectRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var objectSpecifier = Value<ObjectSpecifierType>.Load(stream);
			var listOfInitialValues = Value<Option<ReadOnlyArray<PropertyValue>>>.Load(stream);
			stream.LeaveSequence();
			return new CreateObjectRequest(objectSpecifier, listOfInitialValues);
		}

		public static void Save(IValueSink sink, CreateObjectRequest value)
		{
			sink.EnterSequence();
			Value<ObjectSpecifierType>.Save(sink, value.ObjectSpecifier);
			Value<Option<ReadOnlyArray<PropertyValue>>>.Save(sink, value.ListOfInitialValues);
			sink.LeaveSequence();
		}

		public enum Tags : byte
		{
			ObjectType = 0,
			ObjectIdentifier = 1
		}

		public abstract  partial class ObjectSpecifierType
		{
			public abstract Tags Tag { get; }

			public bool IsObjectType { get { return this.Tag == Tags.ObjectType; } }

			public ObjectType AsObjectType { get { return ((ObjectTypeWrapper)this).Item; } }

			public static ObjectSpecifierType NewObjectType(ObjectType objectType)
			{
				return new ObjectTypeWrapper(objectType);
			}

			public bool IsObjectIdentifier { get { return this.Tag == Tags.ObjectIdentifier; } }

			public ObjectId AsObjectIdentifier { get { return ((ObjectIdentifierWrapper)this).Item; } }

			public static ObjectSpecifierType NewObjectIdentifier(ObjectId objectIdentifier)
			{
				return new ObjectIdentifierWrapper(objectIdentifier);
			}

			public static readonly ISchema Schema = new ChoiceSchema(false, 
				new FieldSchema("ObjectType", 0, Value<ObjectType>.Schema),
				new FieldSchema("ObjectIdentifier", 1, Value<ObjectId>.Schema));

			public static ObjectSpecifierType Load(IValueStream stream)
			{
				ObjectSpecifierType ret = null;
				Tags tag = (Tags)stream.EnterChoice();
				switch(tag)
				{
					case Tags.ObjectType:
						ret = Value<ObjectTypeWrapper>.Load(stream);
						break;
					case Tags.ObjectIdentifier:
						ret = Value<ObjectIdentifierWrapper>.Load(stream);
						break;
					default:
						throw new Exception();
				}
				stream.LeaveChoice();
				return ret;
			}

			public static void Save(IValueSink sink, ObjectSpecifierType value)
			{
				sink.EnterChoice((byte)value.Tag);
				switch(value.Tag)
				{
					case Tags.ObjectType:
						Value<ObjectTypeWrapper>.Save(sink, (ObjectTypeWrapper)value);
						break;
					case Tags.ObjectIdentifier:
						Value<ObjectIdentifierWrapper>.Save(sink, (ObjectIdentifierWrapper)value);
						break;
					default:
						throw new Exception();
				}
				sink.LeaveChoice();
			}
		}

		public  partial class ObjectTypeWrapper : ObjectSpecifierType
		{
			public override Tags Tag { get { return Tags.ObjectType; } }

			public ObjectType Item { get; private set; }

			public ObjectTypeWrapper(ObjectType item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ObjectType>.Schema;

			public static new ObjectTypeWrapper Load(IValueStream stream)
			{
				var temp = Value<ObjectType>.Load(stream);
				return new ObjectTypeWrapper(temp);
			}

			public static void Save(IValueSink sink, ObjectTypeWrapper value)
			{
				Value<ObjectType>.Save(sink, value.Item);
			}

		}

		public  partial class ObjectIdentifierWrapper : ObjectSpecifierType
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
	}
}
