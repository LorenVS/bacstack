using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class ActionCommand
	{
		public Option<ObjectId> DeviceIdentifier { get; private set; }

		public ObjectId ObjectIdentifier { get; private set; }

		public PropertyIdentifier PropertyIdentifier { get; private set; }

		public Option<uint> PropertyArrayIndex { get; private set; }

		public GenericValue PropertyValue { get; private set; }

		public Option<uint> Priority { get; private set; }

		public Option<uint> PostDelay { get; private set; }

		public bool QuitOnFailure { get; private set; }

		public bool WriteSuccessful { get; private set; }

		public ActionCommand(Option<ObjectId> deviceIdentifier, ObjectId objectIdentifier, PropertyIdentifier propertyIdentifier, Option<uint> propertyArrayIndex, GenericValue propertyValue, Option<uint> priority, Option<uint> postDelay, bool quitOnFailure, bool writeSuccessful)
		{
			this.DeviceIdentifier = deviceIdentifier;
			this.ObjectIdentifier = objectIdentifier;
			this.PropertyIdentifier = propertyIdentifier;
			this.PropertyArrayIndex = propertyArrayIndex;
			this.PropertyValue = propertyValue;
			this.Priority = priority;
			this.PostDelay = postDelay;
			this.QuitOnFailure = quitOnFailure;
			this.WriteSuccessful = writeSuccessful;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("DeviceIdentifier", 0, Value<Option<ObjectId>>.Schema),
			new FieldSchema("ObjectIdentifier", 1, Value<ObjectId>.Schema),
			new FieldSchema("PropertyIdentifier", 2, Value<PropertyIdentifier>.Schema),
			new FieldSchema("PropertyArrayIndex", 3, Value<Option<uint>>.Schema),
			new FieldSchema("PropertyValue", 4, Value<GenericValue>.Schema),
			new FieldSchema("Priority", 5, Value<Option<uint>>.Schema),
			new FieldSchema("PostDelay", 6, Value<Option<uint>>.Schema),
			new FieldSchema("QuitOnFailure", 7, Value<bool>.Schema),
			new FieldSchema("WriteSuccessful", 8, Value<bool>.Schema));

		public static ActionCommand Load(IValueStream stream)
		{
			stream.EnterSequence();
			var deviceIdentifier = Value<Option<ObjectId>>.Load(stream);
			var objectIdentifier = Value<ObjectId>.Load(stream);
			var propertyIdentifier = Value<PropertyIdentifier>.Load(stream);
			var propertyArrayIndex = Value<Option<uint>>.Load(stream);
			var propertyValue = Value<GenericValue>.Load(stream);
			var priority = Value<Option<uint>>.Load(stream);
			var postDelay = Value<Option<uint>>.Load(stream);
			var quitOnFailure = Value<bool>.Load(stream);
			var writeSuccessful = Value<bool>.Load(stream);
			stream.LeaveSequence();
			return new ActionCommand(deviceIdentifier, objectIdentifier, propertyIdentifier, propertyArrayIndex, propertyValue, priority, postDelay, quitOnFailure, writeSuccessful);
		}

		public static void Save(IValueSink sink, ActionCommand value)
		{
			sink.EnterSequence();
			Value<Option<ObjectId>>.Save(sink, value.DeviceIdentifier);
			Value<ObjectId>.Save(sink, value.ObjectIdentifier);
			Value<PropertyIdentifier>.Save(sink, value.PropertyIdentifier);
			Value<Option<uint>>.Save(sink, value.PropertyArrayIndex);
			Value<GenericValue>.Save(sink, value.PropertyValue);
			Value<Option<uint>>.Save(sink, value.Priority);
			Value<Option<uint>>.Save(sink, value.PostDelay);
			Value<bool>.Save(sink, value.QuitOnFailure);
			Value<bool>.Save(sink, value.WriteSuccessful);
			sink.LeaveSequence();
		}
	}
}
