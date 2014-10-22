using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public class Float32Value : IValue
    {
        /// <summary>
        /// The value type for float32 values
        /// </summary>
        public ValueType Type { get { return ValueType.Float32; } }

        /// <summary>
        /// The wrapped float32 value
        /// </summary>
        public float Value { get; private set; }

        /// <summary>
        /// Constructs a new Float32Value instance
        /// </summary>
        /// <param name="value">The wrapped float32 instance</param>
        public Float32Value(float value)
        {
            this.Value = value;
        }
    }
}
