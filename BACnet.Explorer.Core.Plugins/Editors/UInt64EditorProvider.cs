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
    public class UInt64EditorProvider : EditorProvider<ulong, UInt64EditorProvider.UInt64Editor>
    { 
        public class UInt64Editor : Editor<ulong>
        {
            public override Control Control { get { return _upDown; } }

            protected override ulong controlValue
            {
                get { return Convert.ToUInt64(_upDown.Value); }
                set { _upDown.Value = value; }
            }

            /// <summary>
            /// The numeric updown
            /// </summary>
            private NumericUpDown _upDown;

            public UInt64Editor()
            {
                _upDown = new NumericUpDown();
                _upDown.DecimalPlaces = 0;
                _upDown.MinValue = ulong.MinValue;
                _upDown.MaxValue = ulong.MaxValue;
            }
        }
    }
}
