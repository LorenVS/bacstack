using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Explorer.Core.Models
{
    public class PortMapping : PropertyChangedBase
    {
        /// <summary>
        /// The network number of the mapping
        /// </summary>
        public ushort Network
        {
            get { return _network; }
            set { changeProperty(ref _network, value, "Network"); }
        }
        private ushort _network;

        /// <summary>
        /// The port id of the mapping
        /// </summary>
        public byte PortId
        {
            get { return _portId; }
            set { changeProperty(ref _portId, value, "PortId"); }
        }
        private byte _portId;
    }
}
