using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class ReadPropertyConditionalRequest
	{
		public ObjectSelectionCriteriaType ObjectSelectionCriteria { get; private set; }

		public Option<ReadOnlyArray<PropertyReference>> ListOfPropertyReferences { get; private set; }

		public ReadPropertyConditionalRequest(ObjectSelectionCriteriaType objectSelectionCriteria, Option<ReadOnlyArray<PropertyReference>> listOfPropertyReferences)
		{
			this.ObjectSelectionCriteria = objectSelectionCriteria;
			this.ListOfPropertyReferences = listOfPropertyReferences;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ObjectSelectionCriteria", 0, Value<ObjectSelectionCriteriaType>.Schema),
			new FieldSchema("ListOfPropertyReferences", 1, Value<Option<ReadOnlyArray<PropertyReference>>>.Schema));

		public static ReadPropertyConditionalRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var objectSelectionCriteria = Value<ObjectSelectionCriteriaType>.Load(stream);
			var listOfPropertyReferences = Value<Option<ReadOnlyArray<PropertyReference>>>.Load(stream);
			stream.LeaveSequence();
			return new ReadPropertyConditionalRequest(objectSelectionCriteria, listOfPropertyReferences);
		}

		public static void Save(IValueSink sink, ReadPropertyConditionalRequest value)
		{
			sink.EnterSequence();
			Value<ObjectSelectionCriteriaType>.Save(sink, value.ObjectSelectionCriteria);
			Value<Option<ReadOnlyArray<PropertyReference>>>.Save(sink, value.ListOfPropertyReferences);
			sink.LeaveSequence();
		}
		public  partial class ObjectSelectionCriteriaType
		{
			public SelectionLogicType SelectionLogic { get; private set; }

			public Option<ReadOnlyArray<ListOfSelectionCriteriaType>> ListOfSelectionCriteria { get; private set; }

			public ObjectSelectionCriteriaType(SelectionLogicType selectionLogic, Option<ReadOnlyArray<ListOfSelectionCriteriaType>> listOfSelectionCriteria)
			{
				this.SelectionLogic = selectionLogic;
				this.ListOfSelectionCriteria = listOfSelectionCriteria;
			}

			public static readonly ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("SelectionLogic", 0, Value<SelectionLogicType>.Schema),
				new FieldSchema("ListOfSelectionCriteria", 1, Value<Option<ReadOnlyArray<ListOfSelectionCriteriaType>>>.Schema));

			public static ObjectSelectionCriteriaType Load(IValueStream stream)
			{
				stream.EnterSequence();
				var selectionLogic = Value<SelectionLogicType>.Load(stream);
				var listOfSelectionCriteria = Value<Option<ReadOnlyArray<ListOfSelectionCriteriaType>>>.Load(stream);
				stream.LeaveSequence();
				return new ObjectSelectionCriteriaType(selectionLogic, listOfSelectionCriteria);
			}

			public static void Save(IValueSink sink, ObjectSelectionCriteriaType value)
			{
				sink.EnterSequence();
				Value<SelectionLogicType>.Save(sink, value.SelectionLogic);
				Value<Option<ReadOnlyArray<ListOfSelectionCriteriaType>>>.Save(sink, value.ListOfSelectionCriteria);
				sink.LeaveSequence();
			}
		}
		public enum SelectionLogicType : uint
		{
			And = 0,
			Or = 1,
			All = 2
		}
		public  partial class ListOfSelectionCriteriaType
		{
			public PropertyIdentifier PropertyIdentifier { get; private set; }

			public Option<uint> PropertyArrayIndex { get; private set; }

			public RelationSpecifierType RelationSpecifier { get; private set; }

			public GenericValue ComparisonValue { get; private set; }

			public ListOfSelectionCriteriaType(PropertyIdentifier propertyIdentifier, Option<uint> propertyArrayIndex, RelationSpecifierType relationSpecifier, GenericValue comparisonValue)
			{
				this.PropertyIdentifier = propertyIdentifier;
				this.PropertyArrayIndex = propertyArrayIndex;
				this.RelationSpecifier = relationSpecifier;
				this.ComparisonValue = comparisonValue;
			}

			public static readonly ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("PropertyIdentifier", 0, Value<PropertyIdentifier>.Schema),
				new FieldSchema("PropertyArrayIndex", 1, Value<Option<uint>>.Schema),
				new FieldSchema("RelationSpecifier", 2, Value<RelationSpecifierType>.Schema),
				new FieldSchema("ComparisonValue", 3, Value<GenericValue>.Schema));

			public static ListOfSelectionCriteriaType Load(IValueStream stream)
			{
				stream.EnterSequence();
				var propertyIdentifier = Value<PropertyIdentifier>.Load(stream);
				var propertyArrayIndex = Value<Option<uint>>.Load(stream);
				var relationSpecifier = Value<RelationSpecifierType>.Load(stream);
				var comparisonValue = Value<GenericValue>.Load(stream);
				stream.LeaveSequence();
				return new ListOfSelectionCriteriaType(propertyIdentifier, propertyArrayIndex, relationSpecifier, comparisonValue);
			}

			public static void Save(IValueSink sink, ListOfSelectionCriteriaType value)
			{
				sink.EnterSequence();
				Value<PropertyIdentifier>.Save(sink, value.PropertyIdentifier);
				Value<Option<uint>>.Save(sink, value.PropertyArrayIndex);
				Value<RelationSpecifierType>.Save(sink, value.RelationSpecifier);
				Value<GenericValue>.Save(sink, value.ComparisonValue);
				sink.LeaveSequence();
			}
		}
		public enum RelationSpecifierType : uint
		{
			Equal = 0,
			NotEqual = 1,
			LessThan = 2,
			GreaterThan = 3,
			LessThanOrEqual = 4,
			GreaterThanOrEqual = 5
		}
	}
}
