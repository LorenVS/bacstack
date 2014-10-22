using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto;
using Eto.Forms;
using BACnet.Client;
using BACnet.Client.Descriptors;
using BACnet.Explorer.Core.Controls;
using BACnet.Explorer.Core.Extensibility;
using BACnet.Explorer.Core.Models;

namespace BACnet.Explorer.Core
{
    public class MainForm : Form
    {
        /// <summary>
        /// Retrieves the current main form instance
        /// </summary>
        public static MainForm Current
        {
            get { return Eto.Forms.Application.Instance.MainForm as MainForm; }
        }

        public MainFormModel Model { get; private set; }
        public SplitterStack Stack { get; private set; }

        private BACnetSession _bacnet;
        private SessionChooser _chooser;
        private DevicesTree _devicesTree;

        public MainForm()
        {
            _createMenu();

            this.WindowState = WindowState.Maximized;
            this.Model = new MainFormModel();
            this._chooser = new SessionChooser();
            this._chooser.Closed += _chooserClosed;
            this._chooser.SessionChosen += _sessionChosen;

            // sets the client (inner) size of the window for your content
            this.ClientSize = new Eto.Drawing.Size(600, 400);
            this.Title = Constants.MainFormDefaultTitle;
        }

        private void _createMenu()
        {
            this.Menu = new MenuBar();

            var tools = ExtensionManager.GetExtensions<ICustomTool>()
                .Where(tool => tool.Active())
                .ToArray();

            if (tools.Length > 0)
            {
                var menu = new ButtonMenuItem() { Text = "Tools" };
                this.Menu.Items.Add(menu);

                foreach(var t in tools)
                {
                    var item = new ButtonMenuItem() { Text = t.Name };
                    item.Click += (o, e) => t.Launch();
                    menu.Items.Add(item);
                }
            }
        }

        private void _chooserClosed(object sender, EventArgs e)
        {
            if (Model.Session == null)
                this.Close();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

        }

        /// <summary>
        /// Called when the form initially loads
        /// </summary>
        /// <param name="e">The event args</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this._chooser.ShowModalAsync();
        }

        /// <summary>
        /// Called when a session is chosen
        /// </summary>
        /// <param name="e">The chosen session</param>
        private void _sessionChosen(object sender, Session e)
        {
            Model.Session = e;

            var options = e.Processes.Select(p => p.CreateOptions());
            BACnetSession.Current = new BACnetSession(options);
            this._bacnet = BACnetSession.Current;

            this.Title = string.Format(Constants.MainFormTitle, Model.Session.Name);
            _createUI();
        }



        /// <summary>
        /// Creates the controls
        /// </summary>
        private void _createUI()
        {
            this.Stack = new SplitterStack();
            this.Content = this.Stack;

            _devicesTree = new DevicesTree();

            _devicesTree.Size = new Eto.Drawing.Size(300, -1);
            this.Stack.Push(_devicesTree, false);
        }
    }
}
