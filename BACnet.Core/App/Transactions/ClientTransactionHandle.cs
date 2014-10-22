using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Core.App.Messages;
using BACnet.Core.Exceptions;

namespace BACnet.Core.App.Transactions
{
    /// <summary>
    /// A handle that allows the requester to control
    /// the transaction
    /// </summary>
    internal class ClientTransactionHandle : IClientTransactionHandle
    {
        private readonly object _lock = new object();
        private ClientTransaction _transaction;
        private Queue<BufferSegment> _segments;
        private bool _finished;
        private bool _isComplexAck;
        private bool _isSimpleAck;
        private ServiceError _error;
        private AbortReason? _abortReason;
        private RejectReason? _rejectReason;

        /// <summary>
        /// Constructs a new Handle instance
        /// </summary>
        /// <param name="transaction">The transaction that the handle is for</param>
        public ClientTransactionHandle(ClientTransaction transaction)
        {
            this._transaction = transaction;
            this._segments = new Queue<BufferSegment>();
        }

        /// <summary>
        /// Feeds a simple ack to the handle
        /// </summary>
        /// <param name="message">The simple ack message</param>
        internal void FeedSimpleAck(SimpleAckMessage message)
        {
            lock (_lock)
            {
                _isSimpleAck = true;
                Monitor.PulseAll(_lock);
            }
        }

        /// <summary>
        /// Feeds a complex ack to the handle
        /// </summary>
        /// <param name="message">The complex ack message</param>
        internal void FeedComplexAck(ComplexAckMessage message, BufferSegment segment)
        {
            lock (_lock)
            {
                _isComplexAck = true;
                _segments.Enqueue(segment);
                if (!message.MoreFollows)
                    _finished = true;
                Monitor.PulseAll(_lock);
            }
        }

        /// <summary>
        /// Feeds an error to the handle
        /// </summary>
        /// <param name="message">The error</param>
        internal void FeedError(ServiceError error)
        {
            lock (_lock)
            {
                _error = error;
                Monitor.PulseAll(_lock);
            }
        }

        /// <summary>
        /// Feeds a rejection to the handle
        /// </summary>
        /// <param name="message">The reject reason</param>
        internal void FeedReject(RejectReason reason)
        {
            lock (_lock)
            {
                _rejectReason = reason;
                Monitor.PulseAll(_lock);
            }
        }

        /// <summary>
        /// Feeds an abortion to the handle
        /// </summary>
        /// <param name="message">The abort reason</param>
        internal void FeedAbort(AbortReason reason)
        {
            lock (_lock)
            {
                _abortReason = reason;
                Monitor.PulseAll(_lock);
            }
        }

        /// <summary>
        /// Aborts the transaction
        /// </summary>
        public void Abort(AbortReason reason)
        {
            this._transaction.Abort(reason);
        }

        /// <summary>
        /// Gets the result of the transaction
        /// </summary>
        /// <returns>The transaction stream</returns>
        public Stream GetResponse()
        {
            lock (_lock)
            {
                while (true)
                {
                    if (_error != null)
                        throw new ErrorException(this._error);
                    else if (_rejectReason != null)
                        throw new RejectException(this._rejectReason.Value);
                    else if (_abortReason != null)
                       throw new AbortException(this._abortReason.Value);
                    else if (_isComplexAck)
                        return new ResponseStream(this);
                    else if (_isSimpleAck)
                        return null;

                    Monitor.Wait(_lock);
                }
            }
        }

        private class ResponseStream : Stream
        {
            /// <summary>
            /// The transaction handle
            /// </summary>
            private ClientTransactionHandle _handle;

            /// <summary>
            /// The current buffer segment
            /// </summary>
            private BufferSegment _buffer;

            /// <summary>
            /// Constructs a new ResponseStream instance
            /// </summary>
            /// <param name="handle">The handle for the transaction</param>
            public ResponseStream(ClientTransactionHandle handle)
            {
                this._handle = handle;
            }

            #region Invalid Operations

            public override bool CanRead
            {
                get
                {
                    return true;
                }
            }

            public override bool CanSeek
            {
                get
                {
                    return false;
                }
            }

            public override bool CanWrite
            {
                get
                {
                    return false;
                }
            }

            public override long Length
            {
                get
                {
                    throw new InvalidOperationException();
                }
            }

            public override long Position
            {
                get
                {
                    throw new InvalidOperationException();
                }

                set
                {
                    throw new InvalidOperationException();
                }
            }

            public override void Flush()
            {
                throw new InvalidOperationException();
            }

            public override void SetLength(long value)
            {
                throw new InvalidOperationException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new InvalidOperationException();
            }

            #endregion

            /// <summary>
            /// Blocks for the next segment to be availble
            /// </summary>
            /// <returns>True if there was another segment, false otherwise</returns>
            private bool _getNextBuffer()
            {
                lock (_handle._lock)
                {
                    while (true)
                    {
                        if (_handle._abortReason != null)
                            throw new AbortException(_handle._abortReason.Value);
                        if (_handle._rejectReason != null)
                            throw new RejectException(_handle._rejectReason.Value);
                        if (_handle._error != null)
                            throw new ErrorException(_handle._error);

                        if (_handle._segments.Count > 0)
                        {
                            _buffer = _handle._segments.Dequeue();
                            return true;
                        }
                        else if (_handle._finished)
                            return false;

                        Monitor.Wait(_handle._lock);
                    }
                }
            }

            /// <summary>
            /// Seeds to a certain position 
            /// </summary>
            /// <param name="offset"></param>
            /// <param name="origin"></param>
            /// <returns></returns>
            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new InvalidOperationException();
            }

            /// <summary>
            /// Reads bytes from the transaction's response
            /// </summary>
            /// <param name="buffer">The buffer to read the bytes into</param>
            /// <param name="offset">The offset in the buffer to begin reading</param>
            /// <param name="count">The number of bytes to read</param>
            /// <returns>The number of bytes read</returns>
            public override int Read(byte[] buffer, int offset, int count)
            {
                int read = 0;

                while (count > 0)
                {
                    var segment = _buffer;

                    if (segment.Buffer != null)
                    {
                        int bytes = Math.Min(segment.End - segment.Offset, count);
                        Array.Copy(segment.Buffer, segment.Offset, buffer, offset, bytes);
                        _buffer = new BufferSegment(segment.Buffer, segment.Offset + bytes, segment.End);
                        count -= bytes;
                        offset += bytes;
                        read += bytes;
                    }

                    if (count > 0)
                    {
                        if (!_getNextBuffer())
                            break;
                    }
                }

                return read;
            }
        }
    }
}
