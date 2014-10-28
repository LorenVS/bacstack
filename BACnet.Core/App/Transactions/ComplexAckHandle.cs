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
    public class ComplexAckHandle<TAck> : ClientTransactionHandle
        where TAck : IComplexAck
    {
        /// <summary>
        /// The completion source for the ack task
        /// </summary>
        private TaskCompletionSource<TAck> _source = new TaskCompletionSource<TAck>();

        /// <summary>
        /// The list of segments received
        /// </summary>
        private List<BufferSegment> _buffers = new List<BufferSegment>();

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
            _source.SetException(new AbortException(AbortReason.InvalidApduInThisState));
        }

        /// <summary>
        /// Notifies the handle that a new response segment
        /// is available
        /// </summary>
        /// <param name="message">The complex ack message</param>
        /// <param name="segment">The response segment</param>
        public override void FeedComplexAck(ComplexAckMessage message, BufferSegment segment)
        {
            _buffers.Add(segment);

            if(!message.MoreFollows)
            {
                try
                {
                    // TODO: Is this the best place to do this? Decoding
                    // large requests could potentially be more time than
                    // we want to spent in the transaction's lock

                    using (var stream = new MultiBufferStream(_buffers))
                    {
                        var tagReader = new TagReader(stream);
                        var tagReaderStream = new TagReaderStream(tagReader, Value<TAck>.Schema);
                        var ack = Value<TAck>.Load(tagReaderStream);
                        _source.SetResult(ack);
                    }
                }
                catch(Exception e)
                {
                    _source.SetException(new RejectException(RejectReason.InvalidTag));
                }
            }
        }

        /// <summary>
        /// Retrieves the response asynchronously
        /// </summary>
        /// <returns>The task for the ack</returns>
        public Task<TAck> GetResponseAsync()
        {
            return _source.Task;
        }

        /// <summary>
        /// Retrieves the response synchronously
        /// </summary>
        /// <returns>The ack</returns>
        public TAck GetResponse()
        {
            return _source.Task.Result;
        }
    }
}
