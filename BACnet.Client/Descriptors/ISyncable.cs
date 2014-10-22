using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Client.Descriptors
{
    public interface ISyncable
    {
        /// <summary>
        /// Syncs this object to another
        /// </summary>
        /// <param name="other">The instance to sync from</param>
        void CopyFrom(object other);
    }
}
