using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Core.Exceptions;
using BACnet.Core.Network;
using BACnet.Core.App.Messages;
using BACnet.Core.App.Transactions;
using BACnet.Tagging;
using BACnet.Types;

namespace BACnet.Core.App
{
    public class Host : IProcess, IObserver<InboundAppgram>, IObservable<InboundUnconfirmedRequest>, ISearchHandler<Recipient>
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
        /// The list of device searches
        /// </summary>
        private SearchList<Recipient, DeviceTableEntry> _deviceSearches;

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
            this._deviceSearches = new SearchList<Recipient, DeviceTableEntry>(this);
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
        /// Saves an unconfirmed request to its tagged form
        /// </summary>
        /// <param name="request">The request to save</param>
        /// <returns>The unconfirmed request content</returns>
        private byte[] _saveUnconfirmedRequest(IUnconfirmedRequest request)
        {
            switch (request.ServiceChoice)
            {
                case UnconfirmedServiceChoice.IAm:
                    return Tags.EncodeBytes((IAmRequest)request);
                case UnconfirmedServiceChoice.IHave:
                    return Tags.EncodeBytes((IHaveRequest)request);
                case UnconfirmedServiceChoice.TimeSynchronization:
                    return Tags.EncodeBytes((TimeSynchronizationRequest)request);
                case UnconfirmedServiceChoice.UnconfirmedCOVNotification:
                    return Tags.EncodeBytes((UnconfirmedCOVNotificationRequest)request);
                case UnconfirmedServiceChoice.UnconfirmedEventNotification:
                    return Tags.EncodeBytes((UnconfirmedEventNotificationRequest)request);
                case UnconfirmedServiceChoice.UnconfirmedPrivateTransfer:
                    return Tags.EncodeBytes((UnconfirmedPrivateTransferRequest)request);
                case UnconfirmedServiceChoice.UnconfirmedTextMessage:
                    return Tags.EncodeBytes((UnconfirmedTextMessageRequest)request);
                case UnconfirmedServiceChoice.UtcTimeSynchronization:
                    return Tags.EncodeBytes((UtcTimeSynchronizationRequest)request);
                case UnconfirmedServiceChoice.WhoHas:
                    return Tags.EncodeBytes((WhoHasRequest)request);
                case UnconfirmedServiceChoice.WhoIs:
                    return Tags.EncodeBytes((WhoIsRequest)request);
                default:
                    throw new RejectException(RejectReason.UnrecognizedService);
            }
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
            IUnconfirmedRequest request = null;

            switch (serviceChoice)
            {
                case UnconfirmedServiceChoice.IAm:
                    request = Tags.Decode<IAmRequest>(buffer, offset, end);
                    break;
                case UnconfirmedServiceChoice.IHave:
                    request = Tags.Decode<IHaveRequest>(buffer, offset, end);
                    break;
                case UnconfirmedServiceChoice.TimeSynchronization:
                    request = Tags.Decode<TimeSynchronizationRequest>(buffer, offset, end);
                    break;
                case UnconfirmedServiceChoice.UnconfirmedCOVNotification:
                    request = Tags.Decode<UnconfirmedCOVNotificationRequest>(buffer, offset, end);
                    break;
                case UnconfirmedServiceChoice.UnconfirmedEventNotification:
                    request = Tags.Decode<UnconfirmedEventNotificationRequest>(buffer, offset, end);
                    break;
                case UnconfirmedServiceChoice.UnconfirmedPrivateTransfer:
                    request = Tags.Decode<UnconfirmedPrivateTransferRequest>(buffer, offset, end);
                    break;
                case UnconfirmedServiceChoice.UnconfirmedTextMessage:
                    request = Tags.Decode<UnconfirmedTextMessageRequest>(buffer, offset, end);
                    break;
                case UnconfirmedServiceChoice.UtcTimeSynchronization:
                    request = Tags.Decode<UtcTimeSynchronizationRequest>(buffer, offset, end);
                    break;
                case UnconfirmedServiceChoice.WhoHas:
                    request = Tags.Decode<WhoHasRequest>(buffer, offset, end);
                    break;
                case UnconfirmedServiceChoice.WhoIs:
                    request = Tags.Decode<WhoIsRequest>(buffer, offset, end);
                    break;
                default:
                    throw new RejectException(RejectReason.UnrecognizedService);
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
            switch (request.ServiceChoice)
            {
                case ConfirmedServiceChoice.AcknowledgeAlarm:
                    return Tags.EncodeBytes((AcknowledgeAlarmRequest)request);
                case ConfirmedServiceChoice.AddListElement:
                    return Tags.EncodeBytes((AddListElementRequest)request);
                case ConfirmedServiceChoice.AtomicReadFile:
                    return Tags.EncodeBytes((AtomicReadFileRequest)request);
                case ConfirmedServiceChoice.AtomicWriteFile:
                    return Tags.EncodeBytes((AtomicWriteFileRequest)request);
                case ConfirmedServiceChoice.Authenticate:
                    return Tags.EncodeBytes((AuthenticateRequest)request);
                case ConfirmedServiceChoice.ConfirmedCOVNotification:
                    return Tags.EncodeBytes((ConfirmedCOVNotificationRequest)request);
                case ConfirmedServiceChoice.ConfirmedEventNotification:
                    return Tags.EncodeBytes((ConfirmedEventNotificationRequest)request);
                case ConfirmedServiceChoice.ConfirmedPrivateTransfer:
                    return Tags.EncodeBytes((ConfirmedPrivateTransferRequest)request);
                case ConfirmedServiceChoice.ConfirmedTextMessage:
                    return Tags.EncodeBytes((ConfirmedTextMessageRequest)request);
                case ConfirmedServiceChoice.CreateObject:
                    return Tags.EncodeBytes((CreateObjectRequest)request);
                case ConfirmedServiceChoice.DeleteObject:
                    return Tags.EncodeBytes((DeleteObjectRequest)request);
                case ConfirmedServiceChoice.DeviceCommunicationControl:
                    return Tags.EncodeBytes((DeviceCommunicationControlRequest)request);
                case ConfirmedServiceChoice.GetAlarmSummary:
                    return Tags.EncodeBytes((GetAlarmSummaryRequest)request);
                case ConfirmedServiceChoice.GetEnrollmentSummary:
                    return Tags.EncodeBytes((GetEnrollmentSummaryRequest)request);
                case ConfirmedServiceChoice.GetEventInformation:
                    return Tags.EncodeBytes((GetEventInformationRequest)request);
                case ConfirmedServiceChoice.LifeSafetyOperation:
                    return Tags.EncodeBytes((LifeSafetyOperationRequest)request);
                case ConfirmedServiceChoice.ReadProperty:
                    return Tags.EncodeBytes((ReadPropertyRequest)request);
                case ConfirmedServiceChoice.ReadPropertyConditional:
                    return Tags.EncodeBytes((ReadPropertyConditionalRequest)request);
                case ConfirmedServiceChoice.ReadPropertyMultiple:
                    return Tags.EncodeBytes((ReadPropertyMultipleRequest)request);
                case ConfirmedServiceChoice.ReadRange:
                    return Tags.EncodeBytes((ReadRangeRequest)request);
                case ConfirmedServiceChoice.ReinitializeDevice:
                    return Tags.EncodeBytes((ReinitializeDeviceRequest)request);
                case ConfirmedServiceChoice.RemoveListElement:
                    return Tags.EncodeBytes((RemoveListElementRequest)request);
                case ConfirmedServiceChoice.RequestKey:
                    return Tags.EncodeBytes((RequestKeyRequest)request);
                case ConfirmedServiceChoice.SubscribeCOV:
                    return Tags.EncodeBytes((SubscribeCOVRequest)request);
                case ConfirmedServiceChoice.SubscribeCOVProperty:
                    return Tags.EncodeBytes((SubscribeCOVPropertyRequest)request);
                case ConfirmedServiceChoice.VtClose:
                    return Tags.EncodeBytes((VtCloseRequest)request);
                case ConfirmedServiceChoice.VtData:
                    return Tags.EncodeBytes((VtDataRequest)request);
                case ConfirmedServiceChoice.VtOpen:
                    return Tags.EncodeBytes((VtOpenRequest)request);
                case ConfirmedServiceChoice.WriteProperty:
                    return Tags.EncodeBytes((WritePropertyRequest)request);
                case ConfirmedServiceChoice.WritePropertyMultiple:
                    return Tags.EncodeBytes((WritePropertyMultipleRequest)request);
                default:
                    throw new RejectException(RejectReason.UnrecognizedService);
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

            lock(_lock)
            {
                _devices.Upsert(entry);
            }

            _deviceSearches.ResultFound(
                Recipient.NewDevice(request.IAmDeviceIdentifier),
                entry);

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
            UnconfirmedRequestMessage message = new UnconfirmedRequestMessage();
            message.ServiceChoice = (byte)request.ServiceChoice;
            byte[] raw = _saveUnconfirmedRequest(request);

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

                if (_deviceSearches != null)
                {
                    _deviceSearches.Dispose();
                    _deviceSearches = null;
                }
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
        /// Searches for a device (Sends a who-is request)
        /// </summary>
        /// <param name="instance">The recipient of the device to search for</param>
        /// <param name="callback">The callback to invoke when the device is found</param>
        public void SearchForDevice(Recipient recipient, ISearchCallback<Recipient, DeviceTableEntry> callback)
        {
            DeviceTableEntry device = null;

            lock(_lock)
            {
                device = _devices.Get(recipient);
            }

            if (device != null)
                callback.OnFound(device);
            else
                _deviceSearches.Search(recipient, callback);
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
        /// <param name="handle">The handle used to control the transaction</param>
        /// <param name="address">The address of the destination device</param>
        /// <param name="request">The request to send</param>
        public void SendConfirmedRequest(ClientTransactionHandle handle, Address address, IConfirmedRequest request)
        {
            lock(_lock)
            {
                var bytes = _saveConfirmedRequest(request);
                _txManager.SendConfirmedRequest(handle, address, (byte)request.ServiceChoice, bytes);       
            }
        }

        /// <summary>
        /// Sends a confirmed request
        /// </summary>
        /// <param name="handle">The handle used to control the transaction</param>
        /// <param name="instance">The address of the destination device</param>
        /// <param name="request">The request to send</param>
        public void SendConfirmedRequest(ClientTransactionHandle handle, uint instance, IConfirmedRequest request)
        {
            lock(_lock)
            {
                var bytes = _saveConfirmedRequest(request);
                _txManager.SendConfirmedRequest(handle, instance, (byte)request.ServiceChoice, bytes);
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
        /// Performs a search for a device on the network
        /// </summary>
        /// <param name="key">The device to search for</param>
        void ISearchHandler<Recipient>.DoSearch(Recipient key)
        {
            switch (key.Tag)
            {
                case Recipient.Tags.Address:
                    _sendWhoIsForAddress(new Address(key.AsAddress));
                    break;
                case Recipient.Tags.Device:
                    _sendWhoIsForInstance(key.AsDevice.Instance);
                    break;
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
                request = _processMessage(value, message, buffer, offset, end);

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
