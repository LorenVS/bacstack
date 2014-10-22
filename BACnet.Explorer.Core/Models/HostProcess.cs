using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Core;
using BACnet.Core.App;
using BACnet.Types;

namespace BACnet.Explorer.Core.Models
{
    public class HostProcess : Process
    {
        public HostProcess()
        {
            this.Name = Constants.HostDefaultName;
            this.ProcessId = BACnet.Core.DefaultProcessIds.Host;
        }

        /// <summary>
        /// Creates a process options instance
        /// </summary>
        /// <returns>The process options instance</returns>
        public override IProcessOptions CreateOptions()
        {
            var opts = new HostOptions()
            {
                ProcessId = this.ProcessId
            };

            // TODO: better way of registering these?

            var unconfirmed = opts.UnconfirmedRegistrar;
            unconfirmed.Register<IAmRequest>(UnconfirmedServiceChoice.IAm);
            unconfirmed.Register<WhoIsRequest>(UnconfirmedServiceChoice.WhoIs);

            var confirmed = opts.ConfirmedRegistrar;
            confirmed.Register<ReadPropertyRequest>(ConfirmedServiceChoice.ReadProperty);
            confirmed.Register<ReadPropertyMultipleRequest>(ConfirmedServiceChoice.ReadPropertyMultiple);
            confirmed.Register<ReadRangeRequest>(ConfirmedServiceChoice.ReadRange);
            confirmed.Register<GetAlarmSummaryRequest>(ConfirmedServiceChoice.GetAlarmSummary);

            return opts;
        }
    }
}
