using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto;
using Eto.Forms;
using BACnet.Ashrae;
using BACnet.Client;
using BACnet.Explorer.Core.Controls;
using BACnet.Explorer.Core.Extensibility;

namespace BACnet.Explorer.Core.Plugins.Editors
{
    public class DateAndTimeEditorProvider : EditorProvider<DateAndTime, DateAndTimeEditorProvider.DateAndTimeEditor>
    {
        public class DateAndTimeEditor : Editor<DateAndTime>
        {
            public override Control Control { get { return _picker; } }

            protected override DateAndTime controlValue
            {
                get {
                    if (_picker.Value == null)
                        return null;
                    else
                        return DateAndTime.FromDateTime(_picker.Value.Value);
                }
                set {
                    if (value == null)
                        _picker.Value = null;
                    else
                        _picker.Value = value.ToDateTime();
                }
            }

            private DateTimePicker _picker;

            public DateAndTimeEditor()
            {
                _picker = new DateTimePicker();
            }


        }
    }
}
