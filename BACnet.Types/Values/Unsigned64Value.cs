using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public class Unsigned64Value : IValue
    {
        /// <summary>
        /// The value type for unsigned64 values
        /// </summary>
        public ValueType Type { get { return ValueType.Unsigned64; } }

        /// <summary>
        /// The wrapped unsigned64 value
        /// </summary>
        public ulong Value { get; private set; }

        /// <summary>
        /// Constructs a new Unsigned64Value instance
        /// </summary>
        /// <param name="value">The unsigned64 value to wrap</param>
        public Unsigned64Value(ulong value)
        {
            this.Value = value;
        }
    }
}
