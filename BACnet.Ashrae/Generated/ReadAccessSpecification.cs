using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class ReadAccessSpecification
	{
		public ObjectId ObjectIdentifier { get; private set; }

		public ReadOnlyArray<PropertyReference> ListOfPropertyReferences { get; private set; }

		public ReadAccessSpecification(ObjectId objectIdentifier, ReadOnlyArray<PropertyReference> listOfPropertyReferences)
		{
			this.ObjectIdentifier = objectIdentifier;
			this.ListOfPropertyReferences = listOfPropertyReferences;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ObjectIdentifier", 0, Value<ObjectId>.Schema),
			new FieldSchema("ListOfPropertyReferences", 1, Value<ReadOnlyArray<PropertyReference>>.Schema));

		public static ReadAccessSpecification Load(IValueStream stream)
		{
			stream.EnterSequence();
			var objectIdentifier = Value<ObjectId>.Load(stream);
			var listOfPropertyReferences = Value<ReadOnlyArray<PropertyReference>>.Load(stream);
			stream.LeaveSequence();
			return new ReadAccessSpecification(objectIdentifier, listOfPropertyReferences);
		}

		public static void Save(IValueSink sink, ReadAccessSpecification value)
		{
			sink.EnterSequence();
			Value<ObjectId>.Save(sink, value.ObjectIdentifier);
			Value<ReadOnlyArray<PropertyReference>>.Save(sink, value.ListOfPropertyReferences);
			sink.LeaveSequence();
		}
	}
}
