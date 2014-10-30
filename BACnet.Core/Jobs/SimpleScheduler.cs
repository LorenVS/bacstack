using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Jobs
{
    public class SimpleScheduler : IScheduler
    {
        /// <summary>
        /// The lock synchronizing access to the scheduler
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// The maximum number of concurrent jobs to execute
        /// </summary>
        public int MaxConcurrentJobs { get; private set; }
        
        /// <summary>
        /// The jobs that are queued for execution
        /// </summary>
        private Queue<IJob> _queuedJobs;

        /// <summary>
        /// The number of jobs that are currently executing
        /// </summary>
        private int _executingCount;

        /// <summary>
        /// The jobs invocations that are currently executing
        /// </summary>
        private Invocation[] _executingJobs;

        /// <summary>
        /// Constructs a new simple scheduler instance
        /// </summary>
        public SimpleScheduler(int maxConcurrentJobs)
        {
            this.MaxConcurrentJobs = maxConcurrentJobs;
            this._queuedJobs = new Queue<IJob>();
            this._executingCount = 0;
            this._executingJobs = new Invocation[MaxConcurrentJobs];
        }

        /// <summary>
        /// Called whenever a job completes
        /// </summary>
        /// <param name="invocation">The job invocation</param>
        private void _jobComplete(Invocation invocation)
        {
            lock(_lock)
            {
                for(int i = 0; i < _executingJobs.Length; i++)
                {
                    if (Object.ReferenceEquals(_executingJobs[i], invocation))
                    {
                        _executingJobs[i] = null;
                        _executingCount--;
                        break;
                    }
                }
            }

            _executeNextJobs();
        }

        /// <summary>
        /// Executes next jobs
        /// </summary>
        private void _executeNextJobs()
        {
            Invocation[] newInvocations = new Invocation[MaxConcurrentJobs];
            int newCount = 0;

            lock(_lock)
            {
                while(_executingCount < MaxConcurrentJobs && _queuedJobs.Count > 0)
                {
                    IJob job = _queuedJobs.Dequeue();
                    int index = _findNullIndex();
                    Invocation invocation = new Invocation(this, job);
                    newInvocations[newCount++] = invocation;
                    _executingCount++;                    
                }
            }

            for(int i = 0; i < newCount; i++)
            {
                newInvocations[i].Job.Execute(newInvocations[i]);
            }
        }

        /// <summary>
        /// Finds a null index in the executing jobs array
        /// </summary>
        /// <returns>The index</returns>
        private int _findNullIndex()
        {
            for(int i = 0; i < _executingJobs.Length; i++)
            {
                if (_executingJobs[i] == null)
                    return i;
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Schedules a new job for execution
        /// </summary>
        /// <param name="job">The job to schedule</param>
        public void Schedule(IJob job)
        {
            lock(_lock)
            {
                _queuedJobs.Enqueue(job);
            }

            _executeNextJobs();
        }

        private class Invocation : IJobInvocation
        {
            /// <summary>
            /// The scheduler instance
            /// </summary>
            public SimpleScheduler Scheduler { get; private set; }
            IScheduler IJobInvocation.Scheduler { get { return Scheduler; } }

            /// <summary>
            /// The job being executed
            /// </summary>
            public IJob Job { get; private set; }

            /// <summary>
            /// Constructs a new invocation instance
            /// </summary>
            /// <param name="scheduler">The scheduler instance</param>
            /// <param name="job">The job being executed</param>
            public Invocation(SimpleScheduler scheduler, IJob job)
            {
                this.Scheduler = scheduler;
                this.Job = job;
            }

            /// <summary>
            /// Called when the job completes
            /// </summary>
            public void Complete()
            {
                Scheduler._jobComplete(this);
            }
        }
    }
}
