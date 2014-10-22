using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class GetEnrollmentSummaryRequest : IConfirmedRequest
    {
        /// <summary>
        /// The service choice for get enrollment summary requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.GetEnrollmentSummary; } }
    }

    public partial class GetEnrollmentSummaryAck : IComplexAck
    {
        /// <summary>
        /// The service choice for get enrollment summary acks
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.GetEnrollmentSummary; } }
    }
}
