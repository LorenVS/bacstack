using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto;
using Eto.Forms;
using BACnet.Ashrae.Objects;
using BACnet.Core.App;
using BACnet.Client;
using BACnet.Client.Descriptors;
using BACnet.Explorer.Core.Controls;
using BACnet.Explorer.Core.Extensibility;

namespace BACnet.Explorer.Core.Plugins.Tabs
{
    public class DeviceInfoTab : IDeviceTab
    {
        /// <summary>
        /// The order in which the tab should appear
        /// </summary>
        public int Order
        {
            get { return 10; }
        }

        /// <summary>
        /// Determines whether this page should be created for a device
        /// </summary>
        /// <param name="deviceInfo">The device info</param>
        /// <returns>True if the page should be created, false otherwise</returns>
        public bool Active(ObjectInfo deviceInfo)
        {
            return true;
        }

        /// <summary>
        /// Creates the tab page
        /// </summary>
        /// <param name="deviceInfo">The device info to create the tab for</param>
        /// <returns>The tab page instance</returns>
        public TabPage Create(ObjectInfo deviceInfo)
        {
            var page = new TabPage();
            page.Text = Constants.DeviceInfoTabText;
            page.Content = new Panel(deviceInfo);
            return page;
        }

        public class Panel : BACnetPanel2
        {
            /// <summary>
            /// Constructs a new device info tab panel
            /// </summary>
            /// <param name="info">The device to show information for</param>
            public Panel(ObjectInfo info)
            {
                var obj = client.With<IDevice>(
                    info.DeviceInstance,
                    info.ObjectIdentifier);

                var form = new FormBuilder()
                    .AddGroup(Constants.CorePropertiesHeader)
                        .AddRow(
                            createLabel(Constants.ObjectNameLabel),
                            bindEditor(obj, dev => dev.ObjectName, enabled: false))
                        .AddRow(
                            createLabel(Constants.VendorIdLabel),
                            bindEditor(obj, dev => dev.VendorIdentifier, enabled: false))
                        .AddRow(
                            createLabel(Constants.VendorNameLabel),
                            bindEditor(obj, dev => dev.VendorName, enabled: false)
                        )
                        .AddRow(
                            createLabel(Constants.ModelNameLabel),
                            bindEditor(obj, dev => dev.ModelName, enabled: false)
                        )
                        .AddRow(
                            createLabel(Constants.ApplicationSoftwareVersionLabel),
                            bindEditor(obj, dev => dev.ApplicationSoftwareVersion, enabled: false))
                        .End()
                    .End();

                this.Content = form.Root;
            }

        }
    }
}
