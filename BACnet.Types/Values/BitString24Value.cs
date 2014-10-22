using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public class BitString24Value : IValue
    {
        /// <summary>
        /// The value type for bitstring8 values
        /// </summary>
        public ValueType Type { get { return ValueType.BitString8; } }

        /// <summary>
        /// The wrapped bitstring24 value
        /// </summary>
        public BitString24 Value { get; private set; }

        /// <summary>
        /// Constructs a new BitString24Value instance
        /// </summary>
        /// <param name="value">The wrapped bitstring24 value</param>
        public BitString24Value(BitString24 value)
        {
            this.Value = value;
        }
    }
}
