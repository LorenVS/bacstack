using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.SchemaParser
{
    public class OptionDefinition : TypeDefinition
    {
        /// <summary>
        /// The definition type for option definitions
        /// </summary>
        public override DefinitionType Type
        {
            get { return DefinitionType.Option; }
        }

        /// <summary>
        /// The optional type
        /// </summary>
        public TypeDefinition ElementType { get; private set; }

        /// <summary>
        /// Constructs a new option definition instance
        /// </summary>
        /// <param name="elementType">The optional type</param>
        public OptionDefinition(TypeDefinition elementType)
        {
            Contract.Requires(elementType != null);
            this.ElementType = elementType;
        }
    }
}
