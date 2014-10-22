using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Core;
using BACnet.IP;

namespace BACnet.Explorer.Core.Models
{
    public class ForeignDevicePortProcess : Process
    {
        /// <summary>
        /// The unique id of the port
        /// </summary>
        public byte PortId
        {
            get { return _portId; }
            set { changeProperty(ref _portId, value, "PortId"); }
        }
        private byte _portId;

        /// <summary>
        /// The local hostname to bind to
        /// </summary>
        public string LocalHost
        {
            get { return _localHost; }
            set { changeProperty(ref _localHost, value, "LocalHost"); }
        }
        private string _localHost;

        /// <summary>
        /// The local port to bind to
        /// </summary>
        public ushort LocalPort
        {
            get { return _localPort; }
            set { changeProperty(ref _localPort, value, "LocalPort"); }
        }
        private ushort _localPort;

        /// <summary>
        /// The bbmd host to communicate with
        /// </summary>
        public string BbmdHost
        {
            get { return _bbmdHost; }
            set { changeProperty(ref _bbmdHost, value, "BbmdHost"); }
        }
        private string _bbmdHost;

        /// <summary>
        /// The bbmd udp port to communicate with
        /// </summary>
        public ushort BbmdPort
        {
            get { return _bbmdPort; }
            set { changeProperty(ref _bbmdPort, value, "BbmdPort"); }
        }
        private ushort _bbmdPort;

        /// <summary>
        /// The interval at which the foreign device should reregister
        /// with the bbmd
        /// </summary>
        public TimeSpan RegistrationInterval
        {
            get { return _registrationInterval; }
            set { changeProperty(ref _registrationInterval, value, "RegistrationInterval"); }
        }
        private TimeSpan _registrationInterval;


        public ForeignDevicePortProcess()
        {
            this.Name = Constants.ForeignDevicePortDefaultName;
            this.ProcessId = BACnet.IP.DefaultProcessIds.ForeignDevicePort;

            _portId = 1;
            _localHost = "0.0.0.0";
            _localPort = 47808;
            _bbmdHost = string.Empty;
            _bbmdPort = 47808;
            _registrationInterval = TimeSpan.FromSeconds(240);
        }

        /// <summary>
        /// Creates a process options instance
        /// </summary>
        /// <returns>The process options instance</returns>
        public override IProcessOptions CreateOptions()
        {
            return new ForeignDevicePortOptions()
            {
                ProcessId = this.ProcessId,
                PortId = this.PortId,
                LocalHost = this.LocalHost,
                LocalPort = this.LocalPort,
                BbmdHost = this.BbmdHost,
                BbmdPort = this.BbmdPort,
                RegistrationInterval = this.RegistrationInterval
            };
        }
    }
}
