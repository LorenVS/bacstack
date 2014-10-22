using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types.Values
{
    public class SequenceValue : IValue
    {
        /// <summary>
        /// The value type for sequence values
        /// </summary>
        public ValueType Type { get { return ValueType.Sequence; } }

        /// <summary>
        /// The sequence of values
        /// </summary>
        public ReadOnlyArray<IValue> Values { get; private set; }

        /// <summary>
        /// Constructs a new SequenceValue instance
        /// </summary>
        /// <param name="values">The values of the sequence</param>
        public SequenceValue(ReadOnlyArray<IValue> values)
        {
            this.Values = values;
        }

        /// <summary>
        /// Constructs a new sequence value
        /// </summary>
        /// <param name="clone">True if the provided values need to be cloned, false otherwise</param>
        /// <param name="values">The values of the sequence</param>
        public SequenceValue(bool clone, params IValue[] values)
        {
            this.Values = new ReadOnlyArray<IValue>(values, clone);
        }
    }
}
