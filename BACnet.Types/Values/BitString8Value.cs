using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public class BitString8Value : IValue
    {
        /// <summary>
        /// The value type for bitstring8 values
        /// </summary>
        public ValueType Type { get { return ValueType.BitString8; } }

        /// <summary>
        /// The wrapped bitstring8 value
        /// </summary>
        public BitString8 Value { get; private set; }

        /// <summary>
        /// Constructs a new BitString8Value instance
        /// </summary>
        /// <param name="value">The wrapped bitstring8 value</param>
        public BitString8Value(BitString8 value)
        {
            this.Value = value;
        }
    }
}
