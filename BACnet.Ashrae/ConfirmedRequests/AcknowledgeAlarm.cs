using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class AcknowledgeAlarmRequest : IConfirmedRequest
    {
        /// <summary>
        /// The confirmed service choice for acknowledge alarm requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.AcknowledgeAlarm; } }
    }
}
