using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class DeleteObjectRequest
	{
		public ObjectId ObjectIdentifier { get; private set; }

		public DeleteObjectRequest(ObjectId objectIdentifier)
		{
			this.ObjectIdentifier = objectIdentifier;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ObjectIdentifier", 255, Value<ObjectId>.Schema));

		public static DeleteObjectRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var objectIdentifier = Value<ObjectId>.Load(stream);
			stream.LeaveSequence();
			return new DeleteObjectRequest(objectIdentifier);
		}

		public static void Save(IValueSink sink, DeleteObjectRequest value)
		{
			sink.EnterSequence();
			Value<ObjectId>.Save(sink, value.ObjectIdentifier);
			sink.LeaveSequence();
		}
	}
}
