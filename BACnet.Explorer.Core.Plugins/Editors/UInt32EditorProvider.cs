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
    public class UInt32EditorProvider : EditorProvider<uint, UInt32EditorProvider.UInt32Editor>
    {

        public class UInt32Editor : Editor<uint>
        {
            public override Control Control { get { return _upDown; } }

            protected override uint controlValue
            {
                get { return Convert.ToUInt32(_upDown.Value); }
                set { _upDown.Value = value; }
            }

            /// <summary>
            /// The numeric updown
            /// </summary>
            private NumericUpDown _upDown;

            public UInt32Editor()
            {
                _upDown = new NumericUpDown();
                _upDown.DecimalPlaces = 0;
                _upDown.MinValue = uint.MinValue;
                _upDown.MaxValue = uint.MaxValue;
            }
        }

    }
}
