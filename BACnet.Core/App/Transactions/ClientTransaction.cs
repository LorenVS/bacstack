using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using BACnet.Ashrae;
using BACnet.Core.App.Messages;
using BACnet.Core.Network;

namespace BACnet.Core.App.Transactions
{
    public class ClientTransaction : IDisposable, ISearchCallback<Recipient, DeviceTableEntry>
    {
        /// <summary>
        /// The time to wait fro a response to the transaction
        /// </summary>
        public static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(5);

        /// <summary>
        /// The time to wait for the next segment
        /// </summary>
        public static readonly TimeSpan SegmentTimeout = TimeSpan.FromSeconds(8);

        /// <summary>
        /// The maximum window size
        /// </summary>
        public const int MaxWindowSize = 8;

        /// <summary>
        /// The number of tries to retry the transaction
        /// </summary>
        public const int RequestAttempts = 2;

        /// <summary>
        /// Lock synchronizing access to the transaction
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// The host that initiated the transaction
        /// </summary>
        private readonly Host _host;

        /// <summary>
        /// The transaction manager
        /// </summary>
        private readonly TransactionManager _manager;

        /// <summary>
        /// The invocation id for this transaction
        /// </summary>
        private readonly byte _invokeId;

        /// <summary>
        /// The service choice of the request
        /// </summary>
        private readonly byte _serviceChoice;

        /// <summary>
        /// The content of the request
        /// </summary>
        private readonly byte[] _request;

        /// <summary>
        /// The current state of the transaction
        /// </summary>
        private ClientState _state;

        /// <summary>
        /// The remote device
        /// </summary>
        private DeviceTableEntry _device;

        /// <summary>
        /// The handle used to control this transaction and
        /// process its response
        /// </summary>
        private ClientTransactionHandle _handle;

        /// <summary>
        /// The current attempt
        /// </summary>
        private int _requestAttempt;

        /// <summary>
        /// Timer used when waiting for the request response
        /// </summary>
        private Timer _requestTimer;

        /// <summary>
        /// Timer used when waiting for the next segment
        /// </summary>
        private Timer _segmentTimer;

        /// <summary>
        /// The sequence number at the beginning of the current window
        /// </summary>
        private int _windowStart;

        /// <summary>
        /// The sequence number of the next expected segment
        /// </summary>
        private int _sequenceNumber;

        /// <summary>
        /// The number of segments that are transmitted at once
        /// </summary>
        private int _windowSize;

        /// <summary>
        /// Constructs a new ClientTransaction instance
        /// </summary>
        /// <param name="host">The host that initiated the transaction</param>
        /// <param name="manager">The transaction manager</param>
        /// <param name="handle">The handle used to control this transaction and process its response</param>
        /// <param name="invokeId">The invocation id for the transaction</param>
        /// <param name="destination">The destination device</param>
        /// <param name="serviceChoice">The service choice of the request</param>
        /// <param name="request">The request content</param>
        public ClientTransaction(Host host, TransactionManager manager, ClientTransactionHandle handle, byte invokeId, Recipient destination, byte serviceChoice, byte[] request)
        {
            this._host = host;
            this._manager = manager;
            this._handle = handle;
            this._invokeId = invokeId;
            this._serviceChoice = serviceChoice;
            this._request = request;
            this._state = ClientState.GetDeviceInfo;
            this._handle.SetTransaction(this);
            host.SearchForDevice(destination, this);
        }
        
        /// <summary>
        /// Disposes of all resources held by the transaction
        /// </summary>
        private void _disposeAll()
        {
            _stopRequestTimer();
            _stopSegmentTimer();
        }

        /// <summary>
        /// Disposes of the transaction
        /// </summary>
        public void Dispose()
        {
            lock (_lock)
            {
                _disposeAll();
            }
        }

        /// <summary>
        /// Determines whether this transaction matches a supplied
        /// remote address and invoke id pair
        /// </summary>
        /// <param name="address">The remote address</param>
        /// <param name="invokeId">The invoke id</param>
        /// <returns>True if the transaction matches, false otherwise</returns>
        public bool Matches(Address address, byte invokeId)
        {
            var entry = this._device;
            if (entry == null)
                return false;
            return entry.Address == address && _invokeId == invokeId;
        }

        /// <summary>
        /// Determines whether the supplied sequence number
        /// is within the current window
        /// </summary>
        /// <param name="sequenceNumber">The sequence number</param>
        /// <returns>The full sequence number, or -1 if it is outside of the window</returns>
        private int _inWindow(byte sequenceNumber)
        {
            int full = _windowStart - (_windowStart % 256) + sequenceNumber;
            if (full >= _windowStart && full < _windowStart + _windowSize)
                return full;
            else
                return -1;            
        }

        /// <summary>
        /// Sends the very first segment in the request
        /// </summary>
        private void _sendInitialRequestSegment()
        {
            if (_request.Length > _device.MaxAppgramLength)
                throw new Exception("Segmentation not supported... yet");

            _sequenceNumber = 0;
            _windowStart = 0;
            _windowSize = MaxWindowSize;

            ConfirmedRequestMessage message = new ConfirmedRequestMessage();
            message.Segmented = false;
            message.MoreFollows = false;
            message.SegmentedResponseAccepted = true;
            message.MaxSegmentsAccepted = int.MaxValue;
            message.MaxAppgramLengthAccepted = (int)_device.MaxAppgramLength; // TODO: retrieve from local device?
            message.InvokeId = _invokeId;
            message.SequenceNumber = (byte)_windowStart;
            message.ProposedWindowSize = (byte)_windowSize;
            message.ServiceChoice = _serviceChoice;

            _host.SendRaw(_device.Address, true, message,
                new BufferSegment(_request, 0, _request.Length));

            _transitionTo(ClientState.AwaitConfirmation);
        }

        /// <summary>
        /// Sends a segment ack
        /// </summary>
        private void _sendSegmentAck(bool nack = false)
        {
            if(_sequenceNumber > _windowStart)
                _windowStart = _sequenceNumber;

            SegmentAckMessage message = new SegmentAckMessage();
            message.ActualWindowSize = (byte)_windowSize;
            message.InvokeId = _invokeId;
            message.Nack = nack;
            message.SequenceNumber = (byte)(_windowStart - 1);
            message.Server = false;

            _host.SendRaw(_device.Address, true, message,
                BufferSegment.Empty);
        }

        /// <summary>
        /// Sends an abort message
        /// </summary>
        /// <param name="reason">The abort reason</param>
        private void _sendAbort(AbortReason reason)
        {
            AbortMessage message = new AbortMessage();
            message.AbortReason = (byte)reason;
            message.InvokeId = _invokeId;
            message.Server = false;

            _host.SendRaw(_device.Address, true, message,
                BufferSegment.Empty);
        }

        /// <summary>
        /// Called whenever the request timer ticks
        /// </summary>
        private void _requestTick(object state)
        {
            lock(_lock)
            {
                if (_state == ClientState.AwaitConfirmation)
                {
                    _requestAttempt++;

                    if (_requestAttempt >= RequestAttempts)
                    {
                        // TODO: should probably be a timeout error instead of an abort
                        _handle.FeedAbort(AbortReason.Other);
                        _transitionTo(ClientState.Disposed);
                    }
                    else
                    {
                        _sendInitialRequestSegment();
                    }
                }
            }
        }

        /// <summary>
        /// Called whenever the segment timer ticks
        /// </summary>
        /// <param name="state"></param>
        private void _segmentTick(object state)
        {
            lock(_lock)
            {
                if(_state == ClientState.SegmentedConfirmation)
                {
                    _handle.FeedAbort(AbortReason.Other);
                    _transitionTo(ClientState.Disposed);
                }
            }
        }

        /// <summary>
        /// Disposes of the request timer
        /// </summary>
        private void _stopRequestTimer()
        {
            if(_requestTimer != null)
            {
                _requestTimer.Dispose();
                _requestTimer = null;
            }
        }

        /// <summary>
        /// Starts the request timer
        /// </summary>
        private void _startRequestTimer()
        {
            if(_requestTimer != null)
            {
                _requestTimer.Dispose();
                _requestTimer = null;
            }

            _requestTimer = new Timer(
                _requestTick,
                null,
                RequestTimeout,
                RequestTimeout);
        }

        /// <summary>
        /// Disposes of the segment timer
        /// </summary>
        private void _stopSegmentTimer()
        {
            if(_segmentTimer != null)
            {
                _segmentTimer.Dispose();
                _segmentTimer = null;
            }
        }

        /// <summary>
        /// Starts the segment timer
        /// </summary>
        private void _startSegmentTimer()
        {
            if (_segmentTimer == null)
            {
                _segmentTimer = new Timer(
                    _segmentTick,
                    null,
                    SegmentTimeout,
                    TimeSpan.FromMilliseconds(-1));
            }
            else
            {
                _segmentTimer.Change(
                    SegmentTimeout,
                    TimeSpan.FromMilliseconds(-1));
            }
        }

        /// <summary>
        /// Performs necessary transitions operations
        /// to move to the supplied state
        /// </summary>
        /// <param name="state">The state to transition to</param>
        private void _transitionTo(ClientState state)
        {
            _stopRequestTimer();
            if(state != ClientState.SegmentedConfirmation)
            {
                // we re-use the same timer instance if we are
                // going back to the segmented confirmation state
                _stopSegmentTimer();
            }

            if(state == ClientState.AwaitConfirmation)
            {
                _startRequestTimer();
            }
            else if(state == ClientState.SegmentedConfirmation)
            {
                _startSegmentTimer();
            }
            else if(state == ClientState.Disposed)
            {
                _manager.DisposeTransaction(this);
            }

            _state = state;
        }

        /// <summary>
        /// Called whenever a simple ack is received
        /// for this transaction
        /// </summary>
        /// <param name="message">The received message</param>
        public void OnSimpleAck(SimpleAckMessage message)
        {
            lock(_lock)
            {
                if (_state == ClientState.AwaitConfirmation)
                {
                    _handle.FeedSimpleAck();
                    _manager.DisposeTransaction(this);
                }
            }
        }

        /// <summary>
        /// Called whenever a complex ack is received
        /// for this transaction
        /// </summary>
        /// <param name="message">The received message</param>
        /// <param name="segment">The segment</param>
        public void OnComplexAck(ComplexAckMessage message, BufferSegment segment)
        {
            lock(_lock)
            {
                bool dispose = false;

                if (_state == ClientState.AwaitConfirmation)
                {
                    if(!message.Segmented)
                    {
                        _handle.FeedComplexAck(message, segment);
                        dispose = true;
                    }
                    else if(message.SequenceNumber == 0)
                    {
                        _sequenceNumber = 1;
                        _windowSize = message.ProposedWindowSize;
                        _sendSegmentAck();
                        _handle.FeedComplexAck(message, segment);
                        _transitionTo(ClientState.SegmentedConfirmation);
                    }
                    else
                    {
                        _sendAbort(AbortReason.InvalidApduInThisState);
                        _handle.FeedAbort(AbortReason.InvalidApduInThisState);
                        dispose = true;
                    }
                }
                else if(_state == ClientState.SegmentedConfirmation)
                {
                    int sequenceNumber;

                    if ((sequenceNumber = _inWindow(message.SequenceNumber)) != -1
                        && sequenceNumber == _sequenceNumber)
                    {
                        _handle.FeedComplexAck(message, segment);
                        _windowSize = message.ProposedWindowSize;
                        _sequenceNumber++;

                        dispose = !message.MoreFollows;
                        if (dispose || _sequenceNumber == _windowStart + _windowSize)
                            _sendSegmentAck();
                        else if (!dispose)
                            _transitionTo(ClientState.SegmentedConfirmation);
                    }
                    else
                    {
                        _sendSegmentAck(nack:true);
                        _transitionTo(ClientState.SegmentedConfirmation);
                    }
                }

                if(dispose)
                    _transitionTo(ClientState.Disposed);
            }
        }

        /// <summary>
        /// Called whenever a segment ack is received
        /// for this transaction
        /// </summary>
        /// <param name="message">The segment ack message</param>
        public void OnSegmentAck(SegmentAckMessage message)
        {
        }

        /// <summary>
        /// Called whenever an error message is received
        /// for this transaction
        /// </summary>
        /// <param name="message">The error message</param>
        public void OnError(ErrorMessage message)
        {
            lock(_lock)
            {
                if (_state == ClientState.AwaitConfirmation)
                {
                    _handle.FeedError(message.Error);
                    _transitionTo(ClientState.Disposed);
                }
            }
        }

        /// <summary>
        /// Called whenever a reject message is
        /// received for this transaction
        /// </summary>
        /// <param name="message">The reject message</param>
        public void OnReject(RejectMessage message)
        {
            lock(_lock)
            {
                if (_state == ClientState.AwaitConfirmation)
                {
                    _handle.FeedReject((RejectReason)message.RejectReason);
                    _transitionTo(ClientState.Disposed);
                }
            }
        }

        /// <summary>
        /// Called whenever an abort message is
        /// received for this transaction
        /// </summary>
        /// <param name="message">The abort message</param>
        public void OnAbort(AbortMessage message)
        {
            lock(_lock)
            {
                _handle.FeedAbort((AbortReason)message.AbortReason);
                _transitionTo(ClientState.Disposed);
            }
        }

        /// <summary>
        /// Aborts the transaction
        /// </summary>
        public void Abort(AbortReason reason)
        {
            lock(_lock)
            {
                if(_state != ClientState.GetDeviceInfo)
                    _sendAbort(reason);

                _handle.FeedAbort(reason);
                _transitionTo(ClientState.Disposed);
            }
        }

        /// <summary>
        /// Called when the device search completes, and results
        /// in a device table entry
        /// </summary>
        /// <param name="entry">The entry that was found</param>
        void ISearchCallback<Recipient, DeviceTableEntry>.OnFound(DeviceTableEntry entry)
        {
            lock(_lock)
            {
                if(_state == ClientState.GetDeviceInfo)
                {
                    _device = entry;
                    this._sendInitialRequestSegment();
                }
                else
                {
                    _handle.FeedAbort(AbortReason.Other);
                    _transitionTo(ClientState.Disposed);
                }
            }
        }

        /// <summary>
        /// Called when a device search times out
        /// </summary>
        void ISearchCallback<Recipient, DeviceTableEntry>.OnTimeout(Recipient recipient)
        {
            lock(_lock)
            {
                _handle.FeedAbort(AbortReason.Other);
                _transitionTo(ClientState.Disposed);
            }
        }

    }
}