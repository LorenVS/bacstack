using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types.Values
{
    public class DateValue : IValue
    {
        /// <summary>
        /// The value type for date values
        /// </summary>
        public ValueType Type { get { return ValueType.Date; } }

        /// <summary>
        /// The wrapped date value
        /// </summary>
        public Date Value { get; private set; }

        /// <summary>
        /// Constructs a new DateValue instance
        /// </summary>
        /// <param name="value">The wrapped date value</param>
        public DateValue(Date value)
        {
            this.Value = value;
        }
    }
}
