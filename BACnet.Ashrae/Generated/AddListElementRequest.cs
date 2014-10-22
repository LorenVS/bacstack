using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class AddListElementRequest
	{
		public ObjectId ObjectIdentifier { get; private set; }

		public PropertyIdentifier PropertyIdentifier { get; private set; }

		public Option<uint> PropertyArrayIndex { get; private set; }

		public GenericValue ListOfElements { get; private set; }

		public AddListElementRequest(ObjectId objectIdentifier, PropertyIdentifier propertyIdentifier, Option<uint> propertyArrayIndex, GenericValue listOfElements)
		{
			this.ObjectIdentifier = objectIdentifier;
			this.PropertyIdentifier = propertyIdentifier;
			this.PropertyArrayIndex = propertyArrayIndex;
			this.ListOfElements = listOfElements;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ObjectIdentifier", 0, Value<ObjectId>.Schema),
			new FieldSchema("PropertyIdentifier", 1, Value<PropertyIdentifier>.Schema),
			new FieldSchema("PropertyArrayIndex", 2, Value<Option<uint>>.Schema),
			new FieldSchema("ListOfElements", 3, Value<GenericValue>.Schema));

		public static AddListElementRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var objectIdentifier = Value<ObjectId>.Load(stream);
			var propertyIdentifier = Value<PropertyIdentifier>.Load(stream);
			var propertyArrayIndex = Value<Option<uint>>.Load(stream);
			var listOfElements = Value<GenericValue>.Load(stream);
			stream.LeaveSequence();
			return new AddListElementRequest(objectIdentifier, propertyIdentifier, propertyArrayIndex, listOfElements);
		}

		public static void Save(IValueSink sink, AddListElementRequest value)
		{
			sink.EnterSequence();
			Value<ObjectId>.Save(sink, value.ObjectIdentifier);
			Value<PropertyIdentifier>.Save(sink, value.PropertyIdentifier);
			Value<Option<uint>>.Save(sink, value.PropertyArrayIndex);
			Value<GenericValue>.Save(sink, value.ListOfElements);
			sink.LeaveSequence();
		}
	}
}
