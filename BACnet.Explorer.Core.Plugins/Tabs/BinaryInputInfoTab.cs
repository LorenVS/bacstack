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
    public class BinaryInputInfoTab : IObjectTab
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
            return objectInfo.ObjectIdentifier.Type == (ushort)ObjectType.BinaryInput;
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
                var obj = client.With<IBinaryInput>(
                    info.DeviceInstance,
                    info.ObjectIdentifier);

                var form = new FormBuilder()
                    .AddGroup(Constants.CorePropertiesHeader)
                        .AddRow(
                            createLabel(Constants.ObjectNameLabel),
                            bindEditor(obj, bi => bi.ObjectName)
                        )
                        .AddRow(
                            createLabel(Constants.DeviceTypeLabel),
                            bindEditor(obj, bi => bi.DeviceType)
                        )
                        .Split()
                        .AddRow(
                            createLabel(Constants.PresentValueLabel),
                            bindEditor(obj, bi => bi.PresentValue),
                            createLabel(Constants.OutOfServiceLabel),
                            bindEditor(obj, bi => bi.OutOfService))
                        .End()
                    .AddGroup(Constants.AdditionalPropertiesHeader)
                        .AddRow(
                            createLabel(Constants.ChangeOfStateCountLabel),
                            bindEditor(obj, bi => bi.ChangeOfStateCount, enabled: false),
                            createLabel(Constants.ChangeOfStateTimeLabel),
                            bindEditor(obj, bi => bi.ChangeOfStateTime, enabled: false))
                        .AddRow(
                            createLabel(Constants.ActiveTextLabel),
                            bindEditor(obj, bi => bi.ActiveText),
                            createLabel(Constants.InactiveTextLabel),
                            bindEditor(obj, bi => bi.InactiveText))
                        .End()
                    .End();

                this.Content = form.Root;
            }
        }
    }
}
