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
    public class DefaultScheduler : IScheduler, IDeviceSearchCallback
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
        /// The number of next hop buckets
        /// </summary>
        public int NextHopBucketsPrime { get; private set; }

        /// <summary>
        /// The maximum number of concurrent requests per
        /// destination device
        /// </summary>
        public int MaxConcurrentJobsPerDevice { get; private set; }

        /// <summary>
        /// The number of device buckets
        /// </summary>
        public int DeviceBucketsPrime { get; private set; }

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
        /// The hash set of all the devices that are currently
        /// being searched for
        /// </summary>
        private HashSet<uint> _deviceSearchesActive;

        /// <summary>
        /// Jobs that are currently searching for device information
        /// </summary>
        private LinkedList<JobInfo> _deviceSearches;

        /// <summary>
        /// The hashset of all the networks that are currently
        /// being searched for
        /// </summary>
        private HashSet<ushort> _networkSearchesActive;

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
        /// <param name="maxConcurrentJobs">The maximum number of concurrent jobs to execute</param>
        /// <param name="maxConcurrentJobsPerNextHop">The maximum number of concurrent jobs per next hop device</param>
        /// <param name="nextHopBucketsPrime">The number of next hop buckets</param>
        /// <param name="maxConcurrentJobsPerDevice">The maximum number of concurrent jobs per device</param>
        /// <param name="deviceBucketsPrime">The number of device buckets</param>
        public DefaultScheduler(int maxConcurrentJobs, int maxConcurrentJobsPerNextHop, int nextHopBucketsPrime, int maxConcurrentJobsPerDevice, int deviceBucketsPrime)
        {
            this.MaxConcurrentJobs = maxConcurrentJobs;
            this.MaxConcurrentJobsPerNextHop = maxConcurrentJobsPerNextHop;
            this.NextHopBucketsPrime = nextHopBucketsPrime;
            this.MaxConcurrentJobsPerDevice = MaxConcurrentJobsPerDevice;
            this.DeviceBucketsPrime = DeviceBucketsPrime;

            _jobs = new LinkedList<JobInfo>();
            _deviceSearches = new LinkedList<JobInfo>();
            _routeSearches = new LinkedList<JobInfo>();

            this._executingJobs = 0;
            this._nextHopJobs = new int[NextHopBucketsPrime];
            this._deviceJobs = new int[DeviceBucketsPrime];
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
            int nextHop = job.NextHop.GetHashCode() % NextHopBucketsPrime;
            int device = job.Address.GetHashCode() % DeviceBucketsPrime;

            return
                (_executingJobs == 0 || _executingJobs + weight <= MaxConcurrentJobs) &&
                (_nextHopJobs[nextHop] == 0 || _nextHopJobs[nextHop] + weight <= MaxConcurrentJobsPerNextHop) &&
                (_deviceJobs[device] == 0 || _deviceJobs[device] + weight <= MaxConcurrentJobsPerDevice);
        }

        /// <summary>
        /// Sets a job as executing, updating the associated currently
        /// executing counts
        /// </summary>
        /// <param name="job">The job to execute</param>
        private void _setAsExecuting(JobInfo job)
        {
            int weight = job.Job.Weight;
            int nextHop = job.NextHop.GetHashCode() % NextHopBucketsPrime;
            int device = job.Address.GetHashCode() % DeviceBucketsPrime;

            _executingJobs += weight;
            _nextHopJobs[nextHop] += weight;
            _deviceJobs[nextHop] += weight;
        }

        /// <summary>
        /// Searches for a device, if a search for that device is not
        /// already active
        /// </summary>
        /// <param name="instance">The device instance to search for</param>
        private void _searchForDevice(uint instance)
        {
            if (_host == null || _deviceSearchesActive.Contains(instance))
                return;
            _deviceSearchesActive.Add(instance);
            _host.SearchForDevice(instance, this);
        }

        /// <summary>
        /// Searches for a route to a specific network, if a search for that
        /// network is not already active
        /// </summary>
        /// <param name="network">The network to search for</param>
        private void _searchForRoute(ushort network)
        {
            if (_router == null || _networkSearchesActive.Contains(network))
                return;
            _networkSearchesActive.Add(network);
            _router.SearchForRoute(network, this);
        }


        /// <summary>
        /// Sets a job as complete, updating the associated currently
        /// executing counts
        /// </summary>
        /// <param name="job">The job which has completed</param>
        private void _setAsComplete(JobInfo job)
        {
            int weight = job.Job.Weight;
            int nextHop = job.NextHop.GetHashCode() % NextHopBucketsPrime;
            int device = job.Address.GetHashCode() % DeviceBucketsPrime;

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
        /// Called when a device has been found
        /// </summary>
        /// <param name="entry">The device table entry</param>
        void IDeviceSearchCallback.DeviceFound(DeviceTableEntry entry)
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
        void IDeviceSearchCallback.DeviceSearchTimedOut()
        {
            // needs support for failing a job...
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
