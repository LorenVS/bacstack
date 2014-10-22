using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types.Values
{
    public class ObjectIdValue : IValue
    {
        /// <summary>
        /// The value type for object id values
        /// </summary>
        public ValueType Type { get { return ValueType.ObjectId; } }

        /// <summary>
        /// The wrapped object id value
        /// </summary>
        public ObjectId Value { get; private set; }

        /// <summary>
        /// Constructs a new ObjectIdValue instance
        /// </summary>
        /// <param name="value">The value being wrapped</param>
        public ObjectIdValue(ObjectId value)
        {
            this.Value = value;
        }
    }
}
