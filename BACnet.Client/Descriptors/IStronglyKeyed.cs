using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Client.Descriptors
{
    public interface IStronglyKeyed<TKey>
    {
        /// <summary>
        /// The unique key of the descriptor
        /// </summary>
        TKey Key { get; }
    }
}
