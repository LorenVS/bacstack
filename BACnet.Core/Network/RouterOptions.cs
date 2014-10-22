using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Network
{
    public class RouterOptions : IProcessOptions
    {
        /// <summary>
        /// The process id of the router
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// Mappings between ports and networks for local routes
        /// </summary>
        public List<KeyValuePair<byte, ushort>> PortNetworkMappings { get; private set; }

        /// <summary>
        /// Constructs a new router options instance
        /// </summary>
        public RouterOptions()
        {
            ProcessId = DefaultProcessIds.Router;
            PortNetworkMappings = new List<KeyValuePair<byte, ushort>>();
        }

        /// <summary>
        /// Constructs a new router using these options
        /// </summary>
        /// <returns>The router instance</returns>
        public IProcess Create()
        {
            return new Router(this);
        }

        /// <summary>
        /// Creates an identical copy of this config object
        /// </summary>
        /// <returns>The cloned object</returns>
        public RouterOptions Clone()
        {
            RouterOptions clone = new RouterOptions()
            {
                ProcessId = this.ProcessId
            };

            clone.PortNetworkMappings.AddRange(this.PortNetworkMappings);
            return clone;
        }
    }
}