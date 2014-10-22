using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.SchemaParser
{
    public class ChoiceDefinition : TypeDefinition
    {
        /// <summary>
        /// The definition type for choice definitions
        /// </summary>
        public override DefinitionType Type
        {
            get { return DefinitionType.Choice; }
        }

        /// <summary>
        /// The fields of the choice
        /// </summary>
        public FieldDefinition[] Fields { get; private set; }

        /// <summary>
        /// Constructs a new choice definition
        /// </summary>
        /// <param name="fields">The fields of the choice</param>
        public ChoiceDefinition(FieldDefinition[] fields)
        {
            this.Fields = fields;
        }
    }
}
