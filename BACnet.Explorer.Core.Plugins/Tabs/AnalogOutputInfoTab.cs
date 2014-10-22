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
    public class AnalogOutputInfoTab : IObjectTab
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
            return objectInfo.ObjectIdentifier.Type == (ushort)ObjectType.AnalogOutput;
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
                var obj = client.With<IAnalogOutput>(
                    info.DeviceInstance,
                    info.ObjectIdentifier);

                var form = new FormBuilder()
                    .AddGroup(Constants.CorePropertiesHeader)
                        .AddRow(
                            createLabel(Constants.ObjectNameLabel),
                            bindEditor(obj, ao => ao.ObjectName)
                        )
                        .AddRow(
                            createLabel(Constants.DeviceTypeLabel),
                            bindEditor(obj, ao => ao.DeviceType)
                        )
                        .Split()
                        .AddRow(
                            createLabel(Constants.PresentValueLabel),
                            bindEditor(obj, ao => ao.PresentValue),
                            createLabel(Constants.UnitsLabel),
                            bindEditor(obj, ao => ao.Units))
                        .End()
                    .AddGroup(Constants.AdditionalPropertiesHeader)
                        .AddRow(
                            createLabel(Constants.LowLimitLabel),
                            bindEditor(obj, ao => ao.LowLimit),
                            createLabel(Constants.HighLimitLabel),
                            bindEditor(obj, ao => ao.HighLimit))
                        .AddRow(
                            createLabel(Constants.MinPresValueLabel),
                            bindEditor(obj, ao => ao.MinPresValue, enabled: false),
                            createLabel(Constants.MaxPresValueLabel),
                            bindEditor(obj, ao => ao.MaxPresValue, enabled: false))
                        .AddRow(
                            createLabel(Constants.ResolutionLabel),
                            bindEditor(obj, ao => ao.Resolution, enabled: false),
                            createLabel(Constants.CovIncrementLabel),
                            bindEditor(obj, ao => ao.CovIncrement, enabled: false))
                        .End()
                    .End();

                this.Content = form.Root;
            }

        }
    }
}
