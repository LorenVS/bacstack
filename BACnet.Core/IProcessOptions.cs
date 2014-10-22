using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core
{
    public interface IProcessOptions
    {
        /// <summary>
        /// Creates a process from the options
        /// </summary>
        /// <returns>The created process</returns>
        IProcess Create();
    }
}
