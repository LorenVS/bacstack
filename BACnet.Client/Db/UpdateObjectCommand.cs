using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BACnet.Types;
using BACnet.Client.Descriptors;

namespace BACnet.Client.Db
{
    internal class UpdateObjectCommand : IDisposable
    {
        /// <summary>
        /// The sqlite command instance
        /// </summary>
        private SQLiteCommand _command;

        /// <summary>
        /// The device instance of the object to get
        /// </summary>
        private SQLiteParameter _deviceInstance;

        /// <summary>
        /// The object type of the object to get
        /// </summary>
        private SQLiteParameter _objectType;

        /// <summary>
        /// The object instance of the object to get
        /// </summary>
        private SQLiteParameter _instance;

        /// <summary>
        /// The name of the object
        /// </summary>
        private SQLiteParameter _name;

        /// <summary>
        /// The extended properties of the object
        /// </summary>
        private SQLiteParameter _props;

        /// <summary>
        /// Constructs a new update object command instance
        /// </summary>
        public UpdateObjectCommand(SQLiteConnection connection)
        {
            _command = new SQLiteCommand(Queries.UpdateObject, connection);
            _deviceInstance = new SQLiteParameter("@deviceInstance");
            _objectType = new SQLiteParameter("@objectType");
            _instance = new SQLiteParameter("@instance");
            _name = new SQLiteParameter("@name");
            _props = new SQLiteParameter("@props");

            _command.Parameters.Add(_deviceInstance);
            _command.Parameters.Add(_objectType);
            _command.Parameters.Add(_instance);
            _command.Parameters.Add(_name);
            _command.Parameters.Add(_props);
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="info">The descriptor to update</param>
        /// <returns>True if an object was updated, false otherwise</returns>
        public bool Execute(ObjectInfo info)
        {
            bool ret = false;

            lock (this)
            {
                _deviceInstance.Value = info.DeviceInstance;
                _objectType.Value = info.ObjectIdentifier.Type;
                _instance.Value = info.ObjectIdentifier.Instance;
                _name.Value = info.Name;
                _props.Value = info.SaveProperties();
                ret = (_command.ExecuteNonQuery() == 1);
            }

            return ret;
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
