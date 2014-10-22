using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Core.App.Messages;
using BACnet.Core.App.Transactions;
using BACnet.Core.Network;

namespace BACnet.Core.App
{
    public class TransactionManager
    {
        /// <summary>
        /// Lock used to synchronize access to the transaction manager
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// The host for this transaction manager
        /// </summary>
        private Host _host;

        /// <summary>
        /// The currently active client transactions
        /// </summary>
        private List<ClientTransaction> _clientTransactions;

        /// <summary>
        /// The last invoke id assigned
        /// </summary>
        private byte _lastInvokeId = 0;

        /// <summary>
        /// Constructs a new TransactionManager instance
        /// </summary>
        /// <param name="host">The host for this transaction manager</param>
        public TransactionManager(Host host)
        {
            this._host = host;
            this._clientTransactions = new List<ClientTransaction>();
        }

        /// <summary>
        /// Gets the next invoke id suitable for
        /// a client transaction
        /// </summary>
        /// <returns>The invoke id</returns>
        private byte _getClientInvokeId()
        {
            return ++_lastInvokeId;
        }

        /// <summary>
        /// Gets the client transaction that matches
        /// a remote address, invoke id pair
        /// </summary>
        /// <param name="address">The remote address</param>
        /// <param name="invokeId">The invoke id</param>
        /// <returns>The client transaction, or null of no transaction exists</returns>
        private ClientTransaction _getClientTransaction(Address address, byte invokeId)
        {
            for(int i = 0; i < _clientTransactions.Count; i++)
            {
                var tx = _clientTransactions[i];
                if (tx.Matches(address, invokeId))
                    return tx;
            }
            return null;
        }

        /// <summary>
        /// Sends a confirmed request
        /// </summary>
        /// <param name="destination">The destination address</param>
        /// <param name="serviceChoice">The service choice of the request</param>
        /// <param name="request">The request to send</param>
        /// <returns>The handle to the client transaction</returns>
        public IClientTransactionHandle SendConfirmedRequest(Address destination, byte serviceChoice, byte[] request)
        {
            lock(_lock)
            {
                byte invokeId = _getClientInvokeId();

                var transaction = new ClientTransaction(
                    _host,
                    this,
                    invokeId,
                    destination,
                    serviceChoice,
                    request);

                this._clientTransactions.Add(transaction);
                return transaction.GetHandle();
            }
        }

        /// <summary>
        /// Sends a confirmed request
        /// </summary>
        /// <param name="deviceInstance">The destination address</param>
        /// <param name="serviceChoice">The service choice of the request</param>
        /// <param name="request">The request to send</param>
        /// <returns>The handle to the client transaction</returns>
        public IClientTransactionHandle SendConfirmedRequest(uint deviceInstance, byte serviceChoice, byte[] request)
        {
            lock(_lock)
            {
                byte invokeId = _getClientInvokeId();

                var transaction = new ClientTransaction(
                    _host,
                    this,
                    invokeId,
                    deviceInstance,
                    serviceChoice,
                    request);

                this._clientTransactions.Add(transaction);
                return transaction.GetHandle();
            }
        }

        /// <summary>
        /// Processes a received confirmed request
        /// </summary>
        /// <param name="source">The address of the device that sent the request</param>
        /// <param name="message">The confirmed request header</param>
        /// <param name="segment">The buffer segment containing the request content</param>
        public void ProcessConfirmedRequest(Address source, ConfirmedRequestMessage message, BufferSegment segment)
        {
            
        }

        /// <summary>
        /// Processes a received simple ack
        /// </summary>
        /// <param name="source">The address of the device that sent the ack</param>
        /// <param name="message">The simple ack</param>
        public void ProcessSimpleAck(Address source, SimpleAckMessage message)
        {
            ClientTransaction tx = null;
            lock (_lock)
            {
                tx = _getClientTransaction(source, message.InvokeId);
            }
            if(tx != null)
                tx.OnSimpleAck(message);
        }

        /// <summary>
        /// Processes a received complex ack
        /// </summary>
        /// <param name="source">The address of the device that sent the ack</param>
        /// <param name="message">The complex ack header</param>
        /// <param name="segment">The buffer segment containing the ack content</param>
        public void ProcessComplexAck(Address source, ComplexAckMessage message, BufferSegment segment)
        {
            ClientTransaction tx = null;
            lock (_lock)
            {
                tx = _getClientTransaction(source, message.InvokeId);
            }
            if(tx != null)
                tx.OnComplexAck(message, segment);
        }

        /// <summary>
        /// Processes a received segment ack
        /// </summary>
        /// <param name="source">The address of the device that sent the ack</param>
        /// <param name="message">The segment ack</param>
        public void ProcessSegmentAck(Address source, SegmentAckMessage message)
        {
            if(message.Server)
            {
                ClientTransaction tx = null;
                lock (_lock)
                {
                    tx = _getClientTransaction(source, message.InvokeId);
                }
                if(tx != null)
                    tx.OnSegmentAck(message);
            }
        }

        /// <summary>
        /// Processes a received error
        /// </summary>
        /// <param name="source">The address of the device that sent the error</param>
        /// <param name="message">The error</param>
        public void ProcessError(Address source, ErrorMessage message)
        {
            ClientTransaction tx = null;
            lock (_lock)
            {
                tx = _getClientTransaction(source, message.InvokeId);
            }
            if(tx != null)
                tx.OnError(message);
        }

        /// <summary>
        /// Processes a received rejection
        /// </summary>
        /// <param name="source">The address of the device that sent the rejection</param>
        /// <param name="message">The rejection</param>
        public void ProcessReject(Address source, RejectMessage message)
        {
            ClientTransaction tx = null;
            lock (_lock)
            {
                tx = _getClientTransaction(source, message.InvokeId);
            }
            if(tx != null)
                tx.OnReject(message);
        }

        /// <summary>
        /// Processes a received abortion
        /// </summary>
        /// <param name="source">The address of the device that sent the abortion</param>
        /// <param name="message">The abortion</param>
        public void ProcessAbort(Address source, AbortMessage message)
        {
            if (message.Server)
            {
                ClientTransaction tx = null;
                lock(_lock)
                {
                    tx = _getClientTransaction(source, message.InvokeId);
                }
                if(tx != null)
                    tx.OnAbort(message);
            }
        }

        /// <summary>
        /// Disposes of a client transaction
        /// </summary>
        /// <param name="tx">The transaction to dispose</param>
        internal void DisposeTransaction(ClientTransaction tx)
        {
            lock(_lock)
            {
                _clientTransactions.Remove(tx);
                tx.Dispose();
            }
        }

    }
}
