using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto;
using Eto.Forms;
using BACnet.Explorer.Core.Models;

namespace BACnet.Explorer.Core.Controls
{
    public class SessionSettings : DynamicLayout
    {
        private Session _session;
        private TextBox _name;

        public SessionSettings(Session session)
        {
            _session = session;

            _name = new TextBox();
            _name.Bind(
                tb => tb.Text,
                _session,
                sess => sess.Name,
                DualBindingMode.TwoWay);


            this.AddRow(new Label() { Text = Constants.SessionNameLabel }, _name);

            this.AddRow();
        }
    }
}
