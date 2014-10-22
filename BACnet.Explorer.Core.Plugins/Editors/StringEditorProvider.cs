using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Client;
using BACnet.Explorer.Core.Controls;
using BACnet.Explorer.Core.Extensibility;
using Eto.Forms;

namespace BACnet.Explorer.Core.Plugins.Editors
{
    public class StringEditorProvider : EditorProvider<string, StringEditorProvider.StringEditor>
    {
        public class StringEditor : Editor<string>
        {
            public override Control Control
            {
                get
                {
                    return _textbox;
                }
            }

            protected override string controlValue
            {
                get
                {
                    return _textbox.Text;
                }

                set
                {
                    _textbox.Text = value;
                }
            }

            private TextBox _textbox;

            public StringEditor()
            {
                _textbox = new TextBox();
            }
        }
    }
}
