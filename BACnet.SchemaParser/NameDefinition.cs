using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.SchemaParser
{
    public class NameDefinition : TypeDefinition
    {
        /// <summary>
        /// The definition type for name definitions
        /// </summary>
        public override DefinitionType Type { get { return DefinitionType.Name; } }

        /// <summary>
        /// The name of the referenced type
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Constructs a new name definition instance
        /// </summary>
        /// <param name="name">The name of the referenced type</param>
        public NameDefinition(string name)
        {
            this.Name = name;
        }
    }
}
