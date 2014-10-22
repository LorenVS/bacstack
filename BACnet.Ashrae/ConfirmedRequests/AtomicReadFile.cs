using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class AtomicReadFileRequest : IConfirmedRequest
    {
        /// <summary>
        /// The confirmed service choice for atomic read file requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.AtomicReadFile; } }
    }

    public partial class AtomicReadFileAck : IComplexAck
    {
        /// <summary>
        /// The confirmed service choice for atomic read file acks
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.AtomicReadFile; } }
    }
}
