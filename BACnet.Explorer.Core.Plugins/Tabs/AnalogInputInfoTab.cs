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
    public class AnalogInputInfoTab : IObjectTab
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
            return objectInfo.ObjectIdentifier.Type == (ushort)ObjectType.AnalogInput;
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
            /// <summary>
            /// The object into that the panel is displaying
            /// information for
            /// </summary>
            private ObjectInfo _info;

            public Panel(ObjectInfo info)
            {
                this._info = info;             

                var obj = client.With<IAnalogInput>(_info.DeviceInstance, _info.ObjectIdentifier);

                var form = new FormBuilder()
                    .AddGroup(Constants.CorePropertiesHeader)
                        .AddRow(
                            createLabel(Constants.ObjectNameLabel),
                            bindEditor(obj, ai => ai.ObjectName))
                        .AddRow(
                            createLabel(Constants.DeviceTypeLabel),
                            bindEditor(obj, ai => ai.DeviceType, enabled: true))
                        .End()
                    .AddGroup(Constants.AdditionalPropertiesHeader)
                        .AddRow(
                            createLabel(Constants.PresentValueLabel),
                            bindEditor(obj, ai => ai.PresentValue),
                            createLabel(Constants.UnitsLabel),
                            bindEditor(obj, ai => ai.Units))
                        .AddRow(
                            createLabel(Constants.CovIncrementLabel),
                            bindEditor(obj, ai => ai.CovIncrement),
                            createLabel(Constants.DeadbandLabel),
                            bindEditor(obj, ai => ai.Deadband))
                        .AddRow(
                            createLabel(Constants.LowLimitLabel),
                            bindEditor(obj, ai => ai.LowLimit),
                            createLabel(Constants.HighLimitLabel),
                            bindEditor(obj, ai => ai.HighLimit))
                        .AddRow(
                            createLabel(Constants.MinPresValueLabel),
                            bindEditor(obj, ai => ai.MinPresValue, enabled: false),
                            createLabel(Constants.MaxPresValueLabel),
                            bindEditor(obj, ai => ai.MaxPresValue, enabled: false))
                        .End()
                    .End();

                this.Content = form.Root;
            }
        }
    }
}
