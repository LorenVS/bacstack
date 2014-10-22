using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public class Signed32Value : IValue
    {
        /// <summary>
        /// The value type for signed32 values
        /// </summary>
        public ValueType Type { get { return ValueType.Signed32; } }

        /// <summary>
        /// The wrapped signed32 value
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Constructs a new Signed32Value instance
        /// </summary>
        /// <param name="value">The signed32 value to wrap</param>
        public Signed32Value(int value)
        {
            this.Value = value;
        }
    }
}
