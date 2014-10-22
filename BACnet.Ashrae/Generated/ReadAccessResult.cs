using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class ReadAccessResult
	{
		public ObjectId ObjectIdentifier { get; private set; }

		public Option<ReadOnlyArray<ListOfResultsType>> ListOfResults { get; private set; }

		public ReadAccessResult(ObjectId objectIdentifier, Option<ReadOnlyArray<ListOfResultsType>> listOfResults)
		{
			this.ObjectIdentifier = objectIdentifier;
			this.ListOfResults = listOfResults;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ObjectIdentifier", 0, Value<ObjectId>.Schema),
			new FieldSchema("ListOfResults", 1, Value<Option<ReadOnlyArray<ListOfResultsType>>>.Schema));

		public static ReadAccessResult Load(IValueStream stream)
		{
			stream.EnterSequence();
			var objectIdentifier = Value<ObjectId>.Load(stream);
			var listOfResults = Value<Option<ReadOnlyArray<ListOfResultsType>>>.Load(stream);
			stream.LeaveSequence();
			return new ReadAccessResult(objectIdentifier, listOfResults);
		}

		public static void Save(IValueSink sink, ReadAccessResult value)
		{
			sink.EnterSequence();
			Value<ObjectId>.Save(sink, value.ObjectIdentifier);
			Value<Option<ReadOnlyArray<ListOfResultsType>>>.Save(sink, value.ListOfResults);
			sink.LeaveSequence();
		}
		public  partial class ListOfResultsType
		{
			public PropertyIdentifier PropertyIdentifier { get; private set; }

			public Option<uint> PropertyArrayIndex { get; private set; }

			public ReadResultType ReadResult { get; private set; }

			public ListOfResultsType(PropertyIdentifier propertyIdentifier, Option<uint> propertyArrayIndex, ReadResultType readResult)
			{
				this.PropertyIdentifier = propertyIdentifier;
				this.PropertyArrayIndex = propertyArrayIndex;
				this.ReadResult = readResult;
			}

			public static readonly ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("PropertyIdentifier", 2, Value<PropertyIdentifier>.Schema),
				new FieldSchema("PropertyArrayIndex", 3, Value<Option<uint>>.Schema),
				new FieldSchema("ReadResult", 255, Value<ReadResultType>.Schema));

			public static ListOfResultsType Load(IValueStream stream)
			{
				stream.EnterSequence();
				var propertyIdentifier = Value<PropertyIdentifier>.Load(stream);
				var propertyArrayIndex = Value<Option<uint>>.Load(stream);
				var readResult = Value<ReadResultType>.Load(stream);
				stream.LeaveSequence();
				return new ListOfResultsType(propertyIdentifier, propertyArrayIndex, readResult);
			}

			public static void Save(IValueSink sink, ListOfResultsType value)
			{
				sink.EnterSequence();
				Value<PropertyIdentifier>.Save(sink, value.PropertyIdentifier);
				Value<Option<uint>>.Save(sink, value.PropertyArrayIndex);
				Value<ReadResultType>.Save(sink, value.ReadResult);
				sink.LeaveSequence();
			}
		}

		public enum Tags : byte
		{
			PropertyValue = 0,
			PropertyAccessError = 1
		}

		public abstract  partial class ReadResultType
		{
			public abstract Tags Tag { get; }

			public bool IsPropertyValue { get { return this.Tag == Tags.PropertyValue; } }

			public GenericValue AsPropertyValue { get { return ((PropertyValueWrapper)this).Item; } }

			public static ReadResultType NewPropertyValue(GenericValue propertyValue)
			{
				return new PropertyValueWrapper(propertyValue);
			}

			public bool IsPropertyAccessError { get { return this.Tag == Tags.PropertyAccessError; } }

			public Error AsPropertyAccessError { get { return ((PropertyAccessErrorWrapper)this).Item; } }

			public static ReadResultType NewPropertyAccessError(Error propertyAccessError)
			{
				return new PropertyAccessErrorWrapper(propertyAccessError);
			}

			public static readonly ISchema Schema = new ChoiceSchema(false, 
				new FieldSchema("PropertyValue", 4, Value<GenericValue>.Schema),
				new FieldSchema("PropertyAccessError", 5, Value<Error>.Schema));

			public static ReadResultType Load(IValueStream stream)
			{
				ReadResultType ret = null;
				Tags tag = (Tags)stream.EnterChoice();
				switch(tag)
				{
					case Tags.PropertyValue:
						ret = Value<PropertyValueWrapper>.Load(stream);
						break;
					case Tags.PropertyAccessError:
						ret = Value<PropertyAccessErrorWrapper>.Load(stream);
						break;
					default:
						throw new Exception();
				}
				stream.LeaveChoice();
				return ret;
			}

			public static void Save(IValueSink sink, ReadResultType value)
			{
				sink.EnterChoice((byte)value.Tag);
				switch(value.Tag)
				{
					case Tags.PropertyValue:
						Value<PropertyValueWrapper>.Save(sink, (PropertyValueWrapper)value);
						break;
					case Tags.PropertyAccessError:
						Value<PropertyAccessErrorWrapper>.Save(sink, (PropertyAccessErrorWrapper)value);
						break;
					default:
						throw new Exception();
				}
				sink.LeaveChoice();
			}
		}

		public  partial class PropertyValueWrapper : ReadResultType
		{
			public override Tags Tag { get { return Tags.PropertyValue; } }

			public GenericValue Item { get; private set; }

			public PropertyValueWrapper(GenericValue item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<GenericValue>.Schema;

			public static new PropertyValueWrapper Load(IValueStream stream)
			{
				var temp = Value<GenericValue>.Load(stream);
				return new PropertyValueWrapper(temp);
			}

			public static void Save(IValueSink sink, PropertyValueWrapper value)
			{
				Value<GenericValue>.Save(sink, value.Item);
			}

		}

		public  partial class PropertyAccessErrorWrapper : ReadResultType
		{
			public override Tags Tag { get { return Tags.PropertyAccessError; } }

			public Error Item { get; private set; }

			public PropertyAccessErrorWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new PropertyAccessErrorWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new PropertyAccessErrorWrapper(temp);
			}

			public static void Save(IValueSink sink, PropertyAccessErrorWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}
	}
}
