using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
    public class GetAlarmSummaryRequest : IConfirmedRequest
    {
        public static readonly ISchema Schema = new SequenceSchema(false);

        public static GetAlarmSummaryRequest Load(IValueStream stream)
        {
            return new GetAlarmSummaryRequest();
        }

        public static void Save(IValueSink sink, GetAlarmSummaryRequest request)
        {
        }

        /// <summary>
        /// The service choice for get alarm summary requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.GetAlarmSummary; } }
    }

    public partial class GetAlarmSummaryAck : IComplexAck
    {
        /// <summary>
        /// The service choice for get alarm summary acks
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.GetAlarmSummary; } }
    }
}
