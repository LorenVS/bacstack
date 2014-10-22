using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.SchemaParser
{
    public abstract class TypeDefinition
    {
        
        /// <summary>
        /// The definition type
        /// </summary>
        public abstract DefinitionType Type { get; }
    }
}
