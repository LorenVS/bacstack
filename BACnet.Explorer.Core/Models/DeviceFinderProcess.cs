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
    public class DeviceFinderProcess : Process
    {
        public DeviceFinderProcess()
        {
            this.Name = Constants.DeviceFinderDefaultName;
            this.ProcessId = BACnet.Client.DefaultProcessIds.DeviceFinder;
        }

        /// <summary>
        /// Creates a process options instance
        /// </summary>
        /// <returns>The process options instance</returns>
        public override IProcessOptions CreateOptions()
        {
            var ret = new DeviceFinderOptions()
            {
                ProcessId = this.ProcessId
            };
            
            return ret;
        }
    }
}
