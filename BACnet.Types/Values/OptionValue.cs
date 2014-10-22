using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types.Values
{
    public class OptionValue : IValue
    {
        /// <summary>
        /// Shared global instance for no option
        /// </summary>
        public static readonly OptionValue None = new OptionValue(null);

        /// <summary>
        /// The value type for option values
        /// </summary>
        public ValueType Type { get { return ValueType.Option; } }

        /// <summary>
        /// The option's value, or null for no value
        /// </summary>
        public IValue Value { get; private set; }

        /// <summary>
        /// Constructs a new OptionValue
        /// </summary>
        /// <param name="value">The wrapped value, or null for no value</param>
        public OptionValue(IValue value)
        {
            this.Value = value;
        }
    }
}
