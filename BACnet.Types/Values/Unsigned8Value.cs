using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public class Unsigned8Value : IValue
    {
        /// <summary>
        /// The value type for unsigned8 values
        /// </summary>
        public ValueType Type { get { return ValueType.Unsigned8; } }

        /// <summary>
        /// The wrapped unsigned8 value
        /// </summary>
        public byte Value { get; private set; }

        /// <summary>
        /// Constructs a new Unsigned8Value instance
        /// </summary>
        /// <param name="value">The wrapped unsigned8 value</param>
        public Unsigned8Value(byte value)
        {
            this.Value = value;
        }
    }
}
