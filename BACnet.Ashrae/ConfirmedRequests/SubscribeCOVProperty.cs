using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class SubscribeCOVPropertyRequest : IConfirmedRequest
    {
        /// <summary>
        /// The service choice for subscribe cov property requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.SubscribeCOVProperty; } }
    }
}
