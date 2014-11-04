using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Jobs
{
    public interface IScheduler : IProcess
    {
        /// <summary>
        /// Schedules a job for execution
        /// </summary>
        /// <param name="job">The job to execute</param>
        void Schedule(IJob job);
    }
}
