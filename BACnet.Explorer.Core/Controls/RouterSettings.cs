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
    public class RouterSettings : DynamicLayout
    {
        private RouterProcess _process;
        private TextBox _name;
        private NumericUpDown _processId;

        public RouterSettings(RouterProcess process)
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

            this.BeginVertical();
            this.AddRow(new Label() { Text = Constants.ProcessNameLabel }, _name);
            this.AddRow(new Label() { Text = Constants.ProcessIdLabel }, _processId);
            this.EndVertical();


            GridView gv = new GridView();
            gv.DataStore = process.PortMappings;

            gv.Columns.Add(new GridColumn()
            {
                HeaderText = "Port Id",
                DataCell = new TextBoxCell("PortId"),
                Editable = true
            });
            gv.Columns.Add(new GridColumn()
            {
                HeaderText = "Network Number",
                DataCell = new TextBoxCell("Network"),
                Editable = true
            });
            
            this.BeginVertical();
            this.AddRow(gv);
            this.EndVertical();

            this.AddRow();
        }
    }
}
