using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;

namespace BACnet.Core.Jobs
{
    public interface IJob
    {
        /// <summary>
        /// The relative weight of the job
        /// </summary>
        int Weight { get; }

        /// <summary>
        /// The destination device
        /// </summary>
        Recipient Destination { get; }

        /// <summary>
        /// Executes the job
        /// </summary>
        /// <param name="invocation">The invocation context</param>
        void Execute(IJobInvocation invocation);
    }
}
