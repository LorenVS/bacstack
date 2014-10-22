using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public class CharStringValue : IValue
    {
        /// <summary>
        /// The value type for char string values
        /// </summary>
        public ValueType Type { get { return ValueType.CharString; } }

        /// <summary>
        /// The wrapped char string value
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Constructs a new CharStringValue instance
        /// </summary>
        /// <param name="value">The wrapped char string value</param>
        public CharStringValue(string value)
        {
            this.Value = value;
        }
    }
}
