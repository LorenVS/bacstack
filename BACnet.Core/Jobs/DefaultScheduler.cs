using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Core.App;
using BACnet.Core.Datalink;
using BACnet.Core.Network;

namespace BACnet.Core.Jobs
{
    public class DefaultScheduler : IScheduler, ISearchCallback<Recipient, DeviceTableEntry>
    {
        /// <summary>
        /// The process id of the BACnet process
        /// </summary>
        public int ProcessId { get { return _options.ProcessId; } }

        /// <summary>
        /// Lock used to synchronize access to the scheduler
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// Whether or not the scheduler has been disposed
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// The options for this scheduler
        /// </summary>
        private DefaultSchedulerOptions _options;

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
        /// Jobs that are currently searching for device information
        /// </summary>
        private LinkedList<JobInfo> _deviceSearches;

        /// <summary>
        /// Jobs that are currently searching for route information
        /// </summary>
        private LinkedList<JobInfo> _routeSearches;

        /// <summary>
        /// The total number of executing jobs
        /// </summary>
        private int _executingJobs;

        /// <summary>
        /// The next hop buckets
        /// </summary>
        private int[] _nextHopJobs;

        /// <summary>
        /// The device buckets
        /// </summary>
        private int[] _deviceJobs;

        /// <summary>
        /// Constructs a new default scheduler instance
        /// </summary>
        /// <param name="options">The scheduler options</param>
        public DefaultScheduler(DefaultSchedulerOptions options)
        {
            this._options = options.Clone();

            _jobs = new LinkedList<JobInfo>();
            _deviceSearches = new LinkedList<JobInfo>();
            _routeSearches = new LinkedList<JobInfo>();

            this._executingJobs = 0;
            this._nextHopJobs = new int[_options.NextHopPrime];
            this._deviceJobs = new int[_options.DevicePrime];
        }

        /// <summary>
        /// Disposes of the scheduler
        /// </summary>
        public void Dispose()
        {
            LinkedList<JobInfo> jobs = null;
            LinkedList<JobInfo> routeSearches = null;
            LinkedList<JobInfo> deviceSearches = null;

            lock(_lock)
            {
                if (_disposed)
                    return;

                _host = null;
                _router = null;

                jobs = _jobs;
                routeSearches = _routeSearches;
                deviceSearches = _deviceSearches;

                _jobs = null;
                _routeSearches = null;
                _deviceSearches = null;
            }

            foreach (var job in jobs)
                job.Job.Abort(AbortReason.PreemptedByHigherPriorityTask);
            foreach (var job in routeSearches)
                job.Job.Abort(AbortReason.PreemptedByHigherPriorityTask);
            foreach (var job in deviceSearches)
                job.Job.Abort(AbortReason.PreemptedByHigherPriorityTask);
        }

        /// <summary>
        /// Retrieves the bucket for a next hop address
        /// </summary>
        /// <param name="nextHop">The address to retrieve the bucket for</param>
        /// <returns>The bucket index</returns>
        private int _getNextHopBucket(Mac nextHop)
        {
            return nextHop.GetHashCode() % _options.NextHopPrime;
        }

        /// <summary>
        /// Retrieves the bucket for a device address
        /// </summary>
        /// <param name="addr">The address to retrieve the bucket for</param>
        /// <returns>The bucket index</returns>
        private int _getDeviceBucket(Address addr)
        {
            return addr.GetHashCode() % _options.DevicePrime;
        }

        /// <summary>
        /// Determines whether a job can currently be executed
        /// </summary>
        /// <param name="job">The job info to check</param>
        /// <returns>True if the job can be executed, false otherwise</returns>
        private bool _canExecuteJob(JobInfo job)
        {
            if (!job.HasAllInformation())
                return false;

            int weight = job.Job.Weight;
            int nextHop = _getNextHopBucket(job.NextHop);
            int device = _getDeviceBucket(job.Address);

            return
                (_executingJobs == 0 || _executingJobs + weight <= _options.MaxConcurrentJobs) &&
                (_nextHopJobs[nextHop] == 0 || _nextHopJobs[nextHop] + weight <= _options.MaxConcurrentJobsPerNextHop) &&
                (_deviceJobs[device] == 0 || _deviceJobs[device] + weight <= _options.MaxConcurrentJobsPerDevice);
        }

        /// <summary>
        /// Sets a job as executing, updating the associated currently
        /// executing counts
        /// </summary>
        /// <param name="job">The job to execute</param>
        private void _setAsExecuting(JobInfo job)
        {
            int weight = job.Job.Weight;
            int nextHop = _getNextHopBucket(job.NextHop);
            int device = _getDeviceBucket(job.Address);

            _executingJobs += weight;
            _nextHopJobs[nextHop] += weight;
            _deviceJobs[nextHop] += weight;
        }

        /// <summary>
        /// Sets a job as complete, updating the associated currently
        /// executing counts
        /// </summary>
        /// <param name="job">The job which has completed</param>
        private void _setAsComplete(JobInfo job)
        {
            int weight = job.Job.Weight;
            int nextHop = _getNextHopBucket(job.NextHop);
            int device = _getDeviceBucket(job.Address);

            _executingJobs -= weight;
            _nextHopJobs[nextHop] -= weight;
            _deviceJobs[nextHop] -= weight;
        }

        /// <summary>
        /// Schedules a new job for execution
        /// </summary>
        /// <param name="job">The job to schedule</param>
        public void Schedule(IJob job)
        {
            JobInfo info = new JobInfo(job);
            
            lock(_lock)
            {
                _jobs.AddLast(info);
            }
        }

        /// <summary>
        /// Resolves any process dependencies for this process
        /// </summary>
        /// <param name="processes">The available processes</param>
        public void Resolve(IEnumerable<IProcess> processes)
        {
            var host = processes.OfType<Host>().FirstOrDefault();
            var router = processes.OfType<Router>().FirstOrDefault();

            lock(_lock)
            {
                _host = host;
                _router = router;
            }
        }

        /// <summary>
        /// Tries to schedule a job
        /// </summary>
        /// <param name="job">The job to schedule</param>
        private void _trySchedule(JobInfo job)
        {
            if (job.Address == null)
            {
                if(_host != null)
                    _host.SearchForDevice(job.Job.Destination, this);
            }
            else if(job.NextHop.IsBroadcast())
            {
                if (_router != null)
                    _router.SearchForRoute(job.Address.Network, this);
            }
            else if(_canExecuteJob(job))
            {
            }
        }

        /// <summary>
        /// Called when a device has been found
        /// </summary>
        /// <param name="entry">The device table entry</param>
        void ISearchCallback<Recipient, DeviceTableEntry>.OnFound(DeviceTableEntry entry)
        {
            lock(_lock)
            {
                for(var node = _deviceSearches.First; node != null; node = node.Next)
                {
                    var info = node.Value;
                    var destination = info.Job.Destination;

                    if(destination.IsDevice && destination.AsDevice.Instance == entry.Instance)
                    {
                        info.Address = entry.Address;
                        _deviceSearches.Remove(node);
                        _routeSearches.AddLast(node);
                        _searchForRouter(info.Address);
                    }
                }
            }
        }

        /// <summary>
        /// Called when a device search has timed out
        /// </summary>
        void ISearchCallback<Recipient, DeviceTableEntry>.OnTimeout(Recipient key)
        {
            // needs support for cancelling a job
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
