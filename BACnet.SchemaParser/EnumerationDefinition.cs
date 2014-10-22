using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.SchemaParser
{
    public class EnumerationDefinition : TypeDefinition
    {
        /// <summary>
        /// The definition type for enumeration definitions
        /// </summary>
        public override DefinitionType Type
        {
            get { return DefinitionType.Enumeration; }
        }

        /// <summary>
        /// The options for the enumeration
        /// </summary>
        public EnumerationOptionDefinition[] Options { get; private set; }

        /// <summary>
        /// Constructs a new enumeration definition instance
        /// </summary>
        /// <param name="options">The options for the enumeration</param>
        public EnumerationDefinition(EnumerationOptionDefinition[] options)
        {
            this.Options = options;
        }
    }
}
