using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types.Schemas
{
    /// <summary>
    /// Field schema is not an ISchema instance, since it describes
    /// more than just a type, its type property is howerver
    /// </summary>
    public class FieldSchema
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
        public ISchema Type { get; private set; }

        /// <summary>
        /// Constructs a new FieldSchema instance
        /// </summary>
        /// <param name="name">The name of the field</param>
        /// <param name="tag">The tag of the field</param>
        /// <param name="type">The type of the field</param>
        public FieldSchema(string name, byte tag, ISchema type)
        {
            this.Name = name;
            this.Tag = tag;
            this.Type = type;
        }
    }
}
