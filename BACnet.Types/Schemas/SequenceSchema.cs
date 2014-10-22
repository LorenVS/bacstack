using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types.Schemas
{
    public class SequenceSchema : ISchema
    {
        /// <summary>
        /// The value type for sequence schemas
        /// </summary>
        public ValueType Type { get { return ValueType.Sequence; } }

        /// <summary>
        /// The schema for each field in the sequence
        /// </summary>
        public ReadOnlyArray<FieldSchema> Fields { get; private set; }

        /// <summary>
        /// Constructs a new SequenceSchema instance
        /// </summary>
        /// <param name="fields">The fields in the sequence</param>
        public SequenceSchema(ReadOnlyArray<FieldSchema> fields)
        {
            this.Fields = fields;
        }

        /// <summary>
        /// Constructs a new SequenceSchema instance
        /// </summary>
        /// <param name="clone">True if the fields need to be cloned, false otherwise</param>
        /// <param name="fields">The fields in the sequence</param>
        public SequenceSchema(bool clone, params FieldSchema[] fields)
        {
            this.Fields = new ReadOnlyArray<FieldSchema>(fields, clone);
        }
    }
}
