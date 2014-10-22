using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types.Values
{
    /// <summary>
    /// Lists are the one mutable value
    /// </summary>
    public class ListValue : IValue
    {
        /// <summary>
        /// The value type for list values
        /// </summary>
        public ValueType Type { get { return ValueType.List; } }

        /// <summary>
        /// The values of the list
        /// </summary>
        public List<IValue> Values { get; private set; }

        /// <summary>
        /// Constructs a new ListValue instance
        /// </summary>
        public ListValue(IValue value)
        {
            this.Values = new List<IValue>();
        }
    }
}
