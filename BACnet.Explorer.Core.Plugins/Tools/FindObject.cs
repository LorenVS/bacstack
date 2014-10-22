using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Types;
using BACnet.Client;
using BACnet.Client.Descriptors;
using BACnet.Explorer.Core.Controls;
using BACnet.Explorer.Core.Extensibility;
using BACnet.Explorer.Core.Models;
using Eto;
using Eto.Forms;

namespace BACnet.Explorer.Core.Plugins.Tools
{
    public class FindObject : ICustomTool
    {
        public string Group
        {
            get
            {
                return null;
            }
        }

        public string Name
        {
            get
            {
                return "Find Object";
            }
        }

        public bool Active()
        {
            return true;
        }

        public void Launch()
        {
            var stack = MainForm.Current.Stack;
            stack.PopUntil<DevicesTree>();
            stack.Push(new FindObjectPanel());
        }

        public class FindObjectPanel : Panel
        {
            private DynamicLayout _layout;
            private SearchBox _search;
            private GridView _grid;
            private DescriptorObserverCollection<ObjectInfo, GlobalObjectId> _objects;
            private IDisposable _objectsSubscription;
            private UITimer _searchTimer;

            public FindObjectPanel()
            {
                _layout = new DynamicLayout();
                _search = new SearchBox();
                _search.PlaceholderText = Constants.ObjectNameSearchPlaceholder;
                _search.TextChanged += _searchChanged;
                _grid = new GridView();

                // helps drastically with layout performance,
                // specifically on WPF
                _grid.RowHeight = 19;

                _grid.AllowMultipleSelection = false;

                _grid.Columns.Add(new GridColumn()
                {
                    HeaderText = "Device",
                    Editable = false,
                    Width = 100,
                    DataCell = new TextBoxCell()
                    {
                        Binding = new LambdaBinding<ObjectInfo, string>(oi => oi.DeviceInstance.ToString())
                    }
                });


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


                _grid.SelectedItemsChanged += delegate (object s, EventArgs e)
                {
                    var item = _grid.SelectedItem as ObjectInfo;
                    if (item != null)
                    {
                        var stack = MainForm.Current.Stack;
                        stack.PopUntil<FindObjectPanel>();
                        stack.Push(new ObjectPanel(item));
                    }
                };

                _layout.BeginVertical();
                _layout.AddRow(_search);
                _layout.AddRow(_grid);
                _layout.EndVertical();

                this.Content = _layout;
            }

            private void _searchChanged(object sender, EventArgs e)
            {
                // refresh the delay on the search refresh
                _searchTimer.Stop();
                _searchTimer.Start();
            }

            private void _searchRefresh(object sender, EventArgs e)
            {
                if (_objectsSubscription != null)
                {
                    _objectsSubscription.Dispose();
                    _objectsSubscription = null;
                }

                if (!string.IsNullOrEmpty(_search.Text))
                {
                    var db = BACnetSession.Current.GetProcess<NetworkDatabase>();
                    _objects = new DescriptorObserverCollection<ObjectInfo, GlobalObjectId>(Eto.Forms.Application.Instance);
                    _objectsSubscription = db.Subscribe(new DescriptorQuery(nameRegex: _search.Text), _objects);
                    _grid.DataStore = _objects;
                }

                _searchTimer.Stop();
            }

            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);

                _searchTimer = new UITimer();
                _searchTimer.Interval = .4;
                _searchTimer.Elapsed += _searchRefresh;
            }

            protected override void OnUnLoad(EventArgs e)
            {
                base.OnUnLoad(e);

                if (_objectsSubscription != null)
                {
                    _objectsSubscription.Dispose();
                    _objectsSubscription = null;
                }

                if(_searchTimer != null)
                {
                    _searchTimer.Dispose();
                    _searchTimer = null;
                }
            }
        }
    }
}