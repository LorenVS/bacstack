﻿using System;
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
    public class AnalogValueInfoTab : IObjectTab
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
            return objectInfo.ObjectIdentifier.Type == (ushort)ObjectType.AnalogValue;
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
                var obj = client.With<IAnalogValue>(
                    info.DeviceInstance,
                    info.ObjectIdentifier);

                var form = new FormBuilder()
                    .AddGroup(Constants.CorePropertiesHeader)
                        .AddRow(
                            createLabel(Constants.ObjectNameLabel),
                            bindEditor(obj, av => av.ObjectName)
                        )
                        .Split()
                        .AddRow(
                            createLabel(Constants.PresentValueLabel),
                            bindEditor(obj, av => av.PresentValue),
                            createLabel(Constants.UnitsLabel),
                            bindEditor(obj, av => av.Units))
                        .End()
                    .AddGroup(Constants.AdditionalPropertiesHeader)
                        .AddRow(
                            createLabel(Constants.LowLimitLabel),
                            bindEditor(obj, av => av.LowLimit),
                            createLabel(Constants.HighLimitLabel),
                            bindEditor(obj, av => av.HighLimit))
                        .AddRow(
                            createLabel(Constants.DeadbandLabel),
                            bindEditor(obj, av => av.Deadband, enabled: false),
                            createLabel(Constants.CovIncrementLabel),
                            bindEditor(obj, av => av.CovIncrement, enabled: false))
                        .End()
                    .End();

                this.Content = form.Root;
            }
        }
    }
}
