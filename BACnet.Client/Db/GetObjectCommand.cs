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
    internal class GetObjectCommand : IDisposable
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
        /// Constructs a new query objects command instance
        /// </summary>
        public GetObjectCommand(SQLiteConnection connection)
        {
            _command = new SQLiteCommand(Queries.GetObject, connection);
            _deviceInstance = new SQLiteParameter("@deviceInstance");
            _objectType = new SQLiteParameter("@objectType");
            _instance = new SQLiteParameter("@instance");

            _command.Parameters.Add(_deviceInstance);
            _command.Parameters.Add(_objectType);
            _command.Parameters.Add(_instance);
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="registrar">The registrar of descriptor types</param>
        /// <param name="deviceInstance">The device instance of the object to get</param>
        /// <param name="objectIdentifier">The object identifier of the object to get</param>
        /// <returns>The descriptors that match the query</returns>
        public ObjectInfo Execute(DescriptorRegistrar registrar, uint deviceInstance, ObjectId objectIdentifier)
        {
            ObjectInfo info = null;

            lock (this)
            {
                _deviceInstance.Value = deviceInstance;
                _objectType.Value = objectIdentifier.Type;
                _instance.Value = objectIdentifier.Instance;

                using (var reader = _command.ExecuteReader())
                {
                    int vendorIdOrdinal = reader.GetOrdinal("vendor_id");
                    int nameOrdinal = reader.GetOrdinal("name");
                    int propsOrdinal = reader.GetOrdinal("props");

                    if (reader.Read())
                    {
                        ushort vendorId = (ushort)reader.GetInt32(vendorIdOrdinal);
                        string name = reader.IsDBNull(nameOrdinal) ? null : reader.GetString(nameOrdinal);

                        info = registrar.CreateDescriptor(
                                vendorId,
                                deviceInstance,
                                objectIdentifier);
                        info.Name = name;

                        if (!reader.IsDBNull(propsOrdinal))
                        {
                            var props = reader.GetString(propsOrdinal);
                            info.LoadProperties(props);
                        }
                    }
                }
            }

            return info;
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
