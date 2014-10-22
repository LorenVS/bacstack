using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class AtomicWriteFileRequest : IConfirmedRequest
    {
        /// <summary>
        /// The confirmed service choice for atomic write file requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.AtomicWriteFile; } }
    }

    public partial class AtomicWriteFileAck : IComplexAck
    {
        /// <summary>
        /// The confirmed service choice for atomic write file acks
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.AtomicWriteFile; } }
    }
}
