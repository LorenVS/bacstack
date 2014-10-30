using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Jobs
{
    public interface IJobInvocation
    {
        /// <summary>
        /// The scheduler that scheduled the job
        /// </summary>
        IScheduler Scheduler { get; }

        /// <summary>
        /// Notifies the scheduler that the job has completed
        /// </summary>
        void Complete();
    }
}
