using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BACnet.Ashrae;
using BACnet.Core;
using BACnet.Core.App;
using BACnet.Core.Network;
using BACnet.Types;

namespace BACnet.Client
{
    public class DeviceFinder : IProcess, IObserver<InboundUnconfirmedRequest>, IObservable<DeviceTableEntry>
    {
#region Constants

        /// <summary>
        /// The minimum time between sending two subsequent Who-Is requests
        /// </summary>
        public static readonly TimeSpan DeviceSearchInterval = TimeSpan.FromSeconds(2);

        /// <summary>
        /// The time that should be allowed between subsequent searches of the same
        /// range, if the range has not changed
        /// </summary>
        public static readonly TimeSpan RangeSearchInterval = TimeSpan.FromSeconds(300);

        /// <summary>
        /// The maximum number of times a device range will be searched if
        /// no devices are replying within that range
        /// </summary>
        public const int RangeAttemptCutoff = 3;

#endregion

        /// <summary>
        /// Lock synchronizing access to the device finder
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// Whether or not the device finder process is disposed
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// The options for the process
        /// </summary>
        private DeviceFinderOptions _options;

        /// <summary>
        /// The BACnet host process
        /// </summary>
        private Host _host;
        
        /// <summary>
        /// Timer used to send out who-is requests to search for devices
        /// </summary>
        private Timer _searchTimer;

        /// <summary>
        /// The subscription to unconfirmed requests
        /// </summary>
        private IDisposable _unconfirmedRequestSubscription;

        /// <summary>
        /// The ranges of devices that have either been found or not
        /// </summary>
        private readonly LinkedList<DeviceRange> _ranges;

        /// <summary>
        /// The last range for which a who-is was sent
        /// </summary>
        private LinkedListNode<DeviceRange> _lastRange;

        /// <summary>
        /// The registered observers
        /// </summary>
        private SubscriptionList<DeviceTableEntry> _observers;

        /// <summary>
        /// The process if of the device finder process
        /// </summary>
        public int ProcessId { get { return _options.ProcessId; } }

        /// <summary>
        /// Creates a new device finder instance
        /// </summary>
        public DeviceFinder(DeviceFinderOptions options)
        {
            this._options = options.Clone();
            this._ranges = new LinkedList<DeviceRange>();
            this._ranges.AddLast(new DeviceRange(false, 1, ObjectId.MaxInstance));
            this._observers = new SubscriptionList<DeviceTableEntry>();
        }

        /// <summary>
        /// Resolves the process dependencies for this process
        /// </summary>
        /// <param name="processes">The available processes</param>
        public void Resolve(IEnumerable<IProcess> processes)
        {
            lock(_lock)
            {
                if (_disposed)
                    return;

                var host = processes.OfType<Host>().FirstOrDefault();
                if (host != null)
                    _setHost(host);
            }
        }

        /// <summary>
        /// Disposes of the device finder process
        /// </summary>
        public void Dispose()
        {
            dispose(true);
        }

        /// <summary>
        /// Disposes of the process
        /// </summary>
        /// <param name="disposing">True if managed resources should be disposed</param>
        protected virtual void dispose(bool disposing)
        {
            if (!_disposed)
            {
                try
                {
                    if(disposing)
                    {
                        _disposeHostDependants();

                        if (_observers != null)
                        {
                            _observers.Dispose();
                            _observers = null;
                        }
                    }
                }
                finally
                {
                    _disposed = true;
                }
            }
        }

        /// <summary>
        /// Sets the host used by the network database
        /// </summary>
        /// <param name="host">The host to use</param>
        private void _setHost(Host host)
        {
            _host = host;
            _searchTimer = new Timer(_searchTick, null, DeviceSearchInterval, DeviceSearchInterval);
            _unconfirmedRequestSubscription = _host.Subscribe(this);
        }

        /// <summary>
        /// Disposes of any resources that are dependent
        /// upon the Host object
        /// </summary>
        private void _disposeHostDependants()
        {
            if(_searchTimer != null)
            {
                _searchTimer.Dispose();
                _searchTimer = null;
            }

            _host = null;
        }


        /// <summary>
        /// Returns the maximum of two datetimes
        /// </summary>
        /// <param name="d1">The first datetime</param>
        /// <param name="d2">The second datetime</param>
        /// <returns>The maximum of the two</returns>
        private static DateTime dtMax(DateTime d1, DateTime d2)
        {
            return (d1 > d2) ? d1 : d2;
        }

        /// <summary>
        /// Tries to merge ranges in between two nodes
        /// </summary>
        /// <param name="first">The first node</param>
        /// <param name="second">The second node</param>
        private void _mergeRanges(LinkedListNode<DeviceRange> first, LinkedListNode<DeviceRange> second)
        {
            if (first == null)
                first = _ranges.First;
            if (second == null)
                second = _ranges.Last;
            var temp = first;

            while (temp != second && temp.Next != null)
            {
                var range1 = temp.Value;
                var range2 = temp.Next.Value;

                if (range1.End == range2.Start - 1 && range1.Found == range2.Found)
                {
                    // we can only merge if the two ranges are right next to each
                    // other (which they always should be), and if they have the
                    // same Found value
                    temp.Value = range1.WithEnd(range2.End);
                    _ranges.Remove(temp.Next);
                }
                else
                {
                    temp = temp.Next;
                }
            }
        }

        /// <summary>
        /// Marks a device as being found, performing
        /// the necessary operations on the device ranges to keep them consistent
        /// </summary>
        /// <param name="instance">The instance of the device</param>
        /// <param name="found">True if the device was found, false otherwise</param>
        private void _markDeviceAsFound(uint instance, bool found = true)
        {
            LinkedListNode<DeviceRange> node = null;

            for (var it = _ranges.First; it != null;)
            {
                if (it.Value.Start <= instance && it.Value.End >= instance)
                {
                    node = it;
                    break;
                }
                else
                {
                    it = it.Next;
                }
            }

            // the device in the range already has the appropriate Found value
            if (node.Value.Found == found)
                return;

            var range = node.Value;
            var previous = node.Previous;
            var next = node.Next;

            if (node.Value.Start == instance && node.Value.End == instance)
            {
                // only device in the range, we replace the whole range
                node.Value = range.WithFound(found);
            }
            else if (node.Value.Start == instance)
            {
                // this device is at the start of the range
                _ranges.AddBefore(node,
                    new DeviceRange(
                        found,
                        instance,
                        instance
                    ));

                node.Value = range.WithStart(instance + 1);
            }
            else if (node.Value.End == instance)
            {
                // this device is at the end of the range
                _ranges.AddAfter(node,
                    new DeviceRange(
                        found,
                        instance,
                        instance));

                node.Value = range.WithEnd(instance - 1);
            }
            else
            {
                // this device is in the middle of the range
                _ranges.AddBefore(node,
                    new DeviceRange(
                        range.Found,
                        range.Start,
                        instance - 1));

                _ranges.AddBefore(node,
                    new DeviceRange(
                        found,
                        instance,
                        instance));

                node.Value = node.Value.WithStart(instance + 1);
            }

            _mergeRanges(previous, next);

        }

        /// <summary>
        /// Subscribes an observer to be notified whenever a device table
        /// entry is found.
        /// </summary>
        /// <remarks>The observer may be notified multiple times of the same device</remarks>
        /// <param name="observer">The observer to subscribe</param>
        /// <returns>A disposable instance to unsubcribe</returns>
        public IDisposable Subscribe(IObserver<DeviceTableEntry> observer)
        {
            _observers.Register(observer);
            return new DeviceSubscription(this, observer);
        }

        /// <summary>
        /// Unsubscribes an observer
        /// </summary>
        /// <param name="observer">The observer to unsubscribe</param>
        private void _unsubscribe(IObserver<DeviceTableEntry> observer)
        {
            _observers.Unregister(observer);
        }

        /// <summary>
        /// Called when the search timer ticks, and the
        /// next Who-Is request should be sent
        /// </summary>
        /// <param name="state"></param>
        private void _searchTick(object state)
        {
            DeviceRange range = null;

            lock (_lock)
            {
                // get the next range we should send
                // a who-is request for

                var node = _lastRange;
                if (node == null)
                    node = _ranges.First;
                else
                    node = node.Next;

                DateTime maxLastSearch = DateTime.UtcNow.Add(RangeSearchInterval.Negate());

                while (node != null && (node.Value.Found || node.Value.LastSearch > maxLastSearch))
                    node = node.Next;

                if (node != null)
                {
                    range = node.Value;
                    range.Attempts++;
                    range.LastSearch = DateTime.UtcNow;
                    _lastRange = node;
                }
            }

            if (range != null)
            {
                WhoIsRequest request = new WhoIsRequest(
                        range.Start,
                        range.End);

                _host.SendUnconfirmedRequest(Address.GlobalBroadcast, true, request);
            }
        }

        /// <summary>
        /// Called when the next unconfirmed request is received
        /// </summary>
        /// <param name="value">The unconfirmed request</param>
        void IObserver<InboundUnconfirmedRequest>.OnNext(InboundUnconfirmedRequest value)
        {
            if (value.Request.ServiceChoice == UnconfirmedServiceChoice.IAm && value.Source != null)
            {
                lock (_lock)
                {
                    _markDeviceAsFound(value.Source.Instance);
                }

                _observers.Next(value.Source);
            }
        }

        /// <summary>
        /// Called when the host experiences an error
        /// </summary>
        /// <param name="error">The error</param>
        void IObserver<InboundUnconfirmedRequest>.OnError(Exception error)
        {
        }

        /// <summary>
        /// Called when the host is no longer providing information
        /// about received unconfirmed requests
        /// </summary>
        void IObserver<InboundUnconfirmedRequest>.OnCompleted()
        {
            lock (_lock)
            {
                _disposeHostDependants();
            }
        }

#region Nested Types

        /// <summary>
        /// A range of devices that have either
        /// been found (received IAm's recently) or not found
        /// </summary>
        private class DeviceRange
        {
            /// <summary>
            /// True if the devices have been found, false otherwise
            /// </summary>
            public bool Found { get; private set; }

            /// <summary>
            /// The start of the range
            /// </summary>
            public uint Start { get; private set; }

            /// <summary>
            /// The end of the range
            /// </summary>
            public uint End { get; private set; }

            /// <summary>
            /// The last time a who-is request was sent for this range
            /// </summary>
            public DateTime LastSearch { get; set; }

            /// <summary>
            /// The number of times a who-is has been sent for this range
            /// </summary>
            public int Attempts { get; set; }

            /// <summary>
            /// Constructs a new device range instance
            /// </summary>
            /// <param name="found">True if the devices have been found, false otherwise</param>
            /// <param name="start">The start of the range</param>
            /// <param name="end">The end of the range</param>
            public DeviceRange(bool found, uint start, uint end, DateTime lastSearch = default(DateTime), int attempts = 0)
            {
                this.Found = found;
                this.Start = start;
                this.End = end;
                this.LastSearch = lastSearch;
                this.Attempts = attempts;
            }

            /// <summary>
            /// Creates an identical device range object with the
            /// supplied Found value
            /// </summary>
            /// <param name="found"></param>
            /// <returns></returns>
            public DeviceRange WithFound(bool found)
            {
                return new DeviceRange(
                    found,
                    this.Start,
                    this.End,
                    DateTime.MinValue,
                    0);
            }

            /// <summary>
            /// Creates an identical device range object with
            /// the supplied start value
            /// </summary>
            /// <param name="start">The start value for the range</param>
            /// <returns>The new range</returns>
            public DeviceRange WithStart(uint start)
            {
                return new DeviceRange(
                    this.Found,
                    start,
                    this.End,
                    DateTime.MinValue,
                    0);
            }

            /// <summary>
            /// Creates an identical device range object with
            /// the supplied end value
            /// </summary>
            /// <param name="end">The end value for the range</param>
            /// <returns>The new range</returns>
            public DeviceRange WithEnd(uint end)
            {
                return new DeviceRange(
                    this.Found,
                    this.Start,
                    end,
                    DateTime.MinValue,
                    0);
            }
        }

        private class DeviceSubscription : IDisposable
        {
            /// <summary>
            /// The device finder that owns the subscription
            /// </summary>
            private DeviceFinder _finder;

            /// <summary>
            /// The observer that is subscribed
            /// </summary>
            private IObserver<DeviceTableEntry> _observer;

            public DeviceSubscription(DeviceFinder finder, IObserver<DeviceTableEntry> observer)
            {
                this._finder = finder;
                this._observer = observer;
            }

            /// <summary>
            /// Disposes of the subscription, which unsubscribes
            /// the observer from the device finder
            /// </summary>
            public void Dispose()
            {
                _finder._unsubscribe(this._observer);
            }
        }

#endregion

    }
}
