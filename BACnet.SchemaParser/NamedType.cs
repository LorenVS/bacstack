using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.SchemaParser
{
    public class NamedType
    {
        /// <summary>
        /// The name of the type
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The type
        /// </summary>
        public TypeDefinition Definition { get; private set; }

        /// <summary>
        /// Constructs a new named type instance
        /// </summary>
        /// <param name="name">The name of the type</param>
        /// <param name="definition">The type definition</param>
        public NamedType(string name, TypeDefinition definition)
        {
            this.Name = name;
            this.Definition = definition;
        }
    }
}
