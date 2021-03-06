﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto;
using Eto.Forms;
using SharpPcap.LibPcap;
using BACnet.Core.Datalink;
using BACnet.Explorer.Core.Models;

namespace BACnet.Explorer.Core.Controls
{
    public class EthernetPortSettings : DynamicLayout
    {
        private EthernetPortProcess _process;
        private TextBox _name;
        private NumericUpDown _processId;
        private ComboBox _deviceName;

        public EthernetPortSettings(EthernetPortProcess process)
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

            _deviceName = new ComboBox();
            _deviceName.DataStore = LibPcapLiveDeviceList.Instance.Where(dev => dev.Interface != null);
            _deviceName.KeyBinding = new PropertyBinding<string>("Name");
            _deviceName.TextBinding = new PropertyBinding<string>("Description");
            _deviceName.SelectedValueChanged += _deviceNameChanged;
            
            this.BeginVertical();
            this.AddRow(new Label() { Text = Constants.ProcessNameLabel }, _name);
            this.AddRow(new Label() { Text = Constants.ProcessIdLabel }, _processId);
            this.AddRow(new Label() { Text = Constants.DeviceNameLabel }, _deviceName);
            this.EndVertical();


            this.AddRow();
        }

        private void _deviceNameChanged(object sender, EventArgs e)
        {
            var device = (LibPcapLiveDevice)_deviceName.SelectedValue;
            _process.DeviceName = device.Name;
        }
    }
}
