using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Jobs
{
    public class SimpleSchedulerOptions : IProcessOptions
    {
        /// <summary>
        /// The pocess id of the scehduler
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// The maximum number of concurrent jobs
        /// </summary>
        public int MaxConcurrentJobs { get; set; }

        /// <summary>
        /// Clones the simple scheduler options
        /// </summary>
        /// <returns>The new options instance</returns>
        public SimpleSchedulerOptions Clone()
        {
            return new SimpleSchedulerOptions()
            {
                ProcessId = this.ProcessId,
                MaxConcurrentJobs = this.MaxConcurrentJobs
            };
        }

        /// <summary>
        /// Creates a new simple scheduler process
        /// </summary>
        /// <returns></returns>
        public IProcess Create()
        {
            return new SimpleScheduler(this);
        }
    }
}
