using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto;
using Eto.Forms;
using BACnet.Ashrae;
using BACnet.Ashrae.Objects;
using BACnet.Core.App;
using BACnet.Client;
using BACnet.Client.Descriptors;
using BACnet.Explorer.Core.Controls;
using BACnet.Explorer.Core.Extensibility;

namespace BACnet.Explorer.Core.Plugins.Tabs
{
    public class ScheduleInfoTab : IObjectTab
    {
        /// <summary>
        /// The order in which the tab should appear
        /// </summary>
        public int Order
        {
            get { return 10; }
        }

        /// <summary>
        /// Determines whether this page should be created for an object
        /// </summary>
        /// <param name="objectInfo">The object info</param>
        /// <returns>True if the page should be created, false otherwise</returns>
        public bool Active(ObjectInfo objectInfo)
        {
            return objectInfo.ObjectIdentifier.Type == (ushort)ObjectType.Schedule;
        }

        /// <summary>
        /// Creates the tab page
        /// </summary>
        /// <param name="objectInfo">The object info to create the tab for</param>
        /// <returns>The tab page instance</returns>
        public TabPage Create(ObjectInfo objectInfo)
        {
            var page = new TabPage();
            page.Text = Constants.InfoTabText;
            page.Content = new Panel(objectInfo);
            return page;
        }

        public class Panel : BACnetPanel2
        {
            public Panel(ObjectInfo info)
            {
                var obj = client.With<ISchedule>(
                    info.DeviceInstance,
                    info.ObjectIdentifier);

                var form = new FormBuilder()
                    .AddGroup(Constants.CorePropertiesHeader)
                        .AddRow(
                            createLabel(Constants.ObjectNameLabel),
                            bindEditor(obj, sch => sch.ObjectName))
                        .AddRow(
                            createLabel(Constants.OutOfServiceLabel),
                            bindEditor(obj, sch => sch.OutOfService))
                        .End()
                    .End();

                this.Content = form.Root;
            }
        }
    }
}
