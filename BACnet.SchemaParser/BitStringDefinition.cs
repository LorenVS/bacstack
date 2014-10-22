using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.SchemaParser
{
    public class BitStringDefinition : TypeDefinition
    {
        /// <summary>
        /// The definition type for bit string definitions
        /// </summary>
        public override DefinitionType Type
        {
            get { return DefinitionType.BitString; }
        }

        /// <summary>
        /// The bits for the bitstring
        /// </summary>
        public BitDefinition[] Bits { get; private set; }

        /// <summary>
        /// Constructs a new bit string definition instance
        /// </summary>
        /// <param name="bits">The bits for the bitstring</param>
        public BitStringDefinition(BitDefinition[] bits)
        {
            this.Bits = bits;
        }
    }
}
