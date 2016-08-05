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
        /// The session which this process belongs to
        /// </summary>
        Session Session { get; set; }

        /// <summary>
        /// The message types that this process can handle
        /// </summary>
        IEnumerable<Type> MessageTypes { get; }

        /// <summary>
        /// Handles a message within the current session
        /// </summary>
        /// <param name="message">The message to handle</param>
        /// <returns>True if the message was handled, false otherwise</returns>
        bool HandleMessage(IMessage message);

        /// <summary>
        /// Resolves any dependencies of this process
        /// with a list of processes
        /// </summary>
        /// <param name="processes">The processes</param>
        void Resolve(IEnumerable<IProcess> processes);
    }
}
