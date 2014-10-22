using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Core;
using BACnet.Core.Datalink;

namespace BACnet.Explorer.Core.Models
{
    public class PortManagerProcess : Process
    {
        public PortManagerProcess()
        {
            this.Name = Constants.PortManagerDefaultName;
            this.ProcessId = BACnet.Core.DefaultProcessIds.PortManager;
        }

        /// <summary>
        /// Creates a process options instance
        /// </summary>
        /// <returns>The process options instance</returns>
        public override IProcessOptions CreateOptions()
        {
            return new PortManagerOptions()
            {
                ProcessId = this.ProcessId
            };
        }
    }
}
