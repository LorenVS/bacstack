using System;
using System.Diagnostics.Contracts;
using System.Linq;
using BACnet.Core.Datalink;

namespace BACnet.Core
{
    public class Session : IDisposable
    {
        /// <summary>
        /// The processes in the session
        /// </summary>
        private IProcess[] _processes;

        /// <summary>
        /// Constructs a new session
        /// </summary>
        /// <param name="processes">The processes in the session</param>
        public Session(params IProcess[] processes)
        {
            Contract.Requires(processes != null);
            _processes = new IProcess[processes.Length];
            Array.Copy(processes, _processes, processes.Length);

            // open all of the ports first
            foreach (var p in _processes.OfType<IPort>())
            {
                p.Open();
            }

            // then resolve all of the process dependencies
            foreach (var process in _processes)
            {
                process.Resolve(processes);
            }
        }

        /// <summary>
        /// Disposes of the session
        /// </summary>
        public void Dispose()
        {
            foreach(var process in _processes)
            {
                process.Dispose();
            }
        }
    }
}
