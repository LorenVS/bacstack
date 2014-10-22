using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public interface ISchema
    {
        /// <summary>
        /// The type of value that the schema describes
        /// </summary>
        ValueType Type { get; }
    }
}
