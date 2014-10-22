using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types.Values
{
    public class ChoiceValue : IValue
    {
        /// <summary>
        /// The value type for choice values
        /// </summary>
        public ValueType Type { get { return ValueType.Choice; } }

        /// <summary>
        /// The active choice
        /// </summary>
        public int Choice { get; private set; }

        /// <summary>
        /// The active value
        /// </summary>
        public IValue Value { get; private set; }

        /// <summary>
        /// Constructs a new ChoiceValue instance
        /// </summary>
        /// <param name="choice">The active choice</param>
        /// <param name="value">The active value</param>
        public ChoiceValue(int choice, IValue value)
        {
            this.Choice = choice;
            this.Value = value;
        }
    }
}
