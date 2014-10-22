using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class WriteAccessSpecification
	{
		public ObjectId ObjectIdentifier { get; private set; }

		public ReadOnlyArray<PropertyValue> ListOfProperties { get; private set; }

		public WriteAccessSpecification(ObjectId objectIdentifier, ReadOnlyArray<PropertyValue> listOfProperties)
		{
			this.ObjectIdentifier = objectIdentifier;
			this.ListOfProperties = listOfProperties;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ObjectIdentifier", 0, Value<ObjectId>.Schema),
			new FieldSchema("ListOfProperties", 1, Value<ReadOnlyArray<PropertyValue>>.Schema));

		public static WriteAccessSpecification Load(IValueStream stream)
		{
			stream.EnterSequence();
			var objectIdentifier = Value<ObjectId>.Load(stream);
			var listOfProperties = Value<ReadOnlyArray<PropertyValue>>.Load(stream);
			stream.LeaveSequence();
			return new WriteAccessSpecification(objectIdentifier, listOfProperties);
		}

		public static void Save(IValueSink sink, WriteAccessSpecification value)
		{
			sink.EnterSequence();
			Value<ObjectId>.Save(sink, value.ObjectIdentifier);
			Value<ReadOnlyArray<PropertyValue>>.Save(sink, value.ListOfProperties);
			sink.LeaveSequence();
		}
	}
}
