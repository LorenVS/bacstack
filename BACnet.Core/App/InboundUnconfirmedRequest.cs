using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Core.Network;

namespace BACnet.Core.App
{
    public class InboundUnconfirmedRequest
    {
        /// <summary>
        /// The address of the device that sent the
        /// request
        /// </summary>
        public Address SourceAddress { get; private set; }

        /// <summary>
        /// The device table entry of the device that sent
        /// the request, may be null
        /// </summary>
        public DeviceTableEntry Source { get; private set; }

        /// <summary>
        /// The unconfirmed request
        /// </summary>
        public IUnconfirmedRequest Request { get; private set; }

        /// <summary>
        /// Constructs a new inbound unconfirmed request
        /// </summary>
        /// <param name="sourceAddress">The device that sent the request</param>
        /// <param name="source">The device table entry of the device that sent the request</param>
        /// <param name="request">The request instance</param>
        public InboundUnconfirmedRequest(Address sourceAddress, DeviceTableEntry source, IUnconfirmedRequest request)
        {
            this.SourceAddress = sourceAddress;
            this.Source = source;
            this.Request = request;
        }
    }
}
