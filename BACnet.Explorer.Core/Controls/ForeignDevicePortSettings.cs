using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto;
using Eto.Forms;
using BACnet.Core.Datalink;
using BACnet.Explorer.Core.Models;

namespace BACnet.Explorer.Core.Controls
{
    public class ForeignDevicePortSettings : DynamicLayout
    {
        private ForeignDevicePortProcess _process;
        private TextBox _name;
        private NumericUpDown _processId;
        private TextBox _localHost;
        private NumericUpDown _localPort;
        private TextBox _bbmdHost;
        private NumericUpDown _bbmdPort;

        public ForeignDevicePortSettings(ForeignDevicePortProcess process)
        {
            _process = process;

            _name = new TextBox();
            _name.Bind(
                tb => tb.Text,
                _process,
                proc => proc.Name,
                DualBindingMode.TwoWay);

            _processId = new NumericUpDown();
            _processId.Bind(
                nud => nud.Value,
                _process,
                proc => proc.ProcessId,
                DualBindingMode.TwoWay);

            _localHost = new TextBox();
            _localHost.Bind(
                tb => tb.Text,
                _process,
                proc => proc.LocalHost,
                DualBindingMode.TwoWay);

            _localPort = new NumericUpDown();
            _localPort.Bind(
                nud => nud.Value,
                _process,
                proc => proc.LocalPort,
                DualBindingMode.TwoWay);

            _bbmdHost = new TextBox();
            _bbmdHost.Bind(
                tb => tb.Text,
                _process,
                proc => proc.BbmdHost,
                DualBindingMode.TwoWay);

            _bbmdPort = new NumericUpDown();
            _bbmdPort.Bind(
                nud => nud.Value,
                _process,
                proc => proc.BbmdPort,
                DualBindingMode.TwoWay);

            this.BeginVertical();
            this.AddRow(new Label() { Text = Constants.ProcessNameLabel }, _name);
            this.AddRow(new Label() { Text = Constants.ProcessIdLabel }, _processId);
            this.EndVertical();

            this.BeginVertical();
            this.AddRow(new Label() { Text = Constants.LocalHostLabel }, _localHost,
                new Label() { Text = Constants.LocalPortLabel }, _localPort);
            this.AddRow(new Label() { Text = Constants.BbmdHostLabel }, _bbmdHost,
                new Label() { Text = Constants.BbmdPortLabel }, _bbmdPort);
            this.EndVertical();
            
            this.AddRow();
        }
    }
}
