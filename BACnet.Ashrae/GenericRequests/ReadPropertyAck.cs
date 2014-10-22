using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
    public partial class ReadPropertyAck<TData> : IComplexAck
    {
        /// <summary>
        /// The service choice for read property requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.ReadProperty; } }

        public ObjectId ObjectIdentifier { get; private set; }

        public PropertyIdentifier PropertyIdentifier { get; private set; }

        public Option<uint> PropertyArrayIndex { get; private set; }

        public TData PropertyValue { get; private set; }

        public ReadPropertyAck(ObjectId objectIdentifier, PropertyIdentifier propertyIdentifier, Option<uint> propertyArrayIndex, TData propertyValue)
        {
            this.ObjectIdentifier = objectIdentifier;
            this.PropertyIdentifier = propertyIdentifier;
            this.PropertyArrayIndex = propertyArrayIndex;
            this.PropertyValue = propertyValue;
        }

        public static readonly ISchema Schema = new SequenceSchema(false,
            new FieldSchema("ObjectIdentifier", 0, Value<ObjectId>.Schema),
            new FieldSchema("PropertyIdentifier", 1, Value<PropertyIdentifier>.Schema),
            new FieldSchema("PropertyArrayIndex", 2, Value<Option<uint>>.Schema),
            new FieldSchema("PropertyValue", 3, Value<TData>.Schema));

        public static ReadPropertyAck<TData> Load(IValueStream stream)
        {
            stream.EnterSequence();
            var objectIdentifier = Value<ObjectId>.Load(stream);
            var propertyIdentifier = Value<PropertyIdentifier>.Load(stream);
            var propertyArrayIndex = Value<Option<uint>>.Load(stream);
            var propertyValue = Value<TData>.Load(stream);
            stream.LeaveSequence();
            return new ReadPropertyAck<TData>(objectIdentifier, propertyIdentifier, propertyArrayIndex, propertyValue);
        }

        public static void Save(IValueSink sink, ReadPropertyAck<TData> value)
        {
            sink.EnterSequence();
            Value<ObjectId>.Save(sink, value.ObjectIdentifier);
            Value<PropertyIdentifier>.Save(sink, value.PropertyIdentifier);
            Value<Option<uint>>.Save(sink, value.PropertyArrayIndex);
            Value<TData>.Save(sink, value.PropertyValue);
            sink.LeaveSequence();
        }
    }
}
