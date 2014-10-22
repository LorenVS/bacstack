using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public class PropertyAttribute : Attribute
    {
        /// <summary>
        /// The property identifier for the property
        /// </summary>
        public ushort PropertyIdentifier { get; private set; }

        /// <summary>
        /// Constructs a new property attribute instance
        /// </summary>
        /// <param name="propertyIdentifier">The property identifier for the property</param>
        public PropertyAttribute(ushort propertyIdentifier)
        {
            this.PropertyIdentifier = propertyIdentifier;
        }
    }
}
