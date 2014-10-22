using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public class Signed16Value : IValue
    {
        /// <summary>
        /// The value type for signed16 values
        /// </summary>
        public ValueType Type { get { return ValueType.Signed16; } }

        /// <summary>
        /// The wrapped signed16 value
        /// </summary>
        public short Value { get; private set; }

        /// <summary>
        /// Constructs a new Signed16Value instance
        /// </summary>
        /// <param name="value">The signed16 value to wrap</param>
        public Signed16Value(short value)
        {
            this.Value = value;
        }
    }
}
