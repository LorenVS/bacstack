using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public class BitString56Value : IValue
    {
        /// <summary>
        /// The value type for bitstring56 values
        /// </summary>
        public ValueType Type { get { return ValueType.BitString56; } }

        /// <summary>
        /// The wrapped bitstring56 value
        /// </summary>
        public BitString56 Value { get; private set; }

        /// <summary>
        /// Constructs a new BitString56Value instance
        /// </summary>
        /// <param name="value">The wrapped bitstring56 value</param>
        public BitString56Value(BitString56 value)
        {
            this.Value = value;
        }
    }
}
