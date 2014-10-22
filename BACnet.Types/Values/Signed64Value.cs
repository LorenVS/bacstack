using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public class Signed64Value : IValue
    {
        /// <summary>
        /// The value type for signed64 values
        /// </summary>
        public ValueType Type { get { return ValueType.Signed64; } }

        /// <summary>
        /// The wrapped signed64 value
        /// </summary>
        public long Value { get; private set; }

        /// <summary>
        /// Constructs a new Signed64Value instance
        /// </summary>
        /// <param name="value">The signed64 value to wrap</param>
        public Signed64Value(long value)
        {
            this.Value = value;
        }
    }
}
