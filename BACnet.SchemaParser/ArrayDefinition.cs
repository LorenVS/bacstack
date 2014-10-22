using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.SchemaParser
{
    public class ArrayDefinition : TypeDefinition
    {
        /// <summary>
        /// The definition type for array definitions
        /// </summary>
        public override DefinitionType Type
        {
            get { return DefinitionType.Array; }
        }

        /// <summary>
        /// The optional type
        /// </summary>
        public TypeDefinition ElementType { get; private set; }

        /// <summary>
        /// Constructs a new array definition instance
        /// </summary>
        /// <param name="elementType">The array type</param>
        public ArrayDefinition(TypeDefinition elementType)
        {
            Contract.Requires(elementType != null);
            this.ElementType = elementType;
        }
    }
}
