using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Drawing;
using Eto.Forms;
using BACnet.Core.Datalink;
using BACnet.Core.Network;
using BACnet.Core.App;
using BACnet.Client;
using BACnet.Explorer.Core.Models;

namespace BACnet.Explorer.Core.Controls
{
    public class SessionCreator : DynamicLayout
    {
        private Session _session;
        private ListBox _processesList;
        private Panel _contentPanel;
        private ComboBox _portTypesCombo;
        private Button _addPortButton;
        private Button _createSessionButton;

        public event EventHandler<Session> SessionCreated;

        public SessionCreator()
        {
            _session = new Session();
            _session.Processes.Add(new PortManagerProcess());
            _session.Processes.Add(new RouterProcess());
            _session.Processes.Add(new HostProcess());
            _session.Processes.Add(new DeviceFinderProcess ());
            _session.Processes.Add(new NetworkDatabaseProcess());

            var left = new TableLayout(1, 3);
            
            _processesList = new ListBox();
            _processesList.Size = new Size(250, -1);
            _processesList.DataStore = _session.Processes;
            _processesList.SelectedValueChanged += _selectedProcessChanged;
            
            left.Add(_processesList, 0, 0);

            _portTypesCombo = new ComboBox();            
            foreach (var portType in PortType.All())
            {
                _portTypesCombo.Items.Add(portType);
            }

            _addPortButton = new Button();
            _addPortButton.Text = Constants.AddPortButtonText;
            _addPortButton.Click += _addPortButtonClicked;

            var newPortLayout = new DynamicLayout();
            newPortLayout.Padding = Eto.Drawing.Padding.Empty;
            newPortLayout.AddRow(_portTypesCombo, _addPortButton);
            left.Add(newPortLayout, 0, 1);

            _contentPanel = new Panel();
            _contentPanel.Content = new SessionSettings(_session);

            _createSessionButton = new Button();
            _createSessionButton.Text = Constants.CreateSessionButtonText;
            _createSessionButton.Click += _createSessionButtonClicked;

            
            this.BeginVertical(padding:null, xscale: true, yscale: true);
            this.AddRow(left, _contentPanel);
            this.EndVertical();

            this.BeginVertical(padding:null, xscale: true, yscale: false);
            this.AddRow(null, _createSessionButton);
            this.EndVertical();
        }

        private void _createSessionButtonClicked(object sender, EventArgs e)
        {
            var session = this._session;
            this._session = new Session();

            SessionsStore.Instance.SaveSession(session);
            if (this.SessionCreated != null)
                this.SessionCreated(this, session);
        }

        private void _addPortButtonClicked(object sender, EventArgs e)
        {
            var portType = _portTypesCombo.SelectedValue as PortType;
            if(portType != null)
            {
                var process = Activator.CreateInstance(portType.Type) as Process;
                if(process != null)
                {
                    _session.Processes.Add(process);
                    _processAdded(process);
                }
            }
        }

        /// <summary>
        /// Called whenever a process is added
        /// </summary>
        /// <param name="process">The process that was added</param>
        private void _processAdded(Process process)
        {
            // bad way to check if the process is a port
            var portIdProp = process.GetType().GetProperty("PortId");
            if(portIdProp != null)
            {
                byte portId = (byte)portIdProp.GetValue(process);
                var router = _session.Processes.OfType<RouterProcess>().FirstOrDefault();
                if (router != null)
                {
                    router.PortMappings.Add(new PortMapping()
                    {
                        PortId = portId,
                        Network = 0
                    });
                }
            }
        }

        private void _selectedProcessChanged(object sender, EventArgs e)
        {
            var temp = _processesList.SelectedValue;
            Control content = null;

            if(temp is PortManagerProcess)
            {
                var process = temp as PortManagerProcess;
                content = new PortManagerSettings(process);
            }
            else if(temp is RouterProcess)
            {
                var process = temp as RouterProcess;
                content = new RouterSettings(process);
            }
            else if(temp is HostProcess)
            {
                var process = temp as HostProcess;
                content = new HostSettings(process);
            }
            else if(temp is ForeignDevicePortProcess)
            {
                var process = temp as ForeignDevicePortProcess;
                content = new ForeignDevicePortSettings(process);
            }
            else if(temp is DeviceFinderProcess)
            {
                var process = temp as DeviceFinderProcess;
                content = new DeviceFinderSettings(process);
            }
            else if(temp is NetworkDatabaseProcess)
            {
                var process = temp as NetworkDatabaseProcess;
                content = new NetworkDatabaseSettings(process);
            }
            else
            {
                content = new SessionSettings(_session);
            }

            _contentPanel.Content = content;
        }
    }
}
