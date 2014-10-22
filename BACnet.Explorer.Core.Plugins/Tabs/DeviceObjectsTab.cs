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
using BACnet.Explorer.Core.Models;

namespace BACnet.Explorer.Core.Plugins.Tabs
{
    public class DeviceObjectsTab : IDeviceTab
    {
        /// <summary>
        /// The order in which the tab should appear
        /// </summary>
        public int Order
        {
            get { return 0; }
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
            page.Text = Constants.DeviceObjectsTabText;
            page.Content = new Panel(deviceInfo);
            return page;
        }

        public class Panel : BACnetPanel
        {
            /// <summary>
            /// The device that the panel is showing information for
            /// </summary>
            private ObjectInfo _device;

            /// <summary>
            /// The observable collection of objects for the device
            /// </summary>
            private DescriptorObserverCollection<ObjectInfo, GlobalObjectId> _objects;

            /// <summary>
            /// Subscription to the network database's objects for the device
            /// </summary>
            private IDisposable _objectsSubscription;

            /// <summary>
            /// Grid view to display the objects
            /// </summary>
            private GridView<ObjectInfo> _grid;

            /// <summary>
            /// Constructs a new device info tab panel
            /// </summary>
            /// <param name="device">The device to show information for</param>
            public Panel(ObjectInfo device)
            {
                this._device = device;

                // subscribe to the device's objects
                var db = BACnetSession.Current.GetProcess<NetworkDatabase>();
                this._objects = new DescriptorObserverCollection<ObjectInfo, GlobalObjectId>(Application.Instance);
                this._objectsSubscription = db.Subscribe(new DescriptorQuery(deviceInstance: _device.DeviceInstance), this._objects);
                
                _grid = new GridView<ObjectInfo>();
                _grid.DataStore = _objects;

                // helps drastically with layout performance,
                // specifically on WPF
                _grid.RowHeight = 19;

                _grid.AllowMultipleSelection = false;

                _grid.Columns.Add(new GridColumn()
                {
                    HeaderText = Constants.ObjectTypeHeaderText,
                    Editable = false,
                    Width = 100,
                    DataCell = new TextBoxCell()
                    {
                        Binding = new LambdaBinding<ObjectInfo, string>(
                            oi => ((ObjectType)oi.ObjectIdentifier.Type).ToString())
                    }
                });

                _grid.Columns.Add(new GridColumn()
                {
                    HeaderText = Constants.ObjectInstanceHeaderText,
                    Editable = false,
                    Width = 75,
                    DataCell = new TextBoxCell()
                    {
                        Binding = new LambdaBinding<ObjectInfo, string>(
                            oi => oi.ObjectIdentifier.Instance.ToString())
                    }
                });

                _grid.Columns.Add(new GridColumn()
                {
                    HeaderText = Constants.ObjectNameHeaderText,
                    Editable = false,
                    DataCell = new TextBoxCell()
                    {
                        Binding = new LambdaBinding<ObjectInfo, string>(
                            oi => oi.Name)
                    }
                });

                _grid.SelectedItemsChanged += delegate(object s, EventArgs e)
                {
                    var item = _grid.SelectedItem as ObjectInfo;
                    if (item != null)
                    {
                        var stack = MainForm.Current.Stack;
                        stack.PopUntil<DevicePanel>();
                        stack.Push(new ObjectPanel(item));
                    }
                };

                this.Content = _grid;
                               
            }

            protected override void OnUnLoad(EventArgs e)
            {
                base.OnUnLoad(e);
                if(_objectsSubscription != null)
                {
                    _objectsSubscription.Dispose();
                    _objectsSubscription = null;
                }
            }

        }
    }
}
