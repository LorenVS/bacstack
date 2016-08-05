using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Datalink
{
    public class PortManager : IProcess
    {
        /// <summary>
        /// Retrieves the process id of the process
        /// </summary>
        public int ProcessId { get { return _options.ProcessId; } }

        /// <summary>
        /// The session which this process belongs to
        /// </summary>
        public Session Session { get; set; }

        /// <summary>
        /// Lock used to synchronize access to the port manager
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// The options for the port manager
        /// </summary>
        private readonly PortManagerOptions _options;

        /// <summary>
        /// The list of ports registered with the port manager
        /// </summary>
        private List<IPort> _ports;
        
        /// <summary>
        /// Constructs a new port manager instance
        /// </summary>
        public PortManager(PortManagerOptions options)
        {
            Contract.Requires(options != null);
            _options = options.Clone();
            _ports = new List<IPort>();
        }
        
        /// <summary>
        /// The message types which are handled by this process
        /// </summary>
        public IEnumerable<Type> MessageTypes
        {
            get { return new Type[] { }; }
        }

        /// <summary>
        /// Handles a message queued on this session.
        /// </summary>
        /// <param name="message">The message to handle</param>
        public bool HandleMessage(IMessage message)
        {
            return false;
        }

        /// <summary>
        /// Retrieves a registered port by its port id
        /// </summary>
        /// <param name="portId">The id of the port to retrieve</param>
        /// <returns>The port instance, or null if no port was found</returns>
        private IPort _getPort(byte portId)
        {
            for(int i = 0; i < _ports.Count; i++)
            {
                if (_ports[i].PortId == portId)
                    return _ports[i];
            }
            return null;
        }

        /// <summary>
        /// Unregisters all port registrations
        /// </summary>
        private void _disposePorts()
        {
            foreach(var registration in _ports)
            {
                registration.Dispose();
            }
            _ports.Clear();
        }

        /// <summary>
        /// Disposes of all resources held by the port manager
        /// </summary>
        private void _disposeAll()
        {
            _disposePorts();
        }

        /// <summary>
        /// Resolves the port manager's dependencies
        /// </summary>
        /// <param name="processes">The processes that are potential dependencies</param>
        public void Resolve(IEnumerable<IProcess> processes)
        {
            lock(_lock)
            {
                var ports = processes.OfType<IPort>();
                this._ports.AddRange(ports);
            }
        }


        /// <summary>
        /// Disposes the port manager
        /// </summary>
        public void Dispose()
        {
            lock (_lock)
            {
                this._disposeAll();
            }
        }

        /// <summary>
        /// Removes a registration from the port manager
        /// </summary>
        /// <param name="port">The port to remove</param>
        private void _removePort(IPort port)
        {
            lock(_lock)
            {
                this._ports.Remove(port);
            }
        }

        /// <summary>
        /// Sends a netgram
        /// </summary>
        /// <param name="netgram">The netgram to send</param>
        public void SendNetgram(OutboundNetgram netgram)
        {
            lock(_lock)
            {
                if (netgram.PortId == 255)
                {
                    // send to all ports
                    foreach (var port in _ports)
                    {
                        port.SendNetgram(netgram);
                    }
                }
                else
                {
                    var port = _getPort(netgram.PortId);
                    if (port != null)
                    {
                        port.SendNetgram(netgram);
                    }
                }
            }
        }
        
               

    }
}
