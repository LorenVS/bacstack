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
    public class RealEditorProvider : EditorProvider<float, RealEditorProvider.RealEditor>
    {
        public class RealEditor : Editor<float>
        {
            public override Control Control { get { return _upDown; } }

            protected override float controlValue
            {
                get { return (float)_upDown.Value; }
                set { _upDown.Value = value; }
            }

            private NumericUpDown _upDown;

            public RealEditor()
            {
                _upDown = new NumericUpDown();
            }
        }
    }
}
