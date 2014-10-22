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
    public class DeviceAlarmsTab : IDeviceTab
    {
        /// <summary>
        /// The order in which the tab should appear
        /// </summary>
        public int Order
        {
            get { return 20; }
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
            page.Text = Constants.AlarmsTabText;
            page.Content = new Panel(deviceInfo);
            return page;
        }

        public class Panel : BACnetPanel
        {
            /// <summary>
            /// The device that the panel is showing information for
            /// </summary>
            private ObjectInfo _device;

            private GridView _gridView;

            /// <summary>
            /// Constructs a new device info tab panel
            /// </summary>
            /// <param name="device">The device to show information for</param>
            public Panel(ObjectInfo device)
            {
                this._device = device;
                var layout = new DynamicLayout();
                this.Content = layout;

                _gridView = new GridView();
                layout.AddRow(_gridView);
                _gridView.Columns.Add(new GridColumn()
                {
                    HeaderText = "State",
                    DataCell = new TextBoxCell("AlarmState")
                });

                layout.AddRow();
            }

            /// <summary>
            /// Refresh information on the panel
            /// </summary>
            /// <param name="client">The bacnet client to use for refreshing</param>
            /// <returns>The task for the refresh operation</returns>
            public async override Task Refresh(Client.Client client)
            {
                var alarms = await client.With(this._device.ObjectIdentifier.Instance)
                    .GetAlarmsAsync();
                _gridView.DataStore = alarms;
            }
        }
    }
}
