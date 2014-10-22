using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core
{
    public interface IContent
    {
        /// <summary>
        /// Serializes the content to the buffer
        /// </summary>
        /// <param name="buffer">The buffer to serialize to</param>
        /// <param name="offset">The offset to begin serialization</param>
        /// <returns>The new offset</returns>
        int Serialize(byte[] buffer, int offset);
    }
}
