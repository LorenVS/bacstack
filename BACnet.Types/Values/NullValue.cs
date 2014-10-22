using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public class NullValue : IValue
    {
        /// <summary>
        /// The shared global instance of the value
        /// </summary>
        public static readonly NullValue Instance = new NullValue();

        /// <summary>
        /// The value type for null values
        /// </summary>
        public ValueType Type { get { return ValueType.Null; } }
    }
}
