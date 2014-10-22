using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Client.Db
{
    internal class GetVersionCommand : IDisposable
    {
        /// <summary>
        /// The sqlite command instance
        /// </summary>
        private SQLiteCommand _command;
        
        /// <summary>
        /// Constructs a new table exists command instance
        /// </summary>
        public GetVersionCommand(SQLiteConnection connection)
        {
            _command = new SQLiteCommand(Queries.GetVersion, connection);
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <returns>The current version</returns>
        public string Execute()
        {
            lock(this)
            {
                return (string)_command.ExecuteScalar();
            }
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
