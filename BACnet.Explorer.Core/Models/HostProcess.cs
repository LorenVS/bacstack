using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Core;
using BACnet.Core.App;
using BACnet.Types;

namespace BACnet.Explorer.Core.Models
{
    public class HostProcess : Process
    {
        public HostProcess()
        {
            this.Name = Constants.HostDefaultName;
            this.ProcessId = BACnet.Core.DefaultProcessIds.Host;
        }

        /// <summary>
        /// Creates a process options instance
        /// </summary>
        /// <returns>The process options instance</returns>
        public override IProcessOptions CreateOptions()
        {
            var opts = new HostOptions()
            {
                ProcessId = this.ProcessId
            };

            return opts;
        }
    }
}
