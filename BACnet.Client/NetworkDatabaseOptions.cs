using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Core;
using BACnet.Client.Descriptors;

namespace BACnet.Client
{
    public class NetworkDatabaseOptions : IProcessOptions
    {

        /// <summary>
        /// The process id of the network database
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// Whether or not object lists and object names
        /// should be loaded eagerly. If set to true, object lists
        /// will be loaded whenever a device is found, if false, object lists
        /// will be only be loaded when an explicit call to LoadObjects()
        /// is made
        /// </summary>
        public bool LoadObjectsEagerly { get; set; }

        /// <summary>
        /// The path to the database file
        /// </summary>
        public string DatabasePath { get; set; }

        /// <summary>
        /// The descriptor registrar for registering various descriptor types
        /// </summary>
        public DescriptorRegistrar DescriptorRegistrar { get; private set; }

        /// <summary>
        /// Constructs a new network database options instance
        /// </summary>
        public NetworkDatabaseOptions()
        {
            this.ProcessId = DefaultProcessIds.NetworkDatabase;
            this.DatabasePath = "network.db";
            this.DescriptorRegistrar = new DescriptorRegistrar();
        }

        /// <summary>
        /// Creates a new network database process using these options
        /// </summary>
        /// <returns>The network database process instance</returns>
        public IProcess Create()
        {
            return new NetworkDatabase(this);
        }

        /// <summary>
        /// Creates an identical copy of this network database options
        /// instance
        /// </summary>
        /// <returns>The cloned instance</returns>
        public NetworkDatabaseOptions Clone()
        {
            return new NetworkDatabaseOptions()
            {
                ProcessId = this.ProcessId,
                LoadObjectsEagerly = this.LoadObjectsEagerly,
                DatabasePath = this.DatabasePath,
                DescriptorRegistrar = this.DescriptorRegistrar.Clone()
            };
        }
    }
}
