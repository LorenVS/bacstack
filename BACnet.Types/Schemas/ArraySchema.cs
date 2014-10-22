using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types.Schemas
{
    public class ArraySchema : ISchema
    {
        /// <summary>
        /// The value type for array schemas
        /// </summary>
        public ValueType Type { get { return ValueType.Array; } }
        
        /// <summary>
        /// The schema for the array's element type
        /// </summary>
        public ISchema ElementType { get; private set; }

        /// <summary>
        /// Constructs a new ArraySchema instance
        /// </summary>
        /// <param name="elementType">The schema for the array's element type</param>
        public ArraySchema(ISchema elementType)
        {
            this.ElementType = elementType;
        }
    }
}
