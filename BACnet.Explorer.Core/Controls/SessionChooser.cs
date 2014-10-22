using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Drawing;
using Eto.Forms;
using BACnet.Explorer.Core.Models;

namespace BACnet.Explorer.Core.Controls
{
    public class SessionChooser : Dialog
    {
        private TabControl _tabs;
        private TabPage _existingPage;
        private SessionPicker _picker;
        private TabPage _newPage;
        private SessionCreator _creator;

        public event EventHandler<Session> SessionChosen;

        public SessionChooser()
        {
            this.Size = new Size(640, 480);

            _existingPage = new TabPage();
            _existingPage.Text = Constants.ExistingSessionText;

            _picker = new SessionPicker();
            _picker.SessionChosen += _pickerSessionChosen;
            _existingPage.Content = _picker;

            _newPage = new TabPage();
            _newPage.Text = Constants.NewSessionText;

            _creator = new SessionCreator();
            _creator.SessionCreated += _creatorSessionCreated;
            _newPage.Content = _creator;


            _tabs = new TabControl();
            _tabs.Pages.Add(_existingPage);
            _tabs.Pages.Add(_newPage);


            this.Content = _tabs;
            this.Title = Constants.ChooseSessionTitle;
        }

        private void _pickerSessionChosen(object sender, Session e)
        {
            if (SessionChosen != null)
                SessionChosen(this, e);
            this.Close();
        }

        private void _creatorSessionCreated(object sender, Session e)
        {
            if (SessionChosen != null)
                SessionChosen(this, e);
            this.Close();
        }
    }
}
