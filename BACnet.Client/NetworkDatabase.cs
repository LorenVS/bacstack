using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Ashrae.Objects;
using BACnet.Core;
using BACnet.Core.App;
using BACnet.Core.Exceptions;
using BACnet.Core.Network;
using BACnet.Types;
using BACnet.Client.Descriptors;
using BACnet.Client.Db;

namespace BACnet.Client
{
    /// <summary>
    /// A database object that can be used
    /// to automatically load common information
    /// from devices on the network, exposing it
    /// to an application that needs it
    /// </summary>
    public class NetworkDatabase : IObserver<InboundUnconfirmedRequest>, IProcess
    {
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
        /// The number of seconds between queries to fill the refresh queue
        /// </summary>
        public static readonly TimeSpan FillRefreshQueueInterval = TimeSpan.FromSeconds(10);

        /// <summary>
        /// The minimum time between refresh attempts for the same object
        /// </summary>
        public static readonly TimeSpan MinTimeBetweenRefreshAttempts = TimeSpan.FromMinutes(5);

        /// <summary>
        /// The interval at which objects should be refreshed
        /// </summary>
        public static readonly TimeSpan RefreshInterval = TimeSpan.FromDays(7);

        /// <summary>
        /// The maximum number of queued refreshes to retrieve from the database at once
        /// </summary>
        public static readonly int GetQueuedRefreshesLimit = 200;

        /// <summary>
        /// The maximum number of times a device range will be searched if
        /// no devices are replying within that range
        /// </summary>
        public const int RangeAttemptCutoff = 3;

        /// <summary>
        /// The number of threads to use for updating properties
        /// </summary>
        public const int ReadThreadCount = 2;

        /// <summary>
        /// The number of objects to refresh at once
        /// </summary>
        public const int ObjectChunkSize = 5;

        /// <summary>
        /// Lock synchronizing access to the database
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// The processid of the network database
        /// </summary>
        public int ProcessId { get { return _options.ProcessId; } }

        /// <summary>
        /// The session which this process belongs to
        /// </summary>
        public Session Session { get; set; }

        /// <summary>
        /// The options instance for the network database
        /// </summary>
        private readonly NetworkDatabaseOptions _options;

        /// <summary>
        /// The host used to communicate with the network
        /// </summary>
        private Host _host;
        
        /// <summary>
        /// The client used to send requests
        /// </summary>
        private Client _client;

        /// <summary>
        /// The wrapper around the SQLite database
        /// </summary>
        private NetworkDb _db;
        
        /// <summary>
        /// Objects that are observing the network database
        /// </summary>
        private readonly List<DescriptorObserver> _observers;
        
        /// <summary>
        /// The queue of refresh tasks that need to be executed
        /// </summary>
        private C5.IPriorityQueue<IRefreshTask> _refreshQueue;

        /// <summary>
        /// Timer used to load the refresh tasks that need to be performed
        /// </summary>
        private Timer _fillRefreshQueueTimer;

        /// <summary>
        /// Threads used to load descriptor properties
        /// </summary>
        private Thread[] _refreshThreads;
        
        /// <summary>
        /// Semaphore to track the number of entries in the read queue
        /// </summary>
        private SemaphoreSlim _refreshQueueSem;

        /// <summary>
        /// Set whenever the database is disposing
        /// </summary>
        private ManualResetEventSlim _disposeHostEvent;

        /// <summary>
        /// The subscription to unconfirmed requests
        /// </summary>
        private IDisposable _unconfirmedRequestSubscription;

        /// <summary>
        /// Constructs a new network database instance
        /// </summary>
        /// <param name="options">The options for the network database</param>
        public NetworkDatabase(NetworkDatabaseOptions options)
        {
            this._options = options.Clone();
            this._db = new NetworkDb(this._options.DatabasePath, _options.DescriptorRegistrar);
            this._observers = new List<DescriptorObserver>();
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
        /// Starts the refresh thread, which reads
        /// descriptor properties
        /// </summary>
        private void _refreshThreadStart()
        {
            while(!_disposeHostEvent.Wait(TimeSpan.Zero))
            {
                IRefreshTask task = null;

                if (_refreshQueueSem.Wait(TimeSpan.FromSeconds(.1)))
                {
                    lock (_lock)
                    {
                        task = _refreshQueue.DeleteMin();
                    }
                }

                if (task != null)
                    task.Execute(this);
            }
        }

        /// <summary>
        /// Disposes of all resources held
        /// that are dependant on the host
        /// </summary>
        private void _disposeHostDependants()
        {
            if (_disposeHostEvent != null)
                _disposeHostEvent.Set();

            if(_unconfirmedRequestSubscription != null)
            {
                _unconfirmedRequestSubscription.Dispose();
                _unconfirmedRequestSubscription = null;
            }

            if(_fillRefreshQueueTimer != null)
            {
                _fillRefreshQueueTimer.Dispose();
                _fillRefreshQueueTimer = null;
            }

            if (_refreshThreads != null)
            {
                for (int i = 0; i < _refreshThreads.Length; i++)
                {
                    if (_refreshThreads[i] != null)
                    {
                        _refreshThreads[i].Join();
                        _refreshThreads[i] = null;
                    }
                }
                _refreshThreads = null;
            }


            if (_disposeHostEvent != null)
            {
                _disposeHostEvent.Dispose();
                _disposeHostEvent = null;
            }

            _host = null;
            _client = null;

        }

        /// <summary>
        /// Disposes of all resources
        /// held by the network database
        /// </summary>
        private void _disposeAll()
        {
            _disposeHostDependants();

            if(_db != null)
            {
                _db.Dispose();
                _db = null;
            }
        }
        
        /// <summary>
        /// Inserts a set of new objects
        /// </summary>
        /// <param name="vendorId">The vendor id of the objects</param>
        /// <param name="deviceInstance">The device instance of the objects</param>
        /// <param name="objectIds">The object ids of the objects</param>
        /// <returns>The number of inserted objects</returns>
        private int _insertObjects(ushort vendorId, uint deviceInstance, IEnumerable<ObjectId> objectIds)
        {
            var inserted = _db.UpsertObjects(vendorId, deviceInstance, objectIds);
            var infos = new List<ObjectInfo>(inserted.Count);
            for (int i = 0; i < inserted.Count; i++)
            {
                infos.Add(new ObjectInfo(vendorId, deviceInstance, inserted[i]));
            }

            if(inserted.Count > 0)
            {
                for(int i = 0; i < inserted.Count; i++)
                {
                    ObjectId[] ids = new ObjectId[Math.Min(ObjectChunkSize, inserted.Count - i)];
                    for(int j = 0; j < ids.Length; j++)
                    {
                        ids[j] = inserted[i + j];
                    }
                    i += ids.Length - 1;
                    _refreshQueue.Add(new RefreshObjectsTask(vendorId, deviceInstance, ids));
                    _refreshQueueSem.Release(1);
                }


                foreach(var observer in _observers)
                {
                    observer.Observer.Add(infos.Where(info => observer.Query.Matches(info)));
                }
            }

            return inserted.Count;
        }

        /// <summary>
        /// Inserts a new object record into the
        /// network database
        /// </summary>
        /// <param name="vendorId">The vendor id of the object</param>
        /// <param name="deviceInstance">The device instance</param>
        /// <param name="objectId">The object id of the object</param>
        /// <returns>True if the object was inserted, false otherwise</returns>
        private bool _insertObject(ushort vendorId, uint deviceInstance, ObjectId objectId)
        {
            ObjectInfo info = null;
            bool inserted = _db.UpsertObject(vendorId, deviceInstance, objectId);

            if(inserted)
            {
                info = new ObjectInfo(vendorId, deviceInstance, objectId);
                foreach (var observer in _observers)
                {
                    if (observer.Query.Matches(info))
                        observer.Observer.Add(info);
                }
            }

            return inserted;
        }

        /// <summary>
        /// Updates a descriptor object
        /// </summary>
        /// <param name="info">The descriptor object</param>
        private void _updateObject(ObjectInfo info)
        {
            foreach(var observer in _observers)
            {
                if (observer.Query.Matches(info))
                    observer.Observer.Update(info);
            }
        }

        /// <summary>
        /// Updates a set of descriptor objects
        /// </summary>
        /// <param name="infos">The descriptor objects</param>
        private void _updateObjects(ObjectInfo[] infos)
        {
            foreach(var observer in _observers)
            {
                observer.Observer.Update(infos.Where(info => observer.Query.Matches(info)));
            }
        }

        /// <summary>
        /// Called whenever the fillRefreshQueueTimer ticks, loading
        /// objects that need to be refreshed from the database
        /// </summary>
        /// <param name="state">The state, null</param>
        private void _fillRefreshQueue(object state)
        {
            var objects = _db.GetQueuedRefreshes(
                MinTimeBetweenRefreshAttempts,
                RefreshInterval,
                GetQueuedRefreshesLimit);

            lock(_lock)
            {
                ushort vendorId = 0;
                uint deviceInstance = 0;
                List<ObjectId> ids = new List<ObjectId>();

                for(int i = 0; i < objects.Count; i++)
                {
                    var obj = objects[i];
                    if(ids.Count >= ObjectChunkSize || obj.DeviceInstance != deviceInstance && ids.Count > 0)
                    {
                        var objectIds = ids.ToArray();
                        RefreshObjectsTask task = new RefreshObjectsTask(
                            vendorId,
                            deviceInstance,
                            objectIds);
                        _refreshQueue.Add(task);
                        _refreshQueueSem.Release(1);
                        ids.Clear();
                    }

                    deviceInstance = obj.DeviceInstance;
                    ids.Add(obj.ObjectIdentifier);
                }

                if (ids.Count > 0)
                {
                    var objectIds = ids.ToArray();
                    RefreshObjectsTask task = new RefreshObjectsTask(
                        vendorId,
                        deviceInstance,
                        objectIds);
                    _refreshQueue.Add(task);
                    _refreshQueueSem.Release(1);
                    ids.Clear();
                }
            }
        }

        /// <summary>
        /// Sets the host used by the network database
        /// </summary>
        /// <param name="host">The host to use</param>
        private void _setHost(Host host)
        {
            _refreshQueue = new C5.IntervalHeap<IRefreshTask>(new RefreshTaskComparer());
            _refreshQueueSem = new SemaphoreSlim(0);
            _disposeHostEvent = new ManualResetEventSlim(false);

            _host = host;
            _client = new Client(host);

            _unconfirmedRequestSubscription = _host.Subscribe(this);

            _fillRefreshQueueTimer = new Timer(_fillRefreshQueue, null, TimeSpan.Zero, FillRefreshQueueInterval);

            _refreshThreads = new Thread[ReadThreadCount];
            for (int i = 0; i < ReadThreadCount; i++)
            {
                _refreshThreads[i] = new Thread(_refreshThreadStart);
                _refreshThreads[i].Start();
            }
        }

        /// <summary>
        /// Disposes of a descriptor subscription, unsubscribing the observer
        /// </summary>
        /// <param name="observer">The observer to unsubscribe</param>
        private void _disposeDescriptorSubscription(IDescriptorObserver<ObjectInfo, GlobalObjectId> observer)
        {
            lock(_lock)
            {
                bool removed = false;

                for(int i = 0; i < _observers.Count; i++)
                {
                    if(_observers[i].Observer == observer)
                    {
                        _observers.RemoveAt(i);
                        removed = true;
                        break;
                    }
                }

                if(removed)
                    observer.Close();                
            }
        }
        
        /// <summary>
        /// Resolves the network database's dependencies
        /// </summary>
        /// <param name="processes">The processes</param>
        public void Resolve(IEnumerable<IProcess> processes)
        {
            lock(_lock)
            {
                _disposeHostDependants();

                var host = processes.OfType<Host>().FirstOrDefault();
                if (host != null)
                    _setHost(host);
            }
        }
        
        /// <summary>
        /// Disposes of the network database instance
        /// </summary>
        public void Dispose()
        {
            lock(_lock)
            {
                _disposeAll();
            }
        }

        /// <summary>
        /// Subscribes to the network database
        /// </summary>
        /// <param name="observer">The obsever to subscribe</param>
        /// <returns>The disposable subscription instance</returns>
        public IDisposable Subscribe(DescriptorQuery query, IDescriptorObserver<ObjectInfo, GlobalObjectId> observer)
        {
            DescriptorSubscription ret = null;

            lock(_lock)
            {
                this._observers.Add(new DescriptorObserver(query, observer));
                ret = new DescriptorSubscription(this, observer);
            }

            Task.Factory.StartNew(() =>
            {
                lock(_lock)
                {
                    var descriptors = _db.QueryObjects(query);
                    observer.Add(descriptors);
                }
            });

            return ret;
        }

        /// <summary>
        /// Called when the next unconfirmed request is received
        /// </summary>
        /// <param name="value">The unconfirmed request</param>
        void IObserver<InboundUnconfirmedRequest>.OnNext(InboundUnconfirmedRequest value)
        {
            if(value.Request.ServiceChoice == UnconfirmedServiceChoice.IAm && value.Source != null)
            {
                // we add devices to the database when we receive IAm requests from them
                lock(_lock)
                {
                    if(_insertObject(
                        value.Source.VendorId,
                        value.Source.Instance,
                        value.Source.ObjectIdentifier))
                    {
                        _refreshQueue.Add(new RefreshObjectTask(value.Source.Instance, value.Source.ObjectIdentifier));
                        _refreshQueue.Add(new RefreshObjectListTask(value.Source.VendorId, value.Source.Instance));
                        _refreshQueueSem.Release(2);
                    }
                }
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
            lock(_lock)
            {
                _disposeHostDependants();
            }
        }

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

        private struct DescriptorObserver
        {
            public DescriptorQuery Query { get; private set; }

            public IDescriptorObserver<ObjectInfo, GlobalObjectId> Observer { get; private set; }

            public DescriptorObserver(DescriptorQuery query, IDescriptorObserver<ObjectInfo, GlobalObjectId> observer)
                : this()
            {
                this.Query = query;
                this.Observer = observer;
            }
        }

        private class DescriptorSubscription : IDisposable
        {

            /// <summary>
            /// The network database
            /// </summary>
            private NetworkDatabase _database;

            /// <summary>
            /// The observer
            /// </summary>
            private IDescriptorObserver<ObjectInfo, GlobalObjectId> _observer;

            /// <summary>
            /// Constructs a new descriptor subscription instance
            /// </summary>
            /// <param name="database">The network database</param>
            /// <param name="observer">The observer</param>
            public DescriptorSubscription(NetworkDatabase database, IDescriptorObserver<ObjectInfo, GlobalObjectId> observer)
            {
                this._database = database;
                this._observer = observer;
            }

            /// <summary>
            /// Disposes of the subscription
            /// </summary>
            public void Dispose()
            {
                _database._disposeDescriptorSubscription(_observer);
            }

        }

        /// <summary>
        /// A refresh task that needs to be performed
        /// </summary>
        private interface IRefreshTask
        {
            /// <summary>
            /// The priority of the task
            /// </summary>
            int Priority { get; }

            /// <summary>
            /// Executes the tasks
            /// </summary>
            void Execute(NetworkDatabase db);
        }

        private class RefreshTaskComparer : IComparer<IRefreshTask>
        {
            public int Compare(IRefreshTask x, IRefreshTask y)
            {
                if (y == null)
                    return 1;
                return x.Priority.CompareTo(y.Priority);
            }
        }

        private class RefreshObjectListTask : IRefreshTask
        {
            /// <summary>
            /// The priority of the task
            /// </summary>
            public int Priority { get { return 2; } }

            /// <summary>
            /// The vendor id of the device
            /// </summary>
            public ushort VendorId { get; private set; }

            /// <summary>
            /// The device instance of the device
            /// </summary>
            public uint DeviceInstance { get; private set; }

            /// <summary>
            /// Constructs a new refresh object list task instance
            /// </summary>
            /// <param name="vendorId">The vendor id of the device</param>
            /// <param name="deviceInstance">The device instance of the device</param>
            public RefreshObjectListTask(ushort vendorId, uint deviceInstance)
            {
                this.VendorId = vendorId;
                this.DeviceInstance = deviceInstance;
            }

            /// <summary>
            /// Executes the task
            /// </summary>
            /// <param name="db">The database that is executing the task</param>
            public void Execute(NetworkDatabase db)
            {
                var client = new Client(db._host);
                try
                {
                    var objectList = client.With(DeviceInstance)
                        .ReadProperty(dev => dev.ObjectList);
                    db._insertObjects(VendorId, DeviceInstance, objectList);
                }
                catch(AggregateException)
                {

                }
            }
        }

        private class RefreshObjectTask : IRefreshTask
        {
            /// <summary>
            /// The priority of the task
            /// </summary>
            public int Priority { get { return (ObjectType)ObjectIdentifier.Type == ObjectType.Device ? 1 : 3; } }

            /// <summary>
            /// The device instance of the object
            /// </summary>
            public uint DeviceInstance { get; private set; }

            /// <summary>
            /// The object identifier of the object
            /// </summary>
            public ObjectId ObjectIdentifier { get; private set; }

            /// <summary>
            /// Constructs a new refresh object task
            /// </summary>
            /// <param name="deviceInstance">The device instance of the object</param>
            /// <param name="objectIdentifier">The object identifier of the object</param>
            public RefreshObjectTask(uint deviceInstance, ObjectId objectIdentifier)
            {
                this.DeviceInstance = deviceInstance;
                this.ObjectIdentifier = objectIdentifier;
            }

            /// <summary>
            /// Executes the refresh task
            /// </summary>
            /// <param name="db">The database that is executing the task</param>
            public void Execute(NetworkDatabase db)
            {
                var client = new Client(db._host);
                var queue = new ReadQueue(client);
                var info = db._db.GetObject(DeviceInstance, ObjectIdentifier);
                info.Refresh(queue);

                try
                {
                    queue.Send();
                    db._db.UpdateObject(info);
                    db._updateObject(info);
                }
                catch (AggregateException)
                {
                }
            }
        }

        private class RefreshObjectsTask : IRefreshTask
        {
            /// <summary>
            /// The priority of the task
            /// </summary>
            public int Priority { get { return 3; } }

            /// <summary>
            /// The vendor id of the device
            /// </summary>
            public ushort VendorId { get; private set; }

            /// <summary>
            /// The device instance of the object
            /// </summary>
            public uint DeviceInstance { get; private set; }

            /// <summary>
            /// The object identifiers of the objects
            /// </summary>
            public ObjectId[] ObjectIdentifiers { get; private set; }

            /// <summary>
            /// Constructs a new refresh objects task
            /// </summary>
            /// <param name="vendorId">The vendor id of the device</param>
            /// <param name="deviceInstance">The device instance of the object</param>
            /// <param name="objectIdentifiers">The object identifiers of the objects</param>
            public RefreshObjectsTask(ushort vendorId, uint deviceInstance, ObjectId[] objectIdentifiers)
            {
                this.VendorId = vendorId;
                this.DeviceInstance = deviceInstance;
                this.ObjectIdentifiers = objectIdentifiers;
            }

            /// <summary>
            /// Executes the refresh task
            /// </summary>
            /// <param name="db">The database that is executing the task</param>
            public void Execute(NetworkDatabase db)
            {
                var client = new Client(db._host);
                var queue = new ReadQueue(client);
                ObjectInfo[] infos = new ObjectInfo[ObjectIdentifiers.Length];

                for(int i = 0; i < infos.Length; i++)
                {
                    infos[i] = db._options.DescriptorRegistrar.CreateDescriptor(
                        VendorId,
                        DeviceInstance,
                        ObjectIdentifiers[i]);

                    infos[i].Refresh(queue);
                }

                try
                {
                    queue.Send();
                    db._db.UpdateObjects(infos);
                    db._updateObjects(infos);
                }
                catch (AggregateException)
                {
                }
            }
        }
    }
}
