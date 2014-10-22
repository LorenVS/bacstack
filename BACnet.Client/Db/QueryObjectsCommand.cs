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
    internal class QueryObjectsCommand  : IDisposable
    {
        /// <summary>
        /// The sqlite command instance
        /// </summary>
        private SQLiteCommand _command;

        /// <summary>
        /// The device instance to query for
        /// </summary>
        private SQLiteParameter _deviceInstance;

        /// <summary>
        /// The object type to query for
        /// </summary>
        private SQLiteParameter _objectType;
        
        /// <summary>
        /// Constructs a new query objects command instance
        /// </summary>
        public QueryObjectsCommand(SQLiteConnection connection)
        {
            _command = new SQLiteCommand(Queries.QueryObjects, connection);
            _deviceInstance = new SQLiteParameter("@deviceInstance");
            _objectType = new SQLiteParameter("@objectType");

            _command.Parameters.Add(_deviceInstance);
            _command.Parameters.Add(_objectType);
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="registrar">The registrar of descriptor types</param>
        /// <param name="query">The descriptor query</param>
        /// <returns>The descriptors that match the query</returns>
        public List<ObjectInfo> Execute(DescriptorRegistrar registrar, DescriptorQuery query)
        {
            List<ObjectInfo> ret = new List<ObjectInfo>();

            lock(this)
            {
                _deviceInstance.Value = query.DeviceInstance;
                _objectType.Value = query.ObjectType;

                using (var reader = _command.ExecuteReader())
                {
                    int deviceInstanceOrdinal = reader.GetOrdinal("device_instance");
                    int objectTypeOrdinal = reader.GetOrdinal("object_type");
                    int instanceOrdinal = reader.GetOrdinal("instance");
                    int vendorIdOrdinal = reader.GetOrdinal("vendor_id");
                    int nameOrdinal = reader.GetOrdinal("name");
                    int propsOrdinal = reader.GetOrdinal("props");

                    while (reader.Read())
                    {
                        uint deviceInstance = (uint)reader.GetInt32(deviceInstanceOrdinal);
                        ushort objectType = (ushort)reader.GetInt32(objectTypeOrdinal);
                        uint instance = (uint)reader.GetInt32(instanceOrdinal);
                        ushort vendorId = (ushort)reader.GetInt32(vendorIdOrdinal);
                        string name = reader.IsDBNull(nameOrdinal) ? null :  reader.GetString(nameOrdinal);

                        if (query.NameRegex == null || (name != null && Regex.IsMatch(name, query.NameRegex, RegexOptions.IgnoreCase)))
                        {
                            var info = registrar.CreateDescriptor(
                                vendorId,
                                deviceInstance,
                                new ObjectId(objectType, instance));
                            info.Name = name;

                            if (!reader.IsDBNull(propsOrdinal))
                            {
                                var props = reader.GetString(propsOrdinal);
                                info.LoadProperties(props);
                            }

                            ret.Add(info);
                        }
                    }
                }
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
