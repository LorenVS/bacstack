using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core
{
    public interface IProcess : IDisposable
    {
        /// <summary>
        /// Retrieves the process id of the process
        /// </summary>
        int ProcessId { get; }

        /// <summary>
        /// Resolves any dependencies of this process
        /// with a list of processes
        /// </summary>
        /// <param name="processes">The processes</param>
        void Resolve(IEnumerable<IProcess> processes);
    }
}
