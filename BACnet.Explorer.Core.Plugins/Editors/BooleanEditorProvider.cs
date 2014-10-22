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
    public class BooleanEditorProvider : EditorProvider<bool, BooleanEditorProvider.BooleanEditor>
    {
        public class BooleanEditor : Editor<bool>
        {
            public override Control Control { get { return _cb; } }

            protected override bool controlValue
            {
                get { return _cb.Checked.Value; }
                set { _cb.Checked = value; }
            }

            private CheckBox _cb;

            public BooleanEditor()
            {
                _cb = new CheckBox();
                _cb.ThreeState = false;
            }


        }
    }
}
