using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public class BooleanValue : IValue
    {
        /// <summary>
        /// Shared global instance for truthy boolean values
        /// </summary>
        public static readonly BooleanValue True = new BooleanValue(true);

        /// <summary>
        /// Shared global instance for falsy boolean values
        /// </summary>
        public static readonly BooleanValue False = new BooleanValue(false);

        /// <summary>
        /// The value type for boolean values
        /// </summary>
        public ValueType Type { get { return ValueType.Boolean; } }

        /// <summary>
        /// The boolean value
        /// </summary>
        public bool Value { get; private set; }

        /// <summary>
        /// Constructs a new boolean value instance
        /// </summary>
        /// <param name="value">The wrapped boolean value</param>
        private BooleanValue(bool value)
        {
            this.Value = value;
        }
    }
}
