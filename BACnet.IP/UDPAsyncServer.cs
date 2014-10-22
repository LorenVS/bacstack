using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.IP
{

    public delegate void DatagramReceivedDelegate(IPEndPoint ep, byte[] buffer, int length);

    public class UDPAsyncServer : IDisposable
    {
        /// <summary>
        /// Maximum length of a udp datagram
        /// </summary>
        public const int MaxDatagramLength = 1500;

        /// <summary>
        /// The lock object used to synchronize access to the
        /// UDP server
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// The underlying socket used to send and receive
        /// UDP datagrams
        /// </summary>
        private Socket _socket;

        /// <summary>
        /// The endpoint of the device being received
        /// from
        /// </summary>
        private EndPoint _remoteEP;

        /// <summary>
        /// Callback to invoke when a receive operation completes
        /// </summary>
        private readonly AsyncCallback _receiveCallback;

        /// <summary>
        /// Callback to invoke when a send operation completes
        /// </summary>
        private readonly AsyncCallback _sendCallback;

        /// <summary>
        /// Callback to user code whenever a datagram is received
        /// </summary>
        private readonly DatagramReceivedDelegate _receiveDelegate;

        /// <summary>
        /// Constructs a new UDPAsyncServer instance
        /// </summary>
        /// <param name="ep">The IP endpoint to bind to</param>
        /// <param name="receiveDelegate">Callback to user code whenever a datagram is received</param>
        public UDPAsyncServer(IPEndPoint ep, DatagramReceivedDelegate receiveDelegate)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.Bind(ep);
            _receiveCallback = new AsyncCallback(_receiveComplete);
            _sendCallback = new AsyncCallback(_sendComplete);
            _receiveDelegate = receiveDelegate;
            _receiveNext();
        }
        
        /// <summary>
        /// Disposes of all resources held
        /// by the UDPAsyncServer instance
        /// </summary>
        private void _disposeAll()
        {
            if(_socket != null)
            {
                _socket.Dispose();
                _socket = null;
            }
        }

        /// <summary>
        /// Begins receiving the next UDP datagram
        /// </summary>
        /// <returns>The async result</returns>
        private IAsyncResult _receiveNext()
        {
            _remoteEP = new IPEndPoint(IPAddress.Any, 0);

            byte[] buffer = new byte[MaxDatagramLength];

            return _socket.BeginReceiveFrom(
                buffer,
                0,
                MaxDatagramLength,
                SocketFlags.None,
                ref _remoteEP,
                _receiveCallback,
                buffer);
        }

        /// <summary>
        /// Called when an asynchronous receive operation completes
        /// </summary>
        /// <param name="result">The IAsyncResult of the asynchronous receive</param>
        private void _receiveComplete(IAsyncResult result)
        {
            if (result.CompletedSynchronously)
                return;

            bool received = false;
            IPEndPoint ep = null;
            int length = 0;
            byte[] buffer = (byte[])result.AsyncState;
            Queue<IAsyncResult> queue = null;

            while (result != null)
            {
                received = false;

                lock(_lock)
                {
                    if (_socket != null)
                    {
                        try
                        {
                            // complete this receive operation
                            length = _socket.EndReceiveFrom(result, ref _remoteEP);
                            ep = (IPEndPoint)_remoteEP;

                            // queue the next receive operation
                            IAsyncResult tempResult = _receiveNext();
                            if (tempResult.CompletedSynchronously)
                            {
                                if (queue == null)
                                    queue = new Queue<IAsyncResult>();
                                queue.Enqueue(tempResult);
                            }

                            received = true;
                        }
                        catch (SocketException)
                        {
                            _disposeAll();
                            break;
                        }
                    }

                }

                if (received)
                {
                    _receiveDelegate(ep, buffer, length);
                }

                result = (queue == null || queue.Count == 0) ? null : queue.Dequeue();
            }

        }

        /// <summary>
        /// Called when an asynchronous send operation completes
        /// </summary>
        /// <param name="result">The IAsyncResult instance</param>
        private void _sendComplete(IAsyncResult result)
        {
            lock(_lock)
            {
                if(_socket != null)
                {
                    try
                    {
                        _socket.EndSendTo(result);
                    }
                    catch(SocketException)
                    {
                        _disposeAll();
                    }
                }
            }
        }

        /// <summary>
        /// Disposes the UDPAsyncServer instance
        /// </summary>
        public void Dispose()
        {
            lock (_lock)
            {
                _disposeAll();
            }
        }

        /// <summary>
        /// Sends a datagram
        /// </summary>
        /// <param name="ep">The IPEndPoint of the destination device</param>
        /// <param name="buffer">The buffer containing the datagram to send</param>
        /// <param name="length">The length of the datagram to send</param>
        public void Send(IPEndPoint ep, byte[] buffer, int length)
        {
            lock(_lock)
            {
                _socket.BeginSendTo(buffer, 0, length, SocketFlags.None, ep, _sendCallback, null);
            }
        }


    }
}
