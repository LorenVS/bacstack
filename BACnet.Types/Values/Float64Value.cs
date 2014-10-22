using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public class Float64Value : IValue
    {
        /// <summary>
        /// The value type for float64 values
        /// </summary>
        public ValueType Type { get { return ValueType.Float64; } }

        /// <summary>
        /// The wrapped float64 value
        /// </summary>
        public double Value { get; private set; }

        /// <summary>
        /// Constructs a new Float64Value instance
        /// </summary>
        /// <param name="value">The wrapped float64 instance</param>
        public Float64Value(double value)
        {
            this.Value = value;
        }
    }
}
