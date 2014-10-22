using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto;
using Eto.Forms;
using BACnet.Client;
using BACnet.Client.Descriptors;
using BACnet.Explorer.Core.Extensibility;

namespace BACnet.Explorer.Core.Controls
{
    public class DevicePanel : Panel
    {
        public DevicePanel(ObjectInfo info)
        {
            if(info == null)
            {
                this.Content = null;
                return;
            }

            TabControl _tabs = new TabControl();
            var tabs = ExtensionManager.GetExtensions<IDeviceTab>()
                .OrderBy(tab => tab.Order);

            foreach(var tab in tabs)
            {
                if(tab.Active(info))
                {
                    _tabs.Pages.Add(tab.Create(info));
                }
            }

            if (_tabs.Pages.Count > 0)
                this.Content = _tabs;
            else
                this.Content = null;
        }
    }
}
