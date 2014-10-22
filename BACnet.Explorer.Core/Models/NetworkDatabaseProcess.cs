using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Core;
using BACnet.Client;
using BACnet.Client.Descriptors;

namespace BACnet.Explorer.Core.Models
{
    public class NetworkDatabaseProcess : Process
    {
        /// <summary>
        /// The path to store the database file at
        /// </summary>
        public string DatabasePath
        {
            get { return _databasePath; }
            set { changeProperty(ref _databasePath, value, nameof(DatabasePath)); }
        }
        private string _databasePath;

        public NetworkDatabaseProcess()
        {
            this.Name = Constants.NetworkDatabaseDefaultName;
            this.ProcessId = BACnet.Client.DefaultProcessIds.NetworkDatabase;
            this.DatabasePath = "network.db";
        }

        /// <summary>
        /// Creates a process options instance
        /// </summary>
        /// <returns>The process options instance</returns>
        public override IProcessOptions CreateOptions()
        {
            var ret = new NetworkDatabaseOptions()
            {
                ProcessId = this.ProcessId,
                DatabasePath = this.DatabasePath
            };

            ret.DescriptorRegistrar.Register((ushort)ObjectType.Device,
                (vi, di, oi) => new DeviceInfo(vi, di, oi));

            return ret;
        }
    }
}
