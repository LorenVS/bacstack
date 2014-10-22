using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.SchemaParser
{
    public class SequenceDefinition : TypeDefinition
    {
        /// <summary>
        /// The definition type for sequence definitions
        /// </summary>
        public override DefinitionType Type
        {
            get { return DefinitionType.Sequence; }
        }

        /// <summary>
        /// The fields of the sequence
        /// </summary>
        public FieldDefinition[] Fields { get; private set; }

        /// <summary>
        /// Constructs a new sequence definition
        /// </summary>
        /// <param name="fields">The fields of the sequence</param>
        public SequenceDefinition(FieldDefinition[] fields)
        {
            this.Fields = fields;
        }
    }
}
