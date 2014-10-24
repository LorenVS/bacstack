using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;
using BACnet.Core;
using BACnet.Core.Datalink;

namespace BACnet.Ethernet
{
    public class EthernetPort : IPort
    {
        /// <summary>
        /// The broadcast mac address for ethernet
        /// </summary>
        private static readonly Mac _ethernetBroadcastMac = new Mac(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });

        /// <summary>
        /// The minimum packet length
        /// </summary>
        private const int _minimumPacketLength = 16;

        /// <summary>
        /// The port id of the port
        /// </summary>
        public byte PortId { get { return _options.PortId; } }

        /// <summary>
        /// The process id of the port
        /// </summary>
        public int ProcessId { get { return _options.ProcessId; } }

        /// <summary>
        /// The lock synchronizing access to the port
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// Whether or not the port has been disposed
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// The port options
        /// </summary>
        private EthernetPortOptions _options;

        /// <summary>
        /// The capture device to use
        /// </summary>
        private ICaptureDevice _device;

        /// <summary>
        /// The mac address bytes for the capture device
        /// </summary>
        private byte[] _deviceMac;

        /// <summary>
        /// The observers that are subscribed to inbound netgrams
        /// </summary>
        private SubscriptionList<InboundNetgram> _observers;

        /// <summary>
        /// Creates a new ethernet port instance
        /// </summary>
        /// <param name="options">The options for the port</param>
        public EthernetPort(EthernetPortOptions options)
        {
            this._options = options.Clone();
            this._device = _getCaptureDevice();
            this._device.OnPacketArrival += _onPacketArrival;
            this._observers = new SubscriptionList<InboundNetgram>();
        }
        

        /// <summary>
        /// Retrieves the capture device that is used by this port
        /// </summary>
        /// <returns>The capture device instance</returns>
        private ICaptureDevice _getCaptureDevice()
        {
            var devices = CaptureDeviceList.Instance;
            bool found = false;

            if(_options.DeviceName != null)
                return devices.Where(dev => dev.Name == _options.DeviceName).FirstOrDefault();
            else
            {
                foreach(var device in devices)
                {
                    device.Open();

                    try
                    {
                        found = (device.LinkType == PacketDotNet.LinkLayers.Ethernet
                            && device.MacAddress != null);
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                        device.Close();
                    }

                    if (found)
                        return device;
                }

                return null;
            }
        }

        /// <summary>
        /// Opens the port
        /// </summary>
        public void Open()
        {
            lock(_lock)
            {
                _device.Open();

                // filter to only bacnet packets
                _device.Filter = "ether proto 0x82";
                _device.StartCapture();

                this._deviceMac = _device.MacAddress.GetAddressBytes();
            }
        }

        /// <summary>
        /// Determines whether a packet contains an outgoing 
        /// packet sent by this device
        /// </summary>
        /// <param name="buffer">The buffer to check</param>
        /// <returns>True if the packet was sent by this device, false otherwise</returns>
        private bool _isOutboundPacket(byte[] buffer)
        {
            // check to see if the source mac 100%
            // matches the device mac address of the local device

            for(int i = 0; i < 6; i++)
            {
                if (buffer[6 + i] != _deviceMac[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Called whenever a packet is captured on the capture device
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event args</param>
        private void _onPacketArrival(object sender, CaptureEventArgs e)
        {
            // don't process any packet too short to not be valid
            if (e.Packet.Data.Length < _minimumPacketLength)
                return;

            // don't process any packets sent by the local device
            //if (_isOutboundPacket(e.Packet.Data))
            //    return;

            byte[] buffer = e.Packet.Data;
            int offset = 0;
            Mac destination, source;
            int length;
            byte dsap, ssap, control;

            destination = new Mac(buffer, offset, 6);
            offset += 6;
            source = new Mac(buffer, offset, 6);
            offset += 6;
            length = buffer.ReadUInt16(offset);
            offset += 2;

            dsap = buffer[offset++];
            ssap = buffer[offset++];
            control = buffer[offset++];

            // don't process non-BACnet packets
            if (dsap != 0x82 || ssap != 0x82 || control != 0x03)
                return;

            InboundNetgram netgram = new InboundNetgram(this);
            netgram.Segment = new BufferSegment(buffer, offset, buffer.Length);
            netgram.Source = source;
            _observers.Next(netgram);
        }

        /// <summary>
        /// Sends a netgram out of this port
        /// </summary>
        /// <param name="netgram">The netgram to send</param>
        public void SendNetgram(OutboundNetgram netgram)
        {
            Contract.Requires(_deviceMac.Length == 6);
            Contract.Requires(netgram.Destination.IsBroadcast() || netgram.Destination.Length == 6);

            byte[] buffer = new byte[1500];
            int lengthOffset = 0;
            int offset = 0;
            int length;
            var destination = netgram.Destination.IsBroadcast() ? _ethernetBroadcastMac : netgram.Destination;
            
            // write the destination mac address bytes
            for(int i = 0; i < 6; i++)
            {
                buffer[offset++] = destination[i];
            }

            // write the source mac address bytes
            for(int i = 0; i < 6; i++)
            {
                buffer[offset++] = _deviceMac[i];
            }

            // the next 2 bytes are used for the packet length, so
            // we skip them until we know what they are
            lengthOffset = offset;
            offset += 2;

            // DSAP and SSAP
            buffer[offset++] = 0x82;
            buffer[offset++] = 0x82;

            // LLC control
            buffer[offset++] = 0x03;

            // serialize the netgram content
            offset = netgram.Content.Serialize(buffer, offset);

            // now that we have the full packet length, we backfill
            // the length field
            length = offset - lengthOffset - 2;
            buffer.WriteUInt16(lengthOffset, (ushort)length);

            lock(_device)
            {
                _device.SendPacket(buffer, offset);
            }

        }

        /// <summary>
        /// Subscribes to netgrams received by the port
        /// </summary>
        /// <param name="observer">The observer</param>
        /// <returns>The disposable instance for the subscription</returns>
        public IDisposable Subscribe(IObserver<InboundNetgram> observer)
        {
            _observers.Register(observer);
            return new NetgramSubscription(this, observer);
        }

        /// <summary>
        /// Unsubscribes an observer from receiving netgram notifications
        /// </summary>
        /// <param name="observer">The observer to unsubscribe</param>
        private void _unsubscribe(IObserver<InboundNetgram> observer)
        {
            var observers = _observers;
            if(observers != null)
                observers.Unregister(observer);
        }

        /// <summary>
        /// Resolves the dependencies of this port
        /// </summary>
        /// <param name="processes">The available processes</param>
        public void Resolve(IEnumerable<IProcess> processes)
        {
        }

        /// <summary>
        /// Disposes of the all resources held by the ethernet port
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void dispose(bool disposing)
        {
            if (!_disposed)
            {
                try
                {
                    if (disposing)
                    {
                        if(_observers != null)
                        {
                            _observers.Dispose();
                            _observers = null;
                        }

                        if(_device != null)
                        {
                            _device.StopCapture();
                            _device.Close();
                        }
                    }
                }
                finally
                {
                    _disposed = true;
                }
            }
        }

        /// <summary>
        /// Disposes of all resources held by the ethernet port
        /// </summary>
        public void Dispose()
        {
            dispose(true);
        }

        private class NetgramSubscription : IDisposable
        {
            private EthernetPort _port;
            private IObserver<InboundNetgram> _observer;

            public NetgramSubscription(EthernetPort port, IObserver<InboundNetgram> observer)
            {
                this._port = port;
                this._observer = observer;
            }

            public void Dispose()
            {
                _port._unsubscribe(_observer);
            }
        }

    }
}
