using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Datalink
{
    public interface IPort : IObservable<InboundNetgram>, IProcess
    {
        /// <summary>
        /// The port id of the port
        /// </summary>
        byte PortId { get; }

        /// <summary>
        /// Opens the port
        /// </summary>
        void Open();

        /// <summary>
        /// Sends a netgram out of this port
        /// </summary>
        /// <param name="netgram">The netgram to send</param>
        void SendNetgram(OutboundNetgram netgram);
    }
}
