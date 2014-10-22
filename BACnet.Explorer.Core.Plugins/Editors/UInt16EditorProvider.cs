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
    public class UInt16EditorProvider : EditorProvider<ushort, UInt16EditorProvider.UInt16Editor>
    {
        public class UInt16Editor : Editor<ushort>
        {
            public override Control Control { get { return _upDown; } }

            protected override ushort controlValue
            {
                get { return Convert.ToUInt16(_upDown.Value); }
                set { _upDown.Value = value; }
            }

            /// <summary>
            /// The numeric updown
            /// </summary>
            private NumericUpDown _upDown;

            public UInt16Editor()
            {
                _upDown = new NumericUpDown();
                _upDown.DecimalPlaces = 0;
                _upDown.MinValue = ushort.MinValue;
                _upDown.MaxValue = ushort.MaxValue;
            }
        }
        
    }
}
