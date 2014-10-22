using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public struct Enumerated
    {
        /// <summary>
        /// The enumerated value
        /// </summary>
        public uint Value { get; private set; }

        /// <summary>
        /// Constructs a new enumerated value
        /// </summary>
        /// <param name="value">The enumerated value</param>
        public Enumerated(uint value) : this()
        {
            Value = value;
        }

        public static implicit operator Enumerated(uint value)
        {
            return new Enumerated(value);
        }

    }
}
