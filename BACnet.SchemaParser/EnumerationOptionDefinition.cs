using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.SchemaParser
{
    public class EnumerationOptionDefinition
    {
        /// <summary>
        /// The name of the option
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The value of the option
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// Constructs a new enumeration option definition
        /// </summary>
        /// <param name="name">The name of the option</param>
        /// <param name="value">The value of the option</param>
        public EnumerationOptionDefinition(string name, int value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
