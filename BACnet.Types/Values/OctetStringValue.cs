using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public class OctetStringValue : IValue
    {
        /// <summary>
        /// The value type for octet string values
        /// </summary>
        public ValueType Type { get { return ValueType.OctetString; } }

        /// <summary>
        /// The wrapped octet string value
        /// </summary>
        public ReadOnlyArray<byte> Value { get; private set; }

        /// <summary>
        /// Constructs a new OctetStringValue instance
        /// </summary>
        /// <param name="value">The wrapped octet string value</param>
        /// <param name="clone">True if the value must be cloned, false otherwise</param>
        public OctetStringValue(byte[] value, bool clone = true)
        {
            this.Value = new ReadOnlyArray<byte>(value, clone);
        }

        /// <summary>
        /// Constructs a new OctetStringValue instance
        /// </summary>
        /// <param name="value">The wrapped octet string value</param>
        public OctetStringValue(ReadOnlyArray<byte> value)
        {
            this.Value = value;
        }
    }
}
