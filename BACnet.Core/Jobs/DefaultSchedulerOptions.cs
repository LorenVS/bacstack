using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Jobs
{
    public class DefaultSchedulerOptions
    {
        /// <summary>
        /// The process id of the scheduler process
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// The maximum number of concurrent jobs
        /// that can execute at once
        /// </summary>
        public int MaxConcurrentJobs { get; set; }
        
        /// <summary>
        /// The maximum number of jobs that can execute
        /// for each next hop device
        /// </summary>
        public int MaxConcurrentJobsPerNextHop { get; set; }

        /// <summary>
        /// Prime number used for hashing next hop addresses
        /// </summary>
        public int NextHopPrime { get; set; }

        /// <summary>
        /// The maximum number of jobs that can execute
        /// for each device
        /// </summary>
        public int MaxConcurrentJobsPerDevice { get; set; }

        /// <summary>
        /// Prime number used for hashing devices
        /// </summary>
        public int DevicePrime { get; set; }

        public DefaultSchedulerOptions()
        {
            this.MaxConcurrentJobs = 8;
            this.MaxConcurrentJobsPerNextHop = 3;
            this.NextHopPrime = 37;
            this.MaxConcurrentJobsPerDevice = 2;
            this.DevicePrime = 137;
        }

        /// <summary>
        /// Creates an identical copy of this default scheduler options instance
        /// </summary>
        /// <returns>The cloned instance</returns>
        public DefaultSchedulerOptions Clone()
        {
            return new DefaultSchedulerOptions()
            {
                ProcessId = this.ProcessId,
                MaxConcurrentJobs = this.MaxConcurrentJobs,
                MaxConcurrentJobsPerNextHop = this.MaxConcurrentJobsPerNextHop,
                NextHopPrime = this.NextHopPrime,
                MaxConcurrentJobsPerDevice = this.MaxConcurrentJobsPerDevice,
                DevicePrime = this.DevicePrime
            };
        }

        /// <summary>
        /// Creates a default scheduler using this options instance
        /// </summary>
        /// <returns><The created scheduler process/returns>
        public IProcess Create()
        {
            return new DefaultScheduler(this);
        }
    }
}
