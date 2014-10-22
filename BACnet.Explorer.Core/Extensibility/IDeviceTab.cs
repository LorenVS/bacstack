using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto;
using Eto.Forms;
using BACnet.Client;
using BACnet.Client.Descriptors;

namespace BACnet.Explorer.Core.Extensibility
{
    public interface IDeviceTab
    {
        /// <summary>
        /// The order in which the tabs should appear
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Determines whether this device tab
        /// should be created for a specific device
        /// </summary>
        /// <param name="deviceInfo">The device info of the device</param>
        /// <returns>True if the tab should be created, false otherwise</returns>
        bool Active(ObjectInfo deviceInfo);

        /// <summary>
        /// Creates the tab page
        /// </summary>
        /// <param name="deviceInfo">The device to create the page for</param>
        /// <returns>The tab page</returns>
        TabPage Create(ObjectInfo deviceInfo);
    }
}
