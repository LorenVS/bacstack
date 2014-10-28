using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Core;
using BACnet.Ethernet;

namespace BACnet.Explorer.Core.Models
{
    public class EthernetPortProcess : Process
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
        /// The device name of the ethernet interface
        /// </summary>
        public string DeviceName
        {
            get { return _deviceName; }
            set { changeProperty(ref _deviceName, value, "DeviceName"); }
        }
        private string _deviceName;


        public EthernetPortProcess()
        {
            this.Name = Constants.EthernetPortDefaultName;
            this.ProcessId = BACnet.Ethernet.DefaultProcessIds.EthernetPort;

            var device = SharpPcap.CaptureDeviceList.Instance.FirstOrDefault();
            if (device != null)
                this.DeviceName = device.Name;
        }

        /// <summary>
        /// Creates a process options instance
        /// </summary>
        /// <returns>The process options instance</returns>
        public override IProcessOptions CreateOptions()
        {
            return new EthernetPortOptions()
            {
                ProcessId = this.ProcessId,
                PortId = this.PortId,
                DeviceName = this.DeviceName
            };
        }
    }
}
