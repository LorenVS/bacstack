using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Core.App;
using BACnet.Core.Datalink;
using BACnet.Core.Network

namespace BACnet.Core.Jobs
{
    public class DefaultScheduler : IScheduler
    {
        /// <summary>
        /// The maximum number of concurrent requests
        /// </summary>
        public int MaxConcurrentJobs { get; private set; }

        /// <summary>
        /// The maximum number of concurrent requests per
        /// next hop device
        /// </summary>
        public int MaxConcurrentJobsPerNextHop { get; private set; }

        /// <summary>
        /// The maximum number of concurrent requests per
        /// destination device
        /// </summary>
        public int MaxConcurrentJobsPerDevice { get; private set; }

        /// <summary>
        /// Lock used to synchronize access to the scheduler
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// The BACnet host
        /// </summary>
        private Host _host;

        /// <summary>
        /// The BACnet router
        /// </summary>
        private Router _router;

        /// <summary>
        /// The jobs that are scheduled to execute
        /// </summary>
        private LinkedList<JobInfo> _jobs;

        /// <summary>
        /// The total number of executing jobs
        /// </summary>
        private int _executingJobs;

        /// <summary>
        /// The number of jobs that are being executed
        /// broken out by next hop
        /// </summary>
        private Dictionary<Mac, int> _nextHopJobs;

        /// <summary>
        /// The number of jobs that are being executed
        /// broken out by device
        /// </summary>
        private Dictionary<Address, int> _deviceJobs;

        /// <summary>
        /// Constructs a new default scheduler instance
        /// </summary>
        /// <param name="maxConcurrentJobs">The maximum number of concurrent jobs to execute</param>
        /// <param name="maxConcurrentJobsPerNextHop">The maximum number of concurrent jobs per next hop device</param>
        /// <param name="maxConcurrentJobsPerDevice">The maximum number of concurrent jobs per device</param>
        public DefaultScheduler(int maxConcurrentJobs, int maxConcurrentJobsPerNextHop, int maxConcurrentJobsPerDevice)
        {
            this.MaxConcurrentJobs = maxConcurrentJobs;
            this.MaxConcurrentJobsPerNextHop = maxConcurrentJobsPerNextHop;
            this.MaxConcurrentJobsPerDevice = MaxConcurrentJobsPerDevice;
        }

        /// <summary>
        /// Determines whether a job can currently be executed
        /// </summary>
        /// <param name="job">The job info to check</param>
        /// <returns>True if the job can be executed, false otherwise</returns>
        private bool _canExecuteJob(JobInfo job)
        {
            int weight = job.Job.Weight;
            int nextHopExecuting = 0;
            int deviceExecuting = 0;

            if (!job.HasAllInformation())
                return false;
            else if (_executingJobs + weight > MaxConcurrentJobs)
                return false;
            else if (_nextHopJobs.TryGetValue(job.NextHop, out nextHopExecuting)
                && nextHopExecuting != 0
                && nextHopExecuting + weight > MaxConcurrentJobsPerNextHop)
                return false;
            else if (_deviceJobs.TryGetValue(job.Address, out deviceExecuting)
                && deviceExecuting != 0
                && deviceExecuting + weight > MaxConcurrentJobsPerDevice)
                return false;

            return true;
        }

        /// <summary>
        /// Sets a job as executing, updating the associated currently
        /// executing counts
        /// </summary>
        /// <param name="job">The job to execute</param>
        private void _setAsExecuting(JobInfo job)
        {
            int weight = job.Job.Weight;
            var mac = job.NextHop;
            var addr = job.Address;

            _executingJobs += weight;

            if (_nextHopJobs.ContainsKey(mac))
                _nextHopJobs[mac] += weight;
            else
                _nextHopJobs.Add(mac, weight);

            if (_deviceJobs.ContainsKey(addr))
                _deviceJobs[addr] += weight;
            else
                _deviceJobs.Add(addr, weight);
        }

        /// <summary>
        /// Schedules a new job for execution
        /// </summary>
        /// <param name="job">The job to schedule</param>
        public void Schedule(IJob job)
        {

        }

        private class JobInfo
        {
            /// <summary>
            /// The job to execute
            /// </summary>
            public IJob Job;

            /// <summary>
            /// The address of the destination device
            /// </summary>
            public Address Address;

            /// <summary>
            /// The route to the device
            /// </summary>
            public Mac NextHop;

            public JobInfo(IJob job)
            {
                this.Job = job;
                this.NextHop = Mac.Broadcast;

                if(job.Destination.IsAddress)
                {
                    var addr = job.Destination.AsAddress;
                    this.Address = new Address(
                        addr.NetworkNumber,
                        new Mac(addr.MacAddress));
                }
            }

            /// <summary>
            /// Determines whether all information
            /// needed to schedule this job has been obtained
            /// </summary>
            /// <returns></returns>
            public bool HasAllInformation()
            {
                return Address != null && !NextHop.IsBroadcast();
            }
        }
    }
}
