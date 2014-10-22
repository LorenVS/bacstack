using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BACnet.Core;
using BACnet.Core.Datalink;
using BACnet.IP.Bvlc;

namespace BACnet.IP
{
    public class ForeignDevicePort : IPort, IDisposable
    {
        public enum State
        {
            Closed,
            Registering,
            Renewing,
            Open
        }

        /// <summary>
        /// The options that the port was created with
        /// </summary>
        private readonly ForeignDevicePortOptions _options;

        /// <summary>
        /// The process id of the foreign device port
        /// </summary>
        public int ProcessId { get { return _options.ProcessId; } }

        /// <summary>
        /// Retrieves the port id of the port
        /// </summary>
        public byte PortId { get { return _options.PortId; } }

        /// <summary>
        /// Lock used to synchronize access to the port
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// The current state of the port
        /// </summary>
        private State _state;

        /// <summary>
        /// The BACnet mac address of the bbmd
        /// </summary>
        private Mac _bbmdMac;

        /// <summary>
        /// The udp server used to send and receive datagrams
        /// </summary>
        private UDPAsyncServer _server;

        /// <summary>
        /// The observers that are used to receive netgrams
        /// from this port
        /// </summary>
        private readonly SubscriptionList<InboundNetgram> _netgramObservers;

        /// <summary>
        /// The timer instance used to schedule foreign device registration
        /// </summary>
        private Timer _registrationTimer;

        /// <summary>
        /// The timestamp at which the current foreign device registration
        /// expires
        /// </summary>
        private DateTime _registrationTimeout;

        /// <summary>
        /// Constructs a new foreign device port instance
        /// </summary>
        /// <param name="options">The options to create the port with</param>
        public ForeignDevicePort(ForeignDevicePortOptions options)
        {
            this._options = options.Clone();
            this._state = State.Closed;
            this._server = null;
            this._netgramObservers = new SubscriptionList<InboundNetgram>();
            this._registrationTimer = null;
            this._registrationTimeout = DateTime.MinValue;
        }
        
        /// <summary>
        /// Begins registration of the foreign device
        /// with the BBMD
        /// </summary>
        private void _startRegistering()
        {
            if (_registrationTimer == null)
                _registrationTimer = new Timer(_register);

            _registrationTimer.Change(
                TimeSpan.Zero,
                TimeSpan.FromSeconds(5));
        }

        /// <summary>
        /// Registers the foreign device with the remote bbmd
        /// </summary>
        /// <param name="ttl">The time to live to register with</param>
        private void _register(object state)
        {
            bool register = false;

            lock (_lock)
            {
                if (_state == State.Registering || _state == State.Renewing)
                {
                    register = true;
                }
                else if (_state == State.Open && DateTime.UtcNow > _registrationTimeout)
                {
                    _state = State.Renewing;
                    register = true;
                }

                if (register)
                {
                    // we use double the registration interval for the ttl
                    int ttl = (int)_options.RegistrationInterval.TotalSeconds;
                    ttl *= 2;

                    RegisterForeignDeviceMessage message = new RegisterForeignDeviceMessage();
                    message.TTL = (ushort)ttl;

                    lock (_lock)
                    {
                        _sendMessage(_bbmdMac, message);
                    }
                }
            }
        }

        /// <summary>
        /// Checks to make sure that the port is in <paramref name="state" />
        /// </summary>
        /// <param name="state">The state to require</param>
        private void _requireState(State state)
        {
            if(_state != state)
            {
                throw new Exception("Can't perform the desired operation in " + state.ToString() + " state");
            }
        }

        /// <summary>
        /// Creates the bbmd mac address
        /// </summary>
        private void _createBbmdMac()
        {
            var bbmdIps = Dns.GetHostAddresses(_options.BbmdHost);
            if (bbmdIps.Length == 0)
            {
                throw new Exception("Could not resolve '" + _options.BbmdHost + "'");
            }

            var bbmdEp = new IPEndPoint(bbmdIps[0], _options.BbmdPort);
            _bbmdMac = IPUtils.IPEndPointToMac(bbmdEp);
        }

        /// <summary>
        /// Creates the udp socket
        /// </summary>
        private void _createServer()
        {
            IPAddress ip = null;
            if(!IPAddress.TryParse(_options.LocalHost, out ip))
            {
                var localIps = Dns.GetHostAddresses(_options.LocalHost);
                if (localIps.Length == 0)
                {
                    throw new Exception("Could not resolve '" + _options.LocalHost + "'");
                }
                else
                {
                    ip = localIps[0];
                }
            }

            var localEp = new IPEndPoint(ip, _options.LocalPort);
            _server = new UDPAsyncServer(localEp, _onDatagramReceived);
        }

        /// <summary>
        /// Creates a new bvlc message based on a function code
        /// </summary>
        /// <param name="function">The function code of the message type to create</param>
        /// <returns>The newly created message instance</returns>
        private IBvlcMessage _createMessage(FunctionCode function)
        {
            switch(function)
            {
                case FunctionCode.Result:
                    return new ResultMessage();
                case FunctionCode.RegisterForeignDevice:
                    return new RegisterForeignDeviceMessage();
                case FunctionCode.ForwardedNpdu:
                    return new ForwardedNpduMessage();
                case FunctionCode.OriginalUnicastNpdu:
                    return new OriginalUnicastNpduMessage();
                case FunctionCode.OriginalBroadcastNpdu:
                    return new OriginalBroadcastNpduMessage();
            }

            throw new Exception("Could not create message with function " + function.ToString());
        }

        /// <summary>
        /// Called whenever a datagram is received
        /// </summary>
        /// <param name="ep">The IPEndPoint of the sending device</param>
        /// <param name="buffer">The buffer containing the datagram</param>
        /// <param name="length">The length of the received datagram</param>
        /// <returns>The inbound netgram to propagate, if any</returns>
        private void _onDatagramReceived(IPEndPoint ep, byte[] buffer, int length)
        {
            int offset = 0;
            BvlcHeader header = null;
            IBvlcMessage message = null;
            InboundNetgram netgram = null;
            Mac mac = IPUtils.IPEndPointToMac(ep);

            try
            {
                if (length < 4)
                    throw new Exception("Received datagram under 4 bytes long");

                header = new BvlcHeader();
                offset = header.Deserialize(buffer, offset);

                if (header.Length != length)
                    throw new Exception("Received bvlc datagram with non-matching lengths");

                message = _createMessage(header.Function);
                offset = message.Deserialize(buffer, offset);
                lock (_lock)
                {
                    netgram = _processMessage(mac, message, buffer, offset, length);
                }

                if (netgram != null)
                    _netgramObservers.Next(netgram);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Processes a received bvlc message
        /// </summary>
        /// <param name="mac">The mac address of the device that sent the message</param>
        /// <param name="message">The bvlc message</param>
        /// <param name="buffer">The buffer containing the message content</param>
        /// <param name="offset">The offset of the content within the message</param>
        /// <param name="length">The length of the received datagram</param>
        /// <returns>The inbound netgram to propagate, if any</returns>
        private InboundNetgram _processMessage(Mac mac, IBvlcMessage message, byte[] buffer, int offset, int length)
        {
            switch(message.Function)
            {
                case FunctionCode.Result:
                    _processResult(mac, ((ResultMessage)message).Result);
                    break;
                case FunctionCode.ForwardedNpdu:
                    mac = ((ForwardedNpduMessage)message).OriginalMac;
                    goto case FunctionCode.OriginalUnicastNpdu;
                case FunctionCode.OriginalUnicastNpdu:
                case FunctionCode.OriginalBroadcastNpdu:
                    return _createInboundNetgram(mac, buffer, offset, length);
            }

            return null;
        }

        /// <summary>
        /// Creates an inbound netgram
        /// </summary>
        /// <param name="mac">The mac address of the device that sent the netgram</param>
        /// <param name="buffer">The buffer containing the netgram content</param>
        /// <param name="offset">The offset of the netgram within the buffer</param>
        /// <param name="length">The length of the received datagram</param>
        private InboundNetgram _createInboundNetgram(Mac mac, byte[] buffer, int offset, int length)
        {
            InboundNetgram netgram = new InboundNetgram(this);
            netgram.Source = mac;
            netgram.Segment = new BufferSegment(buffer, offset, length);
            return netgram;
        }

        /// <summary>
        /// Processes a received result message
        /// </summary>
        /// <param name="mac">The mac address of the device that sent the message</param>
        /// <param name="code">The result code of the message</param>
        private void _processResult(Mac mac, ResultCode code)
        {
            // we only process certain results from the BBMD
            if (mac == _bbmdMac)
            {
                if(code == ResultCode.Success && (_state == State.Registering || _state == State.Renewing))
                {
                    Console.WriteLine("Register Success");
                    _state = State.Open;
                    _registrationTimeout = DateTime.UtcNow.Add(_options.RegistrationInterval);
                }
            }
        }

        /// <summary>
        /// Sends a bvlc message
        /// </summary>
        /// <param name="mac">The BACnet mac address of the destination device</param>
        /// <param name="message"></param>
        private void _sendMessage(Mac mac, IBvlcMessage message)
        {
            // TODO: constant for buffer size, or
            // lease buffers from the UDPAsyncServer instance
            IPEndPoint ep = IPUtils.MacToIPEndPoint(mac);
            byte[] buffer = new byte[1500];
            int offset = 0;
            BvlcHeader header = null;
            
            
            header = new BvlcHeader();
            header.Function = message.Function;
            header.Length = 0;
            offset = header.Serialize(buffer, offset);
            offset = message.Serialize(buffer, offset);

            // patch the length in now that it is known
            buffer[2] = (byte)(offset << 8);
            buffer[3] = (byte)(offset);

            _server.Send(ep, buffer, offset);
        }

        /// <summary>
        /// Disposes all of the resources held by an open port, in preparation
        /// for transitioning back to a closed state
        /// </summary>
        private void _disposeAll()
        {
            if (_registrationTimer != null)
            {
                _registrationTimer.Dispose();
                _registrationTimer = null;
            }

            if(_server != null)
            {
                _server.Dispose();
                _server = null;
            }

            _netgramObservers.Clear();
        }

        /// <summary>
        /// Disposes of the foreign device port
        /// </summary>
        public void Dispose()
        {
            lock(_lock)
            {
                _disposeAll();
            }
        }

        /// <summary>
        /// Resolves the port's dependencies
        /// </summary>
        /// <param name="processes">The processes</param>
        public void Resolve(IEnumerable<IProcess> processes)
        {

        }

        /// <summary>
        /// Opens the foreign device port
        /// </summary>
        public void Open()
        {
            lock(_lock)
            {
                _requireState(State.Closed);

                // make sure that everything is cleaned up
                // from previous times the port was open, should be nop
                _disposeAll();
                
                try
                {
                    _createBbmdMac();

                    // creates the UDP server
                    _createServer();

                    // start foreign device registration
                    _state = State.Registering;
                    _startRegistering();

                }
                catch
                {
                    // clean up anything that has been created
                    _disposeAll();
                    _state = State.Closed;
                    throw;
                }
            }
        }
        
        /// <summary>
        /// Sends a netgram out of this port
        /// </summary>
        /// <param name="netgram">The netgram to send</param>
        public void SendNetgram(OutboundNetgram netgram)
        {
            IPEndPoint ep = null;
            byte[] buffer = new byte[1500];
            int offset = 0;
            BvlcHeader header = new BvlcHeader();
            IBvlcMessage message = null;

            if (netgram.Destination.IsBroadcast())
            {
                ep = IPUtils.MacToIPEndPoint(_bbmdMac);
                header.Function = FunctionCode.OriginalUnicastNpdu;
                //header.Function = FunctionCode.OriginalBroadcastNpdu;
                header.Length = 0;
                message = new OriginalBroadcastNpduMessage();
            }
            else
            {
                ep = IPUtils.MacToIPEndPoint(netgram.Destination);
                header.Function = FunctionCode.OriginalUnicastNpdu;
                header.Length = 0;
                message = new OriginalUnicastNpduMessage();
            }

            offset = header.Serialize(buffer, offset);
            offset = message.Serialize(buffer, offset);
            offset = netgram.Content.Serialize(buffer, offset);

            // patch the length
            buffer[2] = (byte)(offset << 8);
            buffer[3] = (byte)(offset);

            lock(this._lock)
            {
                if (_server != null)
                {
                    _server.Send(ep, buffer, offset);
                }
            }
        }

        /// <summary>
        /// Registers a new netgram observer with the port
        /// </summary>
        /// <param name="observer">The observer to register</param>
        public IDisposable Subscribe(IObserver<InboundNetgram> observer)
        {
            var ret = new NetgramSubscription(this, observer);
            this._netgramObservers.Register(observer);
            return ret;
        }

        /// <summary>
        /// Unsubscribes a netgram observer from the port
        /// </summary>
        /// <param name="observer">The observer to unsubscribe</param>
        private void _unsubscribe(IObserver<InboundNetgram> observer)
        {
            this._netgramObservers.Unregister(observer);
        }

        private class NetgramSubscription : IDisposable
        {
            private ForeignDevicePort _port;
            private IObserver<InboundNetgram> _observer;

            public NetgramSubscription(ForeignDevicePort port, IObserver<InboundNetgram> observer)
            {
                this._port = port;
                this._observer = observer;
            }

            public void Dispose()
            {
                _port._unsubscribe(_observer);
            }
        }
    }
}
