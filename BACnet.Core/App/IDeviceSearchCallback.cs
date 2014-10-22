using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.App
{
    public interface IDeviceSearchCallback
    {
        /// <summary>
        /// Called when the requested device has been found
        /// </summary>
        /// <param name="entry">The device table entry of the device</param>
        void DeviceFound(DeviceTableEntry entry);

        /// <summary>
        /// Called when the device search times out
        /// </summary>
        void DeviceSearchTimedOut();
    }
}
