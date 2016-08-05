using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Core;
using BACnet.Core.Datalink;

namespace BACnet.Explorer.Core
{
    public class BACnetSession : IDisposable
    {
        /// <summary>
        /// The current bacnet session instance
        /// </summary>
        public static BACnetSession Current { get; set; }

        /// <summary>
        /// The lock used to synchronize access to the session
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// The list of processes in the session
        /// </summary>
        private readonly List<IProcess> _processes = new List<IProcess>();

        private readonly Session _session;

        /// <summary>
        /// Constructs a new bacnet session instance
        /// </summary>
        /// <param name="options">The options for the various processes</param>
        public BACnetSession(IEnumerable<IProcessOptions> options)
        {
            try
            {
                foreach(var opt in options)
                {
                    _processes.Add(opt.Create());
                }

                _session = new Session(_processes.ToArray());
            }
            catch(Exception)
            {
                foreach (var process in _processes)
                    process.Dispose();
                _processes.Clear();
                throw;
            }
        }

        /// <summary>
        /// Disposes of the session
        /// </summary>
        public void Dispose()
        {
            lock(_lock)
            {
                _session.Dispose();
            }
        }

        /// <summary>
        /// Retrieves a process from the session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetProcess<T>() where T : IProcess
        {
            lock(_lock)
            {
                return _processes.OfType<T>().FirstOrDefault();
            }
        }
    }
}
