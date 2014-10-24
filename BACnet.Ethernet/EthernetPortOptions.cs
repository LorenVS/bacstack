using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Core;

namespace BACnet.Ethernet
{
    public class EthernetPortOptions : IProcessOptions
    {
        /// <summary>
        /// The process id of the ethernet port
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// The port id of the ethernet port
        /// </summary>
        public byte PortId { get; set; }

        /// <summary>
        /// The ethernet device to use
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// Creates a new ethernet port options instance
        /// </summary>
        public EthernetPortOptions()
        {
            this.ProcessId = DefaultProcessIds.EthernetPort;
        }

        /// <summary>
        /// Creates an ethernet port from this port options instance
        /// </summary>
        /// <returns>The ethernet port</returns>
        public IProcess Create()
        {
            return new EthernetPort(this);
        }

        /// <summary>
        /// Creates an identical copy of the ethernet port options
        /// </summary>
        /// <returns></returns>
        public EthernetPortOptions Clone()
        {
            return new EthernetPortOptions()
            {
                ProcessId = this.ProcessId,
                DeviceName = this.DeviceName
            };
        }
    }
}
