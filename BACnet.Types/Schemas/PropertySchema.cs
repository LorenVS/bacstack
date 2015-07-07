using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types.Schemas
{
    public class PropertySchema
    {
        /// <summary>
        /// The property identifier
        /// </summary>
        public ushort PropertyId { get; private set; }

        /// <summary>
        /// The name of the property
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The type of the property
        /// </summary>
        public ISchema Type { get; private set; }

        /// <summary>
        /// Constructs a new PropertySchema instance
        /// </summary>
        /// <param name="propertyId">The property identifier</param>
        /// <param name="name">The property name</param>
        /// <param name="type">The type of the property</param>
        public PropertySchema(ushort propertyId, string name, ISchema type)
        {
            this.PropertyId = propertyId;
            this.Name = name;
            this.Type = type;
        }
    }
}
