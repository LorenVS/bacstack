using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using BACnet.Core.Datalink;

namespace BACnet.Core
{
    public class Session : IDisposable
    {
        /// <summary>
        /// The processes in the session
        /// </summary>
        private readonly IProcess[] _processes;

        /// <summary>
        /// A multidictionary of message types to the processes
        /// which handle those messages
        /// </summary>
        private readonly Dictionary<Type, List<IProcess>> _messageHandlers;

        /// <summary>
        /// The queue of messages that need to be processed
        /// </summary>
        private readonly Queue<IMessage> _messageQueue;

        /// <summary>
        /// Threadpool callback for processing messages
        /// </summary>
        private readonly WaitCallback _processingCallback;

        /// <summary>
        /// The current message processing state of the session
        /// </summary>
        private ProcessingState _state = ProcessingState.Idle;
        
        /// <summary>
        /// Constructs a new session
        /// </summary>
        /// <param name="processes">The processes in the session</param>
        public Session(params IProcess[] processes)
        {
            Contract.Requires(processes != null);
            _processes = new IProcess[processes.Length];
            Array.Copy(processes, _processes, processes.Length);

            foreach (var process in _processes)
            {
                process.Session = this;
            }

            // create the message handlers dictionary
            _messageHandlers = new Dictionary<Type, List<IProcess>>();
            foreach(var process in _processes)
            {
                foreach(var messageType in process.MessageTypes)
                {
                    List<IProcess> handlers = null;
                    if(!_messageHandlers.TryGetValue(messageType, out handlers))
                    {
                        handlers = new List<IProcess>();
                        _messageHandlers.Add(messageType, handlers);
                    }
                    handlers.Add(process);
                }
            }

            _messageQueue = new Queue<IMessage>();
            _processingCallback = new WaitCallback(_processMessages);
            
            // open all of the ports first
            foreach (var p in _processes.OfType<IPort>())
            {
                p.Open();
            }

            // then resolve all of the process dependencies
            foreach (var process in _processes)
            {
                process.Resolve(processes);
            }
        }

        /// <summary>
        /// Queues a message for processing by the session
        /// </summary>
        /// <param name="message">The message to process</param>
        public void QueueMessage(IMessage message)
        {
            _messageQueue.Enqueue(message);
            if (_state == ProcessingState.Idle)
            {
                ThreadPool.QueueUserWorkItem(_processingCallback);
                _state = ProcessingState.ProcessingQueued;
            }
        }
        
        /// <summary>
        /// Function invoked on a thread pool thread to handle incoming messages
        /// </summary>
        /// <param name="nothing">The state, which is nothing</param>
        private void _processMessages(object nothing)
        {
            _state = ProcessingState.Processing;
            while (_messageQueue.Count > 0)
            {
                var message = _messageQueue.Dequeue();
                try
                {
                    _processMessage(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            _state = ProcessingState.Idle;
        }

        /// <summary>
        /// Handles a single queued message
        /// </summary>
        /// <param name="message">The message to handle</param>
        private void _processMessage(IMessage message)
        {
            Type messageType = message.GetType();
            List<IProcess> handlers = null;
            if (_messageHandlers.TryGetValue(messageType, out handlers))
            {
                foreach(var handler in handlers)
                {
                    handler.HandleMessage(message);
                }
            }
        }

        /// <summary>
        /// Disposes of the session
        /// </summary>
        public void Dispose()
        {
            foreach(var process in _processes)
            {
                process.Dispose();
            }
        }
        

        private enum ProcessingState
        {
            Processing,
            ProcessingQueued,
            Idle
        }
    }
}
