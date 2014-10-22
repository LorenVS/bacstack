using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Client.Db
{
    internal class CreateSchemaCommand
    {
        /// <summary>
        /// The sqlite command instance
        /// </summary>
        private SQLiteCommand _command;
        
        /// <summary>
        /// Constructs a new create schema instance
        /// </summary>
        public CreateSchemaCommand(SQLiteConnection connection)
        {
            _command = new SQLiteCommand(Queries.CreateSchema, connection);
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        public void Execute()
        {
            lock(this)
            {
                _command.ExecuteNonQuery();
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
