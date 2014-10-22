using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Core;
using BACnet.Core.Network;

namespace BACnet.Explorer.Core.Models
{
    public class RouterProcess : Process
    {
        /// <summary>
        /// The mappings between network numbers and port ids
        /// </summary>
        public ObservableCollection<PortMapping> PortMappings { get; private set; }

        public RouterProcess()
        {
            this.Name = Constants.RouterDefaultName;
            this.ProcessId = BACnet.Core.DefaultProcessIds.Router;
            PortMappings = new ObservableCollection<PortMapping>();
        }

        /// <summary>
        /// Creates a process options instance
        /// </summary>
        /// <returns>The process options instance</returns>
        public override IProcessOptions CreateOptions()
        {
            var opts = new RouterOptions()
            {
                ProcessId = this.ProcessId
            };

            opts.PortNetworkMappings.AddRange(
                this.PortMappings.Select(mapping => new KeyValuePair<byte, ushort>(mapping.PortId, mapping.Network)));

            return opts;
        }
    }
}
