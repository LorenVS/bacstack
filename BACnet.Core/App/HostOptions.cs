﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.App
{
    public class HostOptions : IProcessOptions
    {
        /// <summary>
        /// The process id of the host
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// Constructs a new host options instance
        /// </summary>
        public HostOptions()
        {
            this.ProcessId = DefaultProcessIds.Host;
        }

        /// <summary>
        /// Creates a new host process using these options
        /// </summary>
        /// <returns>The host process instance</returns>
        public IProcess Create()
        {
            return new Host(this);
        }

        /// <summary>
        /// Clones this HostOptions instance
        /// </summary>
        /// <returns>The cloned instance</returns>
        public HostOptions Clone()
        {
            return new HostOptions()
            {
                ProcessId = this.ProcessId
            };
        }
    }
}
