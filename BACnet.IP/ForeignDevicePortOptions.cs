using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Core;

namespace BACnet.IP
{
    public class ForeignDevicePortOptions : IProcessOptions
    {
        /// <summary>
        /// The process id of the foreign device port
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// The unique id of the port
        /// </summary>
        public byte PortId { get; set; }

        /// <summary>
        /// The local hostname to bind to
        /// </summary>
        public string LocalHost { get; set; }

        /// <summary>
        /// The local port to bind to
        /// </summary>
        public ushort LocalPort { get; set; }

        /// <summary>
        /// The bbmd host to communicate with
        /// </summary>
        public string BbmdHost{get;set;}

        /// <summary>
        /// The bbmd udp port to communicate with
        /// </summary>
        public ushort BbmdPort { get; set; }

        /// <summary>
        /// The interval at which the foreign device should reregister
        /// with the bbmd
        /// </summary>
        public TimeSpan RegistrationInterval { get; set; }

        /// <summary>
        /// Constructs a new foreign device port options instance
        /// </summary>
        public ForeignDevicePortOptions()
        {
            this.ProcessId = DefaultProcessIds.ForeignDevicePort;
        }

        /// <summary>
        /// Creates a new foreign device port process using these options
        /// </summary>
        /// <returns>The foreign device port instance</returns>
        public IProcess Create()
        {
            return new ForeignDevicePort(this);
        }

        /// <summary>
        /// Creates an identical copy of this
        /// options instance
        /// </summary>
        /// <returns>The identical copy</returns>
        public ForeignDevicePortOptions Clone()
        {
            return new ForeignDevicePortOptions()
            {
                PortId = this.PortId,
                LocalHost = this.LocalHost,
                LocalPort = this.LocalPort,
                BbmdHost = this.BbmdHost,
                BbmdPort = this.BbmdPort,
                RegistrationInterval = this.RegistrationInterval
            };
        }
    }
}
