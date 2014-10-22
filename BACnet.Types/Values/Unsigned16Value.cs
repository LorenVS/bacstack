using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public class Unsigned16Value : IValue
    {
        /// <summary>
        /// The value type for unsigned16 values
        /// </summary>
        public ValueType Type { get { return ValueType.Unsigned16; } }

        /// <summary>
        /// The wrapped unsigned16 value
        /// </summary>
        public ushort Value { get; private set; }

        /// <summary>
        /// Constructs a new Unsigned16Value instance
        /// </summary>
        /// <param name="value">The unsigned16 value to wrap</param>
        public Unsigned16Value(ushort value)
        {
            this.Value = value;
        }
    }
}
