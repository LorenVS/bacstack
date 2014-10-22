using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;
using BACnet.Client.Descriptors;

namespace BACnet.Client.Db
{
    /// <summary>
    /// Wrapper around the SQLite databased used by NetworkDatabase
    /// </summary>
    internal class NetworkDb : IDisposable
    {
        /// <summary>
        /// The format string for generating a sqlite connection string
        /// </summary>
        private const string _connectionStringFS = "Data Source={0};Version=3;FailIfMissing=False";

        /// <summary>
        /// The current version of the database schema
        /// </summary>
        private const string _currentVersion = "v0.3";

        /// <summary>
        /// The path to the database file
        /// </summary>
        private string _path;

        /// <summary>
        /// The descriptor registrar
        /// </summary>
        private DescriptorRegistrar _registrar;

        /// <summary>
        /// The various commands for the database
        /// </summary>
        private Commands _commands;

        /// <summary>
        /// Lock synchronizing access to the connection
        /// </summary>
        private object _connectionLock = new object();

        /// <summary>
        /// Whether or not the database has been disposed
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Connection to the database
        /// </summary>
        private SQLiteConnection _connection;
        
        /// <summary>
        /// Creates a new network database instance
        /// </summary>
        /// <param name="path">The path to the database file</param>
        /// <param name="registrar">The descripto registrar</param>
        public NetworkDb(string path, DescriptorRegistrar registrar)
        {
            this._disposed = false;
            this._path = path;
            this._registrar = registrar;
            this._createConnection();
            this._ensureSchema();
        }

        /// <summary>
        /// Disposes of all resources held by the network database
        /// </summary>
        public void Dispose()
        {
            lock (_connectionLock)
            {
                this._disposed = true;
                _disposeConnection();
            }
        }
        
        /// <summary>
        /// Retrieves a connection string for the sqlite database
        /// </summary>
        /// <param name="path">The path to the database file</param>
        /// <returns>The connection string</returns>
        private static string _getConnectionString(string path)
        {
            return string.Format(_connectionStringFS, path);
        }

        /// <summary>
        /// Determines whether a certain table exists
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <returns>True if the table exists, false otherwise</returns>
        private bool _tableExists(string tableName)
        {
            return _commands.TableExists.Execute(tableName);
        }

        /// <summary>
        /// Retrieves the current version of the schema
        /// </summary>
        /// <returns>The version string</returns>
        private string _getVersion()
        {
            return _commands.GetVersion.Execute();
        }

        /// <summary>
        /// Deletes the database file and creates a new one
        /// </summary>
        private void _resetDatabase()
        {
            _disposeConnection();

            // remove the database file
            if (File.Exists(_path))
                File.Delete(_path);

            // create the new connection
            _createConnection();
        }

        /// <summary>
        /// Creates the SQLite connection
        /// </summary>
        private void _createConnection()
        {
            string connectionString = _getConnectionString(_path);
            this._connection = new SQLiteConnection(connectionString);
            this._connection.Open();
            this._commands = new Commands(this._connection);
        }

        /// <summary>
        /// Disposes of the SQLite connection and the command instances
        /// </summary>
        private void _disposeConnection()
        {
            if (this._commands != null)
            {
                this._commands.Dispose();
                this._commands = null;
            }

            if (this._connection != null)
            {
                this._connection.Dispose();
                this._connection = null;
            }
        }

        /// <summary>
        /// Ensures that the current schema is present in
        /// the database file
        /// </summary>
        private void _ensureSchema()
        {
            // if the version table exists and has a record for the current
            // version, we don't have to modify the schema at all
            if (_tableExists("Version") && _getVersion() == _currentVersion)
                return;

            // otherwise, we start from scratch, and delete the database
            // file before recreating it
            _resetDatabase();

            // then we recreate all of the required schema objects
            _commands.CreateSchema.Execute();
        }

        /// <summary>
        /// Upserts an object into the network db
        /// </summary>
        /// <param name="vendorId">The vendor id of the object</param>
        /// <param name="deviceInstance">The device instance of the object</param>
        /// <param name="objectId">The object id of the object</param>
        /// <returns>True if the object was inserted, false if it already existed</returns>
        public bool UpsertObject(ushort vendorId, uint deviceInstance, ObjectId objectId)
        {
            bool ret = false;

            lock(_connectionLock)
            if(!_disposed)
            {
                ret = _commands.UpsertObject.Execute(vendorId, deviceInstance, objectId);
            }

            return ret;
        }

        /// <summary>
        /// Upserts a collection of objects into the network db
        /// </summary>
        /// <param name="vendorId">The vendor id of the objects</param>
        /// <param name="deviceInstance">The device instance of the objects</param>
        /// <param name="objectIds">The object ids to upsert</param>
        /// <returns>The object ids that were inserted</returns>
        public List<ObjectId> UpsertObjects(ushort vendorId, uint deviceInstance, IEnumerable<ObjectId> objectIds)
        {
            List<ObjectId> inserted = new List<ObjectId>();

            lock(_connectionLock)
            if(!_disposed)
            using(var trans = _connection.BeginTransaction())
            {
                foreach (var objectId in objectIds)
                {
                    if (_commands.UpsertObject.Execute(vendorId, deviceInstance, objectId))
                        inserted.Add(objectId);
                }

                trans.Commit();
            }

            return inserted;
        }

        /// <summary>
        /// Retrieves a single object's descriptor
        /// </summary>
        /// <param name="deviceInstance">The device instance of the object</param>
        /// <param name="objectIdentifier">The object identifier of the object</param>
        /// <returns>The descriptor, or null if no descriptor exists</returns>
        public T GetObject<T>(uint deviceInstance, ObjectId objectIdentifier) where T : ObjectInfo
        {
            return GetObject(deviceInstance, objectIdentifier) as T;
        }

        /// <summary>
        /// Retrieves a single object's descriptor
        /// </summary>
        /// <param name="deviceInstance">The device instance of the object</param>
        /// <param name="objectIdentifier">The object identifier of the object</param>
        /// <returns>The descriptor, or null if no descriptor exists</returns>
        public ObjectInfo GetObject(uint deviceInstance, ObjectId objectIdentifier)
        {
            ObjectInfo ret = null;

            lock (_connectionLock)
            if(!_disposed)
            {
                ret = _commands.GetObject.Execute(_registrar, deviceInstance, objectIdentifier);
            }

            return ret;
        }

        /// <summary>
        /// Queries the list of objects for objects matching
        /// a descriptor query
        /// </summary>
        /// <param name="query">The query</param>
        /// <returns>The list of objects</returns>
        public List<ObjectInfo> QueryObjects(DescriptorQuery query)
        {
            List<ObjectInfo> ret = null;

            lock(_connectionLock)
            if(!_disposed)
            {
                ret = _commands.QueryObjects.Execute(_registrar, query);
            }

            return ret ?? new List<ObjectInfo>();
        }

        /// <summary>
        /// Updates a descriptor object in the database
        /// </summary>
        /// <param name="info">The descriptor to update</param>
        /// <returns>True if a descriptor was updated, false otherwise</returns>
        public bool UpdateObject(ObjectInfo info)
        {
            bool ret = false;

            lock(_connectionLock)
            if(!_disposed)
            {
                ret = _commands.UpdateObject.Execute(info);
            }

            return ret;
        }

        /// <summary>
        /// Updates a set of descriptor objects
        /// </summary>
        /// <param name="infos">The descriptors to update</param>
        /// <returns>The number of rows updated</returns>
        public int UpdateObjects(ObjectInfo[] infos)
        {
            int count = 0;

            lock(_connectionLock)
            if(!_disposed)
            using (var tx = _connection.BeginTransaction())
            {
                for (int i = 0; i < infos.Length; i++)
                {
                    var updated = _commands.UpdateObject.Execute(infos[i]);
                    count += updated ? 1 : 0;
                }

                tx.Commit();
            }

            return count;
        }

        /// <summary>
        /// Retrieves a list of the objects that are in need of refreshing
        /// </summary>
        /// <param name="minTimeBetweenAttempts">The minimum time between two attempts to refresh the same object</param>
        /// <param name="refreshInterval">The interval at which objects should be refreshed</param>
        /// <param name="limit">The maximum number of objects to retrieve</param>
        /// <returns>The list of object ids</returns>
        public List<GlobalObjectId> GetQueuedRefreshes(TimeSpan minTimeBetweenAttempts, TimeSpan refreshInterval, int limit)
        {
            List<GlobalObjectId> ret = null;

            lock(_connectionLock)
            if(!_disposed)
            {
                ret = _commands.GetQueuedRefreshes.Execute(minTimeBetweenAttempts, refreshInterval, limit);
            }

            return ret ?? new List<GlobalObjectId>();
        }

        private class Commands : IDisposable
        {
            public TableExistsCommand TableExists { get; private set; }
            public GetVersionCommand GetVersion { get; private set; }
            public CreateSchemaCommand CreateSchema { get; private set; }
            public UpsertObjectCommand UpsertObject { get; private set; }
            public GetObjectCommand GetObject { get; private set; }
            public QueryObjectsCommand QueryObjects { get; private set; }
            public UpdateObjectCommand UpdateObject { get; private set; }
            public GetQueuedRefreshesCommand GetQueuedRefreshes { get; private set; }

            public Commands(SQLiteConnection connection)
            {
                this.TableExists = new TableExistsCommand(connection);
                this.GetVersion = new GetVersionCommand(connection);
                this.CreateSchema = new CreateSchemaCommand(connection);
                this.UpsertObject = new UpsertObjectCommand(connection);
                this.GetObject = new GetObjectCommand(connection);
                this.QueryObjects = new QueryObjectsCommand(connection);
                this.UpdateObject = new UpdateObjectCommand(connection);
                this.GetQueuedRefreshes = new GetQueuedRefreshesCommand(connection);
            }

            public void Dispose()
            {
                if(TableExists != null)
                {
                    TableExists.Dispose();
                    TableExists = null;
                }

                if (GetVersion != null)
                {
                    GetVersion.Dispose();
                    GetVersion = null;
                }

                if (CreateSchema != null)
                {
                    CreateSchema.Dispose();
                    CreateSchema = null;
                }

                if(UpsertObject != null)
                {
                    UpsertObject.Dispose();
                    UpsertObject = null;
                }

                if(GetObject != null)
                {
                    GetObject.Dispose();
                    GetObject = null;
                }

                if(QueryObjects != null)
                {
                    QueryObjects.Dispose();
                    QueryObjects = null;
                }

                if(UpdateObject != null)
                {
                    UpdateObject.Dispose();
                    UpdateObject = null;
                }

                if(GetQueuedRefreshes != null)
                {
                    GetQueuedRefreshes.Dispose();
                    GetQueuedRefreshes = null;
                }
            }
        }
    }
}
