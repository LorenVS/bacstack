using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Datalink
{
    public class PortManager : IProcess, IObservable<InboundNetgram>
    {
        /// <summary>
        /// Retrieves the process id of the process
        /// </summary>
        public int ProcessId { get { return _options.ProcessId; } }

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
        private List<PortRegistration> _ports;

        /// <summary>
        /// Observers who are handling received netgrams
        /// </summary>
        private readonly SubscriptionList<InboundNetgram> _netgramObservers;
        
        /// <summary>
        /// Constructs a new port manager instance
        /// </summary>
        public PortManager(PortManagerOptions options)
        {
            Contract.Requires(options != null);
            _options = options.Clone();
            _ports = new List<PortRegistration>();
            _netgramObservers = new SubscriptionList<InboundNetgram>();
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
                if (_ports[i].Port.PortId == portId)
                    return _ports[i].Port;
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
            _netgramObservers.Clear();
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
                foreach(var port in ports)
                {
                    var registration = new PortRegistration(this, port);
                    this._ports.Add(registration);
                }
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
        /// <param name="registration">The registration to remove</param>
        private void _removeRegistration(PortRegistration registration)
        {
            lock(_lock)
            {
                this._ports.Remove(registration);
            }
        }

        /// <summary>
        /// Subscribes a netgram observer to the port manager
        /// </summary>
        /// <param name="observer">The observer to subscribe</param>
        public IDisposable Subscribe(IObserver<InboundNetgram> observer)
        {
            var ret = new NetgramSubscription(this, observer);
            this._netgramObservers.Register(observer);
            return ret;
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
                        port.Port.SendNetgram(netgram);
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

        /// <summary>
        /// Unsubscribes a netgram observer from the port manager
        /// </summary>
        /// <param name="observer">The observer to unsubscribe</param>
        private void _unsubscribe(IObserver<InboundNetgram> observer)
        {
            this._netgramObservers.Unregister(observer);
        }

        /// <summary>
        /// Called whenever a netgram is received from a port
        /// </summary>
        /// <param name="netgram">The received netgram</param>
        private void _onNetgram(InboundNetgram netgram)
        {
            _netgramObservers.Next(netgram);
        }

        private class PortRegistration : IDisposable, IObserver<InboundNetgram>
        {
            private readonly object _lock = new object();
            public PortManager Manager { get; private set; }
            public IPort Port { get; private set; }
            public IDisposable Subscription { get; private set; }

            public PortRegistration(PortManager manager, IPort port)
            {
                this.Manager = manager;
                this.Port = port;
                this.Subscription = this.Port.Subscribe(this);
            }

            private void _disposeAll()
            {
                if(this.Subscription != null)
                {
                    this.Subscription.Dispose();
                    this.Subscription = null;
                }
            }

            public void Dispose()
            {
                lock (_lock)
                {
                    _disposeAll();
                }
            }

            public void OnCompleted()
            {
                lock (_lock)
                {
                    _disposeAll();
                }
                Manager._removeRegistration(this);
            }

            public void OnError(Exception error)
            {
            }

            public void OnNext(InboundNetgram value)
            {
                Manager._onNetgram(value);
            }
        }
        
        private class NetgramSubscription : IDisposable
        {
            private PortManager _manager;
            private IObserver<InboundNetgram> _observer;

            public NetgramSubscription(PortManager manager, IObserver<InboundNetgram> observer)
            {
                this._manager = manager;
                this._observer = observer;
            }

            public void Dispose()
            {
                _manager._unsubscribe(_observer);
            }
        }


    }
}
