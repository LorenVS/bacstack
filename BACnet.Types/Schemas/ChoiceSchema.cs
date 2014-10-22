using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types.Schemas
{
    public class ChoiceSchema : ISchema
    {
        /// <summary>
        /// The value type for choice schemas
        /// </summary>
        public ValueType Type { get { return ValueType.Choice; } }
        
        /// <summary>
        /// The schema for each field in the choice
        /// </summary>
        public ReadOnlyArray<FieldSchema> Fields { get; private set; }

        /// <summary>
        /// Constructs a new ChoiceSchema instance
        /// </summary>
        /// <param name="fields">The fields in the sequence</param>
        public ChoiceSchema(ReadOnlyArray<FieldSchema> fields)
        {
            this.Fields = fields;
        }

        /// <summary>
        /// Constructs a new ChoiceSchema instance
        /// </summary>
        /// <param name="clone">True if the fields need to be cloned, false otherwise</param>
        /// <param name="fields">The fields in the sequence</param>
        public ChoiceSchema(bool clone, params FieldSchema[] fields)
        {
            this.Fields = new ReadOnlyArray<FieldSchema>(fields, clone);
        }
    }
}
