using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Client.Db
{
    internal class TableExistsCommand
    {
        /// <summary>
        /// The sqlite command instance
        /// </summary>
        private SQLiteCommand _command;

        /// <summary>
        /// The table name parameter
        /// </summary>
        private SQLiteParameter _tableName;

        /// <summary>
        /// Constructs a new table exists command instance
        /// </summary>
        public TableExistsCommand(SQLiteConnection connection)
        {
            _command = new SQLiteCommand(Queries.TableExists, connection);
            _tableName = new SQLiteParameter("@tableName");
            _command.Parameters.Add(_tableName);
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="tableName">The table name to check for</param>
        /// <returns>True if the table exists, false otherwise</returns>
        public bool Execute(string tableName)
        {
            long count;

            lock(this)
            {
                _tableName.Value = tableName;
                count = (long)_command.ExecuteScalar();
                return count > 0;
            }
        }


        /// <summary>
        /// Disposes of the command
        /// </summary>
        public void Dispose()
        {
            if(_command != null)
            {
                _command.Dispose();
                _command = null;
            }
        }
    }
}
