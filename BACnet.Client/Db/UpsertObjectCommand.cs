using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Client.Db
{
    internal class UpsertObjectCommand  : IDisposable
    {
        /// <summary>
        /// The sqlite command instance
        /// </summary>
        private SQLiteCommand _command;

        /// <summary>
        /// The vendor id of the object
        /// </summary>
        private SQLiteParameter _vendorId;

        /// <summary>
        /// The device instance of the object
        /// </summary>
        private SQLiteParameter _deviceInstance;

        /// <summary>
        /// The object type of the object
        /// </summary>
        private SQLiteParameter _objectType;

        /// <summary>
        /// The instance of the object
        /// </summary>
        private SQLiteParameter _instance;
        
        /// <summary>
        /// Constructs a new table exists command instance
        /// </summary>
        public UpsertObjectCommand(SQLiteConnection connection)
        {
            _command = new SQLiteCommand(Queries.UpsertObject, connection);
            _vendorId = new SQLiteParameter("@vendorId");
            _deviceInstance = new SQLiteParameter("@deviceInstance");
            _objectType = new SQLiteParameter("@objectType");
            _instance = new SQLiteParameter("@instance");

            _command.Parameters.Add(_vendorId);
            _command.Parameters.Add(_deviceInstance);
            _command.Parameters.Add(_objectType);
            _command.Parameters.Add(_instance);
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="vendorId">The vendor id of the object</param>
        /// <param name="deviceInstance">The device instance of the object</param>
        /// <param name="objectId">The object id of the object</param>
        /// <returns>True if the object was inserted, false otherwise</returns>
        public bool Execute(ushort vendorId, uint deviceInstance, ObjectId objectId)
        {
            int rows;

            lock(this)
            {
                _vendorId.Value = vendorId;
                _deviceInstance.Value = deviceInstance;
                _objectType.Value = objectId.Type;
                _instance.Value = objectId.Instance;
                rows = _command.ExecuteNonQuery();
            }

            return rows > 0;
        }

        /// <summary>
        /// Disposes of the command
        /// </summary>
        public void Dispose()
        {
            if (_command != null)
            {
                _command.Dispose();
                _command = null;
            }
        }

    }
}
