using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.SchemaParser
{
    public class FieldDefinition
    {
        /// <summary>
        /// The name of the field
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The tag of the field
        /// </summary>
        public byte Tag { get; private set; }

        /// <summary>
        /// The type of the field
        /// </summary>
        public TypeDefinition Type { get; private set; }

        /// <summary>
        /// Constructs a new field definition instance
        /// </summary>
        /// <param name="name">The name of the field</param>
        /// <param name="tag">The tag of the field</param>
        /// <param name="type">The type of the field</param>
        public FieldDefinition(string name, byte tag, TypeDefinition type)
        {
            this.Name = name;
            this.Tag = tag;
            this.Type = type;
        }
    }
}
