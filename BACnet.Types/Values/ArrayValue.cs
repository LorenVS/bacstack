using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types.Values
{
    public class ArrayValue : IValue
    {
        /// <summary>
        /// Shared global instance for an empty array
        /// </summary>
        public static readonly ArrayValue Empty = new ArrayValue(false);

        /// <summary>
        /// The value type for array values
        /// </summary>
        public ValueType Type { get { return ValueType.Array; } }

        /// <summary>
        /// The values of the array
        /// </summary>
        public ReadOnlyArray<IValue> Values { get; private set; }

        /// <summary>
        /// Constructs a new ArrayValue instance
        /// </summary>
        /// <param name="values">The values of the array</param>
        public ArrayValue(ReadOnlyArray<IValue> values)
        {
            this.Values = values;
        }

        /// <summary>
        /// Constructs a new ArrayValue instance
        /// </summary>
        /// <param name="clone">True if the values need to be cloned, false otherwise</param>
        /// <param name="values">The values of the array</param>
        public ArrayValue(bool clone, params IValue[] values)
        {
            this.Values = new ReadOnlyArray<IValue>(values, clone);
        }
    }
}
