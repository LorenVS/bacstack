using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public class Unsigned32Value : IValue
    {
        /// <summary>
        /// The value type for unsigned32 values
        /// </summary>
        public ValueType Type { get { return ValueType.Unsigned32; } }

        /// <summary>
        /// The wrapped unsigned32 value
        /// </summary>
        public uint Value { get; private set; }

        /// <summary>
        /// Constructs a new Unsigned32Value instance
        /// </summary>
        /// <param name="value">The unsigned32 value to wrap</param>
        public Unsigned32Value(uint value)
        {
            this.Value = value;
        }
    }
}
