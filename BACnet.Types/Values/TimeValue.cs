using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types.Values
{
    public class TimeValue : IValue
    {
        /// <summary>
        /// The value type for time values
        /// </summary>
        public ValueType Type { get { return ValueType.Time; } }

        /// <summary>
        /// The wrapped time value
        /// </summary>
        public Time Value { get; private set; }

        /// <summary>
        /// Constructs a new TimeValue instance
        /// </summary>
        /// <param name="value">The wrapped time value</param>
        public TimeValue(Time value)
        {
            this.Value = value;
        }
    }
}
