using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Client;
using BACnet.Types;
using BACnet.Explorer.Core.Controls;
using BACnet.Explorer.Core.Extensibility;
using Eto.Forms;

namespace BACnet.Explorer.Core.Plugins.Editors
{
    public class OptionEditorProvider : IEditorProvider
    { 
        public bool ProvidesEditorFor<T>()
        {
            var type = typeof(T);
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Option<>);
        }

        public IEditor<T> CreateEditor<T>()
        {
            var elType = typeof(T).GetGenericArguments()[0];

            return (IEditor<T>)Activator.CreateInstance(
                typeof(OptionEditor<>).MakeGenericType(elType));
        }

        public class OptionEditor<T> : Editor<Option<T>>
        {
            private DynamicLayout _layout;
            private CheckBox _checkbox;
            private IEditor<T> _editor;

            public override Control Control { get { return _layout; } }

            protected override Option<T> controlValue
            {
                get
                {
                    if (_checkbox.Checked.Value)
                        return new Option<T>(_editor.CurrentValue);
                    else
                        return new Option<T>();
                }

                set
                {
                    if(value.HasValue)
                    {
                        _checkbox.Checked = true;
                        _editor.Enabled = true;
                        _editor.CurrentValue = value.Value;
                    }
                    else
                    {
                        _checkbox.Checked = false;
                        _editor.Enabled = false;
                    }
                }
            }

            public OptionEditor()
            {
                _layout = new DynamicLayout();
                _checkbox = new CheckBox();
                _checkbox.ThreeState = false;
                _checkbox.CheckedChanged += _checkChanged;
                _editor = ExtensionManager.CreateEditor<T>();
                _editor.PristineValue = default(T);

                _layout.AddRow(
                    _checkbox,
                    _editor.Control);
            }

            private void _checkChanged(object sender, EventArgs e)
            {
                _editor.Enabled = _checkbox.Checked.Value;
            }
        }
    }
}
