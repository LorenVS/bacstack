using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public class Signed8Value : IValue
    {
        /// <summary>
        /// The value type for signed8 values
        /// </summary>
        public ValueType Type { get { return ValueType.Signed8; } }

        /// <summary>
        /// The wrapped signed8 value
        /// </summary>
        public sbyte Value { get; private set; }

        /// <summary>
        /// Constructs a new Signed8Value instance
        /// </summary>
        /// <param name="value">The signed8 value to wrap</param>
        public Signed8Value(sbyte value)
        {
            this.Value = value;
        }
    }
}
