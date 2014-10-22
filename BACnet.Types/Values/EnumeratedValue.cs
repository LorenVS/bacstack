using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public class EnumeratedValue : IValue
    {
        /// <summary>
        /// The value type for enumerated values
        /// </summary>
        public ValueType Type { get { return ValueType.Enumerated; } }

        /// <summary>
        /// The wrapped enumerated value
        /// </summary>
        public uint Value { get; private set; }

        /// <summary>
        /// Constructs a new EnumeratedValue instance
        /// </summary>
        /// <param name="value">The enumerated value to wrap</param>
        public EnumeratedValue(uint value)
        {
            this.Value = value;
        }
    }
}
