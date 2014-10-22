using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types.Schemas
{
    public class OptionSchema : ISchema
    {
        /// <summary>
        /// The value type for option schemas
        /// </summary>
        public ValueType Type { get { return ValueType.Option; } }

        /// <summary>
        /// The schema for the element type of the options
        /// </summary>
        public ISchema ElementType { get; private set; }

        /// <summary>
        /// Constructs a new OptionSchema instance
        /// </summary>
        /// <param name="elementType">The element type of the option</param>
        public OptionSchema(ISchema elementType)
        {
            this.ElementType = elementType;
        }
    }
}
