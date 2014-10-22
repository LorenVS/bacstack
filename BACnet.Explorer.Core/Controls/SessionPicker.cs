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
    public class SessionPicker : DynamicLayout
    {
        private List<Session> _sessions;
        private ListBox _sessionList;
        private Button _pickButton;
        
        public event EventHandler<Session> SessionChosen;

        public SessionPicker()
        {
            _sessions = SessionsStore.Instance.GetSessions();
            _sessionList = new ListBox();
            _sessionList.DataStore = _sessions;
            _sessionList.TextBinding = new PropertyBinding<string>("Name");

            _pickButton = new Button();
            _pickButton.Text = Constants.PickSessionButtonText;
            _pickButton.Click += _pickButtonClicked;

            this.BeginVertical(padding: null, xscale: true, yscale: true);
            this.AddRow(_sessionList);
            this.EndVertical();
            this.BeginVertical(padding: null,  xscale: false, yscale: false);
            this.AddRow(null, _pickButton);
            this.EndVertical();
        }

        private void _pickButtonClicked(object sender, EventArgs e)
        {
            var session = _sessionList.SelectedValue as Session;
            if (session != null && SessionChosen != null)
                SessionChosen(this, session);

        }
    }
}
