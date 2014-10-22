using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Core.Network;
using BACnet.Core.App.Messages;
using BACnet.Core.App.Transactions;
using BACnet.Tagging;
using BACnet.Types;

namespace BACnet.Core.App
{
    public class Host : IProcess, IObserver<InboundAppgram>, IObservable<InboundUnconfirmedRequest>
    {
        /// <summary>
        /// The number of times to attempt a device search
        /// </summary>
        public const int DeviceSearchAttempts = 2;

        /// <summary>
        /// The number of seconds to delay between device search attempts
        /// </summary>
        public static readonly TimeSpan DeviceSearchInterval = TimeSpan.FromSeconds(2);

        /// <summary>
        /// Lock used to synchronize access to the host
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// The options instance for the host
        /// </summary>
        private readonly HostOptions _options;

        /// <summary>
        /// Retrieves the process id of the host
        /// </summary>
        public int ProcessId { get { return _options.ProcessId; } }

        /// <summary>
        /// The device table for this host
        /// </summary>
        private readonly DeviceTable _devices;

        /// <summary>
        /// List of device searches that are currently active
        /// </summary>
        private readonly LinkedList<DeviceSearch> _deviceSearches;

        /// <summary>
        /// The transaction manager for the host
        /// </summary>
        private readonly TransactionManager _txManager;

        /// <summary>
        /// The observers that are subscribed to unconfirmed requests
        /// </summary>
        private readonly SubscriptionList<InboundUnconfirmedRequest> _unconfirmedRequestObservers;

        /// <summary>
        /// The router instance used for sending and
        /// receiving appgrams
        /// </summary>
        private Router _router;

        /// <summary>
        /// The observable subscription from the router
        /// for inbound appgrams
        /// </summary>
        private IDisposable _routerSubscription;

        /// <summary>
        /// Constructs a new Host instance
        /// </summary>
        public Host(HostOptions options)
        {
            this._options = options.Clone();
            this._devices = new DeviceTable();
            this._deviceSearches = new LinkedList<DeviceSearch>();
            this._txManager = new TransactionManager(this);
            this._unconfirmedRequestObservers = new SubscriptionList<InboundUnconfirmedRequest>();
        }

        /// <summary>
        /// Disposes of the router's subscription, and removes
        /// this host's reference to the router
        /// </summary>
        private void _disposeRouter()
        {
            if(_routerSubscription != null)
            {
                _routerSubscription.Dispose();
                _routerSubscription = null;
            }

            _router = null;
        }

        /// <summary>
        /// Creates an app message using a message type
        /// </summary>
        /// <param name="type">The type of message to create</param>
        /// <returns>The created message</returns>
        private IAppMessage _createMessage(MessageType type)
        {
            switch(type)
            {
                case MessageType.ConfirmedRequest:
                    return new ConfirmedRequestMessage();
                case MessageType.UnconfirmedRequest:
                    return new UnconfirmedRequestMessage();
                case MessageType.SimpleAck:
                    return new SimpleAckMessage();
                case MessageType.ComplexAck:
                    return new ComplexAckMessage();
                case MessageType.SegmentAck:
                    return new SegmentAckMessage();
                case MessageType.Error:
                    return new ErrorMessage();
                case MessageType.Reject:
                    return new RejectMessage();
                case MessageType.Abort:
                    return new AbortMessage();
            }

            throw new Exception("Unknown message type: " + type);
        }


        /// <summary>
        /// Loads an unconfirmed request from its tagged form
        /// </summary>
        /// <param name="serviceChoice">The service choice of the unconfirmed request</param>
        /// <param name="buffer">The buffer containing the tagged form</param>
        /// <param name="offset">The offset of the tagged form</param>
        /// <param name="end">The end of the tagged form</param>
        /// <returns>The loaded unconfirmed request</returns>
        private IUnconfirmedRequest _loadUnconfirmedRequest(UnconfirmedServiceChoice serviceChoice, byte[] buffer, int offset, int end)
        {
            var registration = _options.UnconfirmedRegistrar.GetRegistration(serviceChoice);
            IUnconfirmedRequest request = null;

            using (MemoryStream ms = new MemoryStream(buffer, offset, end - offset, false, false))
            {
                TagReader reader = new TagReader(ms);
                TagReaderStream stream = new TagReaderStream(reader, registration.Schema);
                request = registration.Load(stream);
            }

            return request;
        }

        /// <summary>
        /// Tags a confirmed request and retrieves its content
        /// as a byte array
        /// </summary>
        /// <param name="request">The request to save</param>
        /// <returns>The tagged byte array</returns>
        private byte[] _saveConfirmedRequest(IConfirmedRequest request)
        {
            var registration = _options.ConfirmedRegistrar.GetRegistration(request.ServiceChoice);

            using(MemoryStream ms = new MemoryStream())
            {
                TagWriter writer = new TagWriter(ms);
                TagWriterSink sink = new TagWriterSink(writer, registration.Schema);
                registration.Saver(sink, request);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Processes a received message
        /// </summary>
        /// <param name="appgram">The appgram that contains the received message</param>
        /// <param name="message">The message</param>
        /// <param name="buffer">The buffer containing the message content</param>
        /// <param name="offset">The start of the content within the buffer</param>
        /// <param name="end">The end of the content within the buffer</param>
        /// <returns>The inbound unconfirmed request to propagate, if any</returns>
        private InboundUnconfirmedRequest _processMessage(InboundAppgram appgram, IAppMessage message, byte[] buffer, int offset, int end)
        {
            var source = appgram.Source;
            var segment = new BufferSegment(buffer, offset, end);
            InboundUnconfirmedRequest request = null;

            switch(message.Type)
            {
                case MessageType.ConfirmedRequest:
                    _txManager.ProcessConfirmedRequest(source, (ConfirmedRequestMessage)message, segment);
                    break;
                case MessageType.UnconfirmedRequest:
                    request = _processUnconfirmedRequest(appgram, (UnconfirmedRequestMessage)message, buffer, offset, end);
                    break;
                case MessageType.SimpleAck:
                    _txManager.ProcessSimpleAck(source, (SimpleAckMessage)message);
                    break;
                case MessageType.ComplexAck:
                    _txManager.ProcessComplexAck(source, (ComplexAckMessage)message, segment);
                    break;
                case MessageType.SegmentAck:
                    _txManager.ProcessSegmentAck(source, (SegmentAckMessage)message);
                    break;
                case MessageType.Error:
                    _txManager.ProcessError(source, (ErrorMessage)message);
                    break;
                case MessageType.Reject:
                    _txManager.ProcessReject(source, (RejectMessage)message);
                    break;
                case MessageType.Abort:
                    _txManager.ProcessAbort(source, (AbortMessage)message);
                    break;
            }

            return request;
        }

        /// <summary>
        /// Processes a received unconfirmed request
        /// </summary>
        /// <param name="appgram">The inbound appgram</param>
        /// <param name="message">The unconfirmed request message</param>
        /// <param name="buffer">The buffer containing the request content</param>
        /// <param name="offset">The offset of the request content in the buffer</param>
        /// <param name="end">The end of the request content</param>
        private InboundUnconfirmedRequest _processUnconfirmedRequest(InboundAppgram appgram, UnconfirmedRequestMessage message, byte[] buffer, int offset, int end)
        {
            UnconfirmedServiceChoice choice = (UnconfirmedServiceChoice)message.ServiceChoice;
            IUnconfirmedRequest request = _loadUnconfirmedRequest(choice, buffer, offset, end);
            DeviceTableEntry source = null;

            if (request.ServiceChoice == UnconfirmedServiceChoice.IAm)
            {
                // the only request that we handle at the host level is an IAm request,
                // which is necessary for TSM operations, since information about the remote
                // host is needed
                source = _processIAmRequest(appgram, (IAmRequest)request);
            }
            else
            {
                source = _devices.GetByAddress(appgram.Source);
            }

            InboundUnconfirmedRequest inbound = new InboundUnconfirmedRequest(
                appgram.Source,
                source,
                request);

            return inbound;
        }

        /// <summary>
        /// Processes a received IAm request
        /// </summary>
        /// <param name="appgram">The appgram that contained the request</param>
        /// <param name="request">The IAm request</param>
        /// <returns>The created device table entry</returns>
        private DeviceTableEntry _processIAmRequest(InboundAppgram appgram, IAmRequest request)
        {
            DeviceTableEntry entry = new DeviceTableEntry(
                request.IAmDeviceIdentifier.Instance,
                appgram.Source,
                request.MaxAPDULengthAccepted,
                request.SegmentationSupported,
                (ushort)request.VendorID);

            _devices.Upsert(entry);

            for(var node = _deviceSearches.First; node != null; node = node.Next)
            {
                if(node.Value.Feed(entry))
                {
                    node.Value.Dispose();
                    _deviceSearches.Remove(node);
                }
            }

            return entry;
        }

        /// <summary>
        /// Sends an unconfirmed request
        /// </summary>
        /// <param name="destination">The destination address</param>
        /// <param name="expectingReply">Whether or not a response is expected</param>
        /// <param name="request">The unconfirmed request</param>
        private void _sendUnconfirmedRequest(Address destination, bool expectingReply, IUnconfirmedRequest request)
        {
            var registration = _options.UnconfirmedRegistrar.GetRegistration(request.ServiceChoice);

            UnconfirmedRequestMessage message = new UnconfirmedRequestMessage();
            message.ServiceChoice = (byte)request.ServiceChoice;

            byte[] raw = null;
            using(var ms = new MemoryStream())
            {
                TagWriter writer = new TagWriter(ms);
                TagWriterSink sink = new TagWriterSink(writer, registration.Schema);
                registration.Save(sink, request);
                raw = ms.ToArray();
            }

            OutboundAppgram appgram = new OutboundAppgram();
            appgram.Content = new AppgramContent(message, new BufferSegment(raw, 0, raw.Length));
            appgram.Destination = destination;
            appgram.ExpectingReply = expectingReply;
            appgram.Priority = NetgramPriority.Normal;

            if (_router != null)
                _router.SendAppgram(appgram);
        }

        /// <summary>
        /// Disposes of the host
        /// </summary>
        public void Dispose()
        {
            lock(_lock)
            {
                _disposeRouter();
            }
        }

        /// <summary>
        /// Resolves the host's dependencies
        /// </summary>
        /// <param name="processes">The processes</param>
        public void Resolve(IEnumerable<IProcess> processes)
        {
            lock(_lock)
            {
                _disposeRouter();

                var router = processes.OfType<Router>().FirstOrDefault();
                if(router != null)
                {
                    _router = router;
                    _routerSubscription = router.Subscribe(this);
                }
            }
        }

        /// <summary>
        /// Retrieves a device table entry by device instance
        /// </summary>
        /// <param name="instance">The device instance</param>
        /// <returns>The device table entry, or null if no such instance exists</returns>
        public DeviceTableEntry GetDeviceTableEntry(uint instance)
        {
            lock(_lock)
            {
                return _devices.Get(instance);
            }
        }
        
        /// <summary>
        /// Retrieves a device table entry by the remote address
        /// </summary>
        /// <param name="address">The address of the device</param>
        /// <returns>The device table entry</returns>
        public DeviceTableEntry GetDeviceTableEntryByAddress(Address address)
        {
            lock(_lock)
            {
                return _devices.GetByAddress(address);
            }
        }

        /// <summary>
        /// Sends a who is request to find a device at
        /// a specific address
        /// </summary>
        /// <param name="address">The address of the device</param>
        private void _sendWhoIsForAddress(Address address)
        {
            WhoIsRequest request = new WhoIsRequest(
                Option<uint>.None,
                Option<uint>.None);
            _sendUnconfirmedRequest(address, true, request);
        }

        /// <summary>
        /// Sends a who is request to find a device with
        /// a specific instance
        /// </summary>
        /// <param name="instance">The instance of the device</param>
        private void _sendWhoIsForInstance(uint instance)
        {
            WhoIsRequest request = new WhoIsRequest(
                instance,
                instance);
            _sendUnconfirmedRequest(Address.GlobalBroadcast, true, request);
        }

        /// <summary>
        /// Called whenever a timer ticks for a device search
        /// </summary>
        /// <param name="search">The device search</param>
        private void _searchTick(DeviceSearch search)
        {
            lock(_lock)
            {
                var node = _deviceSearches.Find(search);
                if (node != null)
                {
                    search.Attempt++;
                    if (search.Attempt >= DeviceSearchAttempts)
                    {
                        _deviceSearches.Remove(node);
                        search.Callback.DeviceSearchTimedOut();
                        search.Dispose();
                    }
                    else if (search.IsInstanceSearch)
                    {
                        _sendWhoIsForInstance(search.Instance);
                    }
                    else
                    {
                        _sendWhoIsForAddress(search.Address);
                    }
                }
            }
        }

        /// <summary>
        /// Searches for a device (Sends a who-is request)
        /// </summary>
        /// <param name="instance">The instance of the device to search for</param>
        /// <param name="callback">The callback to invoke when the device is found</param>
        public void SearchForDevice(uint instance, IDeviceSearchCallback callback)
        {
            lock(_lock)
            {
                var device = _devices.Get(instance);
                if (device != null)
                    callback.DeviceFound(device);
                else
                {
                    DeviceSearch search = new DeviceSearch(this, instance, callback);
                    this._deviceSearches.AddLast(search);
                    _sendWhoIsForInstance(instance);
                }
            }
        }

        /// <summary>
        /// Searches for a device (Sends a who-is request)
        /// </summary>
        /// <param name="address">The address of the device to search for</param>
        /// <param name="callback">The callback to invoke when the device is found</param>
        public void SearchForDevice(Address address, IDeviceSearchCallback callback)
        {
            lock(_lock)
            {
                var device = _devices.GetByAddress(address);
                if (device != null)
                    callback.DeviceFound(device);
                else
                {
                    DeviceSearch search = new DeviceSearch(this, address, callback);
                    this._deviceSearches.AddLast(search);
                    _sendWhoIsForAddress(address);
                }
            }
        }

        /// <summary>
        /// Sends an unconfirmed request
        /// </summary>
        /// <param name="address">The address of the destination device</param>
        /// <param name="expectingReply">True if a reply is expected to the request, false otherwise</param>
        /// <param name="request">The request to send</param>
        public void SendUnconfirmedRequest(Address address, bool expectingReply, IUnconfirmedRequest request)
        {
            lock(_lock)
            {
                _sendUnconfirmedRequest(address, expectingReply, request);
            }
        }

        /// <summary>
        /// Sends an unconfirmed request
        /// </summary>
        /// <param name="instance">The instance of the destination device</param>
        /// <param name="expectingReply">True if a reply is expected to the request, false otherwise</param>
        /// <param name="request">The request to send</param>
        public void SendUnconfirmedRequest(uint instance, bool expectingReply, IUnconfirmedRequest request)
        {
            lock(_lock)
            {
                
            }
        }

        /// <summary>
        /// Sends a confirmed request
        /// </summary>
        /// <param name="address">The address of the destination device</param>
        /// <param name="request">The request to send</param>
        public IClientTransactionHandle SendConfirmedRequest(Address address, IConfirmedRequest request)
        {
            lock(_lock)
            {
                var bytes = _saveConfirmedRequest(request);
                return _txManager.SendConfirmedRequest(address, (byte)request.ServiceChoice, bytes);       
            }
        }

        /// <summary>
        /// Sends a confirmed request
        /// </summary>
        /// <param name="instance">The address of the destination device</param>
        /// <param name="request">The request to send</param>
        public IClientTransactionHandle SendConfirmedRequest(uint instance, IConfirmedRequest request)
        {
            lock(_lock)
            {
                var bytes = _saveConfirmedRequest(request);
                return _txManager.SendConfirmedRequest(instance, (byte)request.ServiceChoice, bytes);
            }
        }

        /// <summary>
        /// Sends a raw appgram
        /// </summary>
        /// <param name="address">The address of the destination device</param>
        /// <param name="expectingReply">True if a response is expected, false otherwise</param>
        /// <param name="message">The header message</param>
        /// <param name="content">The content of the appgram</param>
        internal void SendRaw(Address address, bool expectingReply, IAppMessage message, BufferSegment content)
        {
            lock(_lock)
            {
                OutboundAppgram appgram = new OutboundAppgram();
                appgram.Content = new AppgramContent(message, content);
                appgram.Destination = address;
                appgram.ExpectingReply = expectingReply;
                appgram.Priority = NetgramPriority.Normal;

                if (_router != null)
                    _router.SendAppgram(appgram);        
            }
        }


        /// <summary>
        /// Called whenever the router instance receives an appgram
        /// </summary>
        /// <param name="value">The appgram value</param>
        void IObserver<InboundAppgram>.OnNext(InboundAppgram value)
        {
            try
            {
                var segment = value.Segment;
                byte[] buffer = segment.Buffer;
                int offset = segment.Offset;
                int end = segment.End;

                if (offset >= end)
                    throw new Exception("Received appgram with no content");

                byte header = segment.Buffer[segment.Offset];
                MessageType type = (MessageType)(header >> 4);
                IAppMessage message = _createMessage(type);
                offset = message.Deserialize(buffer, offset, end);

                InboundUnconfirmedRequest request = null;
                lock(_lock)
                {
                    request = _processMessage(value, message, buffer, offset, end);
                }

                if(request != null)
                {
                    _unconfirmedRequestObservers.Next(request);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Called whenever an error occurs while receiving an appgram
        /// </summary>
        /// <param name="error">The error</param>
        void IObserver<InboundAppgram>.OnError(Exception error)
        {
        }

        /// <summary>
        /// Called when there are no more appgrams to receive
        /// from the router instance
        /// </summary>
        void IObserver<InboundAppgram>.OnCompleted()
        {
            lock(_lock)
            {
                _disposeRouter();
            }
        }

        /// <summary>
        /// Subscribes an observer to be notified
        /// when an unconfirmed request is received
        /// </summary>
        /// <param name="observer">The observer to subscribe</param>
        /// <returns>A disposable object that can be used to terminate the subscription</returns>
        public IDisposable Subscribe(IObserver<InboundUnconfirmedRequest> observer)
        {
            _unconfirmedRequestObservers.Register(observer);
            return new UnconfirmedRequestSubscription(this, observer);
        }

        /// <summary>
        /// Unsubscribes a unconfirmed request observer
        /// </summary>
        /// <param name="observer">The observer to unsubscribe</param>
        private void _unsubscribe(IObserver<InboundUnconfirmedRequest> observer)
        {
            _unconfirmedRequestObservers.Unregister(observer);
        }

        public class AppgramContent : IContent
        {
            /// <summary>
            /// The appgram header
            /// </summary>
            public IAppMessage Message{get; private set;}

            /// <summary>
            /// The content of the request
            /// </summary>
            public BufferSegment Content{get; private set;}

            /// <summary>
            /// Constructs a new AppgramContent instance
            /// </summary>
            /// 
            /// <param name="schema">The schema of the request</param>
            public AppgramContent(IAppMessage message, BufferSegment content)
            {
                this.Message = message;
                this.Content = content;
            }

            /// <summary>
            /// Serializes the content to a buffer
            /// </summary>
            /// <param name="buffer">The buffer to serialize to</param>
            /// <param name="offset">The offset to begin serializing</param>
            /// <returns>The new offset</returns>
            public int Serialize(byte[] buffer, int offset)
            {
                offset = Message.Serialize(buffer, offset);
                Array.Copy(Content.Buffer, Content.Offset, buffer, offset, Content.End - Content.Offset);
                offset += Content.End - Content.Offset;
                return offset;
            }
        }

        private class DeviceSearch : IDisposable
        {
            /// <summary>
            /// The host instance
            /// </summary>
            public Host Host { get; private set; }

            /// <summary>
            /// True if this device search is searching
            /// for a device instance, false if it is searching
            /// for an address
            /// </summary>
            public bool IsInstanceSearch { get; private set; }

            /// <summary>
            /// The device instance that is being searched for
            /// </summary>
            public uint Instance { get; private set; }
            
            /// <summary>
            /// The address that is being searched for
            /// </summary>
            public Address Address { get; private set; }

            /// <summary>
            /// The callback object for the search
            /// </summary>
            public IDeviceSearchCallback Callback { get; private set; }

            public int Attempt { get; set; }
            private Timer _timer;

            /// <summary>
            /// Constructs a new device search instance
            /// </summary>
            /// <param name="host">The host instance</param>
            /// <param name="instance">The device instance to search for</param>
            /// <param name="callback">The callback object for the search</param>
            public DeviceSearch(Host host, uint instance, IDeviceSearchCallback callback)
            {
                this.Host = host;
                this.IsInstanceSearch = true;
                this.Instance = instance;
                this.Callback = callback;

                this.Attempt = 0;

                this._timer = new Timer(
                    _tick,
                    null,
                    DeviceSearchInterval,
                    DeviceSearchInterval
                    );
            }

            /// <summary>
            /// Constructs a new device search instance
            /// </summary>
            /// <param name="host">The host instance</param>
            /// <param name="address">The device address to search for</param>
            /// <param name="callback">The callback object for the search</param>
            public DeviceSearch(Host host, Address address, IDeviceSearchCallback callback)
            {
                this.Host = host;
                this.IsInstanceSearch = false;
                this.Address = address;
                this.Callback = callback;

                this.Attempt = 0;

                this._timer = new Timer(
                    _tick,
                    null,
                    DeviceSearchInterval,
                    DeviceSearchInterval
                    );
            }

            /// <summary>
            /// Disposes of the device search instance
            /// </summary>
            public void Dispose()
            {
                if(_timer != null)
                {
                    _timer.Dispose();
                    _timer = null;
                }
            }

            /// <summary>
            /// Called whenever the timer ticks
            /// </summary>
            /// <param name="state">The state of the timer</param>
            private void _tick(object state)
            {
                Host._searchTick(this);
            }

            /// <summary>
            /// Feeds a new device table entry into the
            /// search
            /// </summary>
            /// <param name="entry">The to feed</param>
            /// <returns>True if the search is now satisfied, false otherwise</returns>
            public bool Feed(DeviceTableEntry entry)
            {
                bool satisfies = false;

                if (IsInstanceSearch && entry.Instance == Instance)
                    satisfies = true;
                else if (!IsInstanceSearch && entry.Instance != Instance)
                    satisfies = true;

                if(satisfies)
                    this.Callback.DeviceFound(entry);
                return satisfies;
            }

        }

        private class UnconfirmedRequestSubscription : IDisposable
        {
            /// <summary>
            /// The host that owns the subscription
            /// </summary>
            private Host _host;

            /// <summary>
            /// The observer that is subscribed
            /// </summary>
            private IObserver<InboundUnconfirmedRequest> _observer;

            /// <summary>
            /// Constructs a new unconfirmed request subscription
            /// </summary>
            /// <param name="host"></param>
            public UnconfirmedRequestSubscription(Host host, IObserver<InboundUnconfirmedRequest> observer)
            {
                this._host = host;
                this._observer = observer;
            }

            /// <summary>
            /// Disposes of the host
            /// </summary>
            public void Dispose()
            {
                _host._unsubscribe(_observer);
            }
        }

    }
}
