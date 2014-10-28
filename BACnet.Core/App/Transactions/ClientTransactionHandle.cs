using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Core.App.Messages;

namespace BACnet.Core.App.Transactions
{
    public abstract class ClientTransactionHandle
    {
        /// <summary>
        /// Lock synchronizing access to the handle
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// The underlying transaction
        /// </summary>
        private ClientTransaction _transaction;

        /// <summary>
        /// The reason the transaction is aborted
        /// </summary>
        private AbortReason? _abortReason;

        /// <summary>
        /// Sets the underlying transaction
        /// </summary>
        /// <param name="transaction">The transaction instance</param>
        internal void SetTransaction(ClientTransaction transaction)
        {
            lock(_lock)
            {
                _transaction = transaction;
                if (_abortReason != null)
                    _transaction.Abort(_abortReason.Value);
            }
        }

        /// <summary>
        /// Aborts the underlying transaction
        /// </summary>
        /// <param name="reason">The reason for the abort</param>
        public void Abort(AbortReason reason)
        {
            lock(_lock)
            {
                if (_transaction != null)
                    _abortReason = reason;
                else
                    _transaction.Abort(reason);
            }
        }

        /// <summary>
        /// Notifies the handle that the transaction
        /// has been aborted
        /// </summary>
        /// <param name="reason">The abort reason</param>
        public abstract void FeedAbort(AbortReason reason);

        /// <summary>
        /// Notifies the handle that the transaction
        /// has been rejected
        /// </summary>
        /// <param name="reason">The reject reason</param>
        public abstract void FeedReject(RejectReason reason);

        /// <summary>
        /// Notifies the handle that the transaction
        /// has errored out
        /// </summary>
        /// <param name="error">The error that occured</param>
        public abstract void FeedError(ServiceError error);

        /// <summary>
        /// Notifies the handle that the transaction
        /// has completed and a simple ack has been received
        /// </summary>
        public abstract void FeedSimpleAck();

        /// <summary>
        /// Notifies the handle that a new response segment
        /// is available
        /// </summary>
        /// <param name="message">The complex ack message</param>
        /// <param name="segment">The response segment</param>
        public abstract void FeedComplexAck(ComplexAckMessage message, BufferSegment segment);
    }
}
