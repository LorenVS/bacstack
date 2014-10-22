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
    public class EnumEditorProvider : IEditorProvider
    {
        public bool ProvidesEditorFor<T>()
        {
            return typeof(T).IsEnum;
        }

        public IEditor<T> CreateEditor<T>()
        {
            var editorType = typeof(EnumEditor<>)
                .MakeGenericType(typeof(T));
            return (IEditor<T>)Activator.CreateInstance(editorType);
        }

        public class EnumEditor<T> : Editor<T>
        {
            public override Control Control { get { return _comboBox; } }

            protected override T controlValue
            {
                get { return (T)Enum.Parse(typeof(T), _comboBox.SelectedKey); }
                set { _comboBox.SelectedKey = Enum.GetName(typeof(T), value); }
            }

            private ComboBox _comboBox;

            public EnumEditor()
            {
                _comboBox = new ComboBox();

                var names = Enum.GetNames(typeof(T));
                foreach(var name in names)
                {
                    _comboBox.Items.Add(name);
                }
            }
        }
    }
}
