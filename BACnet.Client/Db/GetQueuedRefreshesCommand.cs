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
    internal class GetQueuedRefreshesCommand  : IDisposable
    {
        /// <summary>
        /// The sqlite command instance
        /// </summary>
        private SQLiteCommand _command;

        /// <summary>
        /// The maximum lastRefresh timestamp that should be allowed
        /// </summary>
        private SQLiteParameter _lastRefreshThreshold;

        /// <summary>
        /// The maximum lastRefreshed timestamp that should be allowed
        /// </summary>
        private SQLiteParameter _lastRefreshedThreshold;

        /// <summary>
        /// The maximum number of items to return
        /// </summary>
        private SQLiteParameter _limit;
        
        /// <summary>
        /// Constructs a new query objects command instance
        /// </summary>
        public GetQueuedRefreshesCommand(SQLiteConnection connection)
        {
            _command = new SQLiteCommand(Queries.GetQueuedRefreshes, connection);
            _lastRefreshThreshold = new SQLiteParameter("lastRefreshThreshold");
            _lastRefreshedThreshold = new SQLiteParameter("lastRefreshedThreshold");
            _limit = new SQLiteParameter("limit");

            _command.Parameters.Add(_lastRefreshThreshold);
            _command.Parameters.Add(_lastRefreshedThreshold);
            _command.Parameters.Add(_limit);
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="minTimeBetweenAttempts">The minimum time between subsequent attempts to refresh the same object</param>
        /// <param name="refreshInterval">The time between object refreshes</param>
        /// <param name="limit">The maximum number of objects to return</param>
        /// <returns>The object ids of the objects to refresh</returns>
        public List<GlobalObjectId> Execute(TimeSpan minTimeBetweenAttempts, TimeSpan refreshInterval, int limit)
        {
            List<GlobalObjectId> ret = new List<GlobalObjectId>();

            lock(this)
            {
                var now = DateTime.UtcNow;
                _lastRefreshThreshold.Value = now - minTimeBetweenAttempts;
                _lastRefreshedThreshold.Value = now - refreshInterval;
                _limit.Value = limit;

                using (var reader = _command.ExecuteReader())
                {
                    int deviceInstanceOrdinal = reader.GetOrdinal("device_instance");
                    int objectTypeOrdinal = reader.GetOrdinal("object_type");
                    int instanceOrdinal = reader.GetOrdinal("instance");

                    while (reader.Read())
                    {
                        uint deviceInstance = (uint)reader.GetInt32(deviceInstanceOrdinal);
                        ushort objectType = (ushort)reader.GetInt32(objectTypeOrdinal);
                        uint instance = (uint)reader.GetInt32(instanceOrdinal);
                        ret.Add(new GlobalObjectId(
                            deviceInstance,
                            new ObjectId(objectType, instance)));
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
