using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Core;

namespace BACnet.Client
{
    public class DeviceFinderOptions : IProcessOptions
    {
        /// <summary>
        /// The process id of the process
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// Creates a new device finder options instance
        /// </summary>
        public DeviceFinderOptions()
        {
            this.ProcessId = DefaultProcessIds.DeviceFinder;
        }

        /// <summary>
        /// Creates the device finder process
        /// </summary>
        /// <returns>The device finder process</returns>
        public IProcess Create()
        {
            return new DeviceFinder(this);
        }

        /// <summary>
        /// Creates an identical copy of this options instance
        /// </summary>
        /// <returns>The new device finder options instance</returns>
        public DeviceFinderOptions Clone()
        {
            return new DeviceFinderOptions()
            {
                ProcessId = this.ProcessId
            };
        }
    }
}
