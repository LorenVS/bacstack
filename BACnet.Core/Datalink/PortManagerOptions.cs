using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Datalink
{
    public class PortManagerOptions : IProcessOptions
    {
        /// <summary>
        /// The process id of the port manager
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// Constructs a new port manager options instance
        /// </summary>
        public PortManagerOptions()
        {
            this.ProcessId = DefaultProcessIds.PortManager;
        }

        /// <summary>
        /// Creates a new port manager process using these options
        /// </summary>
        /// <returns>The port manager instance</returns>
        public IProcess Create()
        {
            return new PortManager(this);
        }

        /// <summary>
        /// Creates an identical copy of this port manager
        /// options instance
        /// </summary>
        /// <returns>The cloned instance</returns>
        public PortManagerOptions Clone()
        {
            return new PortManagerOptions()
            {
                ProcessId = this.ProcessId
            };
        }
    }
}
