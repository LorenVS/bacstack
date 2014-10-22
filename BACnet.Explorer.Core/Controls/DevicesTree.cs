using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto;
using Eto.Forms;
using BACnet.Client;
using BACnet.Client.Descriptors;
using BACnet.Explorer.Core.Models;

namespace BACnet.Explorer.Core.Controls
{
    public class DevicesTree : Panel
    {
        private IDisposable _devicesSubscription;
        private DeviceTreeStore _devices;
        private TreeGridView _tree;
        private UITimer _refreshTimer;

        public DevicesTree()
        {
            _tree = new TreeGridView();

            _tree.AllowMultipleSelection = false;

            _tree.Columns.Add(new GridColumn()
            {
                HeaderText = "Instance",
                DataCell = new TextBoxCell("DeviceInstance")
            });
            _tree.Columns.Add(new GridColumn()
            {
                HeaderText = "Name",
                DataCell = new TextBoxCell("Name"),
                AutoSize = true
            });
            _tree.SelectedItemsChanged += _selectedItemChanged;

            this.Content = _tree;
        }

        private void _selectedItemChanged(object sender, EventArgs e)
        {
            var node = _tree.SelectedItem as DeviceTreeNode;
            if(node != null)
            {
                var info = node.DeviceInfo;
                var stack = MainForm.Current.Stack;

                stack.PopUntil<DevicesTree>();
                stack.Push(new DevicePanel(info));
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var database = BACnetSession.Current.GetProcess<NetworkDatabase>();
            this._devices = new DeviceTreeStore(Application.Instance);
            this._devicesSubscription = database.Subscribe(new DescriptorQuery(objectType: 8), this._devices);
            _tree.DataStore = this._devices;

            _refreshTimer = new UITimer();
            _refreshTimer.Interval = .5;
            _refreshTimer.Elapsed += _refreshTimerElapsed;
            _refreshTimer.Start();
        }

        private void _refreshTimerElapsed(object sender, EventArgs e)
        {
            _tree.DataStore = this._devices;
        }

        protected override void OnUnLoad(EventArgs e)
        {
            base.OnUnLoad(e);

            if (this._devicesSubscription != null)
            {
                this._devicesSubscription.Dispose();
                this._devicesSubscription = null;
            }

            if(_refreshTimer != null)
            {
                _refreshTimer.Dispose();
                _refreshTimer = null;
            }

            this._devices = null;
            this.Content = null;
        }
    }
}
