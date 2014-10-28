using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Core.App.Messages;
using BACnet.Core.Exceptions;
using BACnet.Tagging;
using BACnet.Types;

namespace BACnet.Core.App.Transactions
{
    public class SimpleAckHandle : ClientTransactionHandle
    {
        /// <summary>
        /// The completion source for the ack task
        /// </summary>
        private TaskCompletionSource<bool> _source = new TaskCompletionSource<bool>();
        
        /// <summary>
        /// Notifies the handle that the transaction
        /// has been aborted
        /// </summary>
        /// <param name="reason">The abort reason</param>
        public override void FeedAbort(AbortReason reason)
        {
            _source.SetException(new AbortException(reason));
        }

        /// <summary>
        /// Notifies the handle that the transaction
        /// has been rejected
        /// </summary>
        /// <param name="reason">The reject reason</param>
        public override void FeedReject(RejectReason reason)
        {
            _source.SetException(new RejectException(reason));
        }

        /// <summary>
        /// Notifies the handle that the transaction
        /// has errored out
        /// </summary>
        /// <param name="error">The error that occured</param>
        public override void FeedError(ServiceError error)
        {
            _source.SetException(new ErrorException(error));
        }

        /// <summary>
        /// Notifies the handle that the transaction
        /// has completed and a simple ack has been received
        /// </summary>
        public override void FeedSimpleAck()
        {
            _source.SetResult(true);
        }

        /// <summary>
        /// Notifies the handle that a new response segment
        /// is available
        /// </summary>
        /// <param name="message">The complex ack message</param>
        /// <param name="segment">The response segment</param>
        public override void FeedComplexAck(ComplexAckMessage message, BufferSegment segment)
        {
            _source.SetException(new AbortException(AbortReason.InvalidApduInThisState));
        }

        /// <summary>
        /// Waits for the request to complete asynchronously
        /// </summary>
        /// <returns>The task for the request</returns>
        public Task WaitAsync()
        {
            return _source.Task;
        }

        /// <summary>
        /// Waits for the request to complete synchronously
        /// </summary>
        public void Wait()
        {
            _source.Task.Wait();
        }
    }
}
