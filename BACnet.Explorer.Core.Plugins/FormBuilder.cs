using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto;
using Eto.Forms;

namespace BACnet.Explorer.Core.Plugins
{
    public class FormBuilder
    {
        public Control Root { get { return _layout; } }

        private DynamicLayout _layout;

        public FormBuilder()
        {
            _layout = new DynamicLayout();
            _layout.DefaultPadding = new Eto.Drawing.Padding(5);
        }

        public GroupBuilder AddGroup(string text)
        {
            GroupBuilder group = new GroupBuilder(this, text);
            _layout.AddRow(group.Root);
            return group;
        }


        public FormBuilder End()
        {
            _layout.AddRow();
            return this;
        }

        public class GroupBuilder
        {
            public Control Root { get { return _group; } }

            private FormBuilder _form;
            private GroupBox _group;
            private DynamicLayout _layout;

            public GroupBuilder(FormBuilder form, string text)
            {
                this._form = form;
                _group = new GroupBox();
                _group.Text = text;
                _layout = new DynamicLayout();
                _group.Content = _layout;
                _layout.DefaultSpacing = new Eto.Drawing.Size(12, 12);
                _layout.BeginVertical();
            }

            public GroupBuilder AddRow(params Control[] controls)
            {
                _layout.AddRow(controls);
                return this;
            }

            public GroupBuilder Split()
            {
                _layout.EndVertical();
                _layout.BeginVertical();
                return this;
            }

            public FormBuilder End()
            {
                return _form;
            }
        }

    }
}
