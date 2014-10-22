using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.SchemaParser
{
    public class BitDefinition
    {
        /// <summary>
        /// The name of the bit
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The index of the bit
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Constructs a new bit definition instance
        /// </summary>
        /// <param name="name">The name of the bit</param>
        /// <param name="index">The index of the bit</param>
        public BitDefinition(string name, int index)
        {
            this.Name = name;
            this.Index = index;
        }
    }
}
