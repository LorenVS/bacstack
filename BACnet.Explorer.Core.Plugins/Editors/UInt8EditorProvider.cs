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
    public class UInt8EditorProvider : EditorProvider<byte, UInt8EditorProvider.UInt8Editor>
    {
        public class UInt8Editor : Editor<byte>
        {
            public override Control Control { get { return _upDown; } }

            protected override byte controlValue
            {
                get { return Convert.ToByte(_upDown.Value); }
                set { _upDown.Value = value; }
            }

            /// <summary>
            /// The numeric updown
            /// </summary>
            private NumericUpDown _upDown;

            public UInt8Editor()
            {
                _upDown = new NumericUpDown();
                _upDown.DecimalPlaces = 0;
                _upDown.MinValue = Byte.MinValue;
                _upDown.MaxValue = Byte.MaxValue;
            }
        }
    }
}
