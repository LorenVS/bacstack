using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Eto;
using Eto.Forms;
using BACnet.Core.App;
using BACnet.Core.Exceptions;
using BACnet.Client;
using BACnet.Explorer.Core.Extensibility;


namespace BACnet.Explorer.Core.Controls
{
    public class BACnetPanel2 : Panel
    {
        /// <summary>
        /// The timer to use for refreshing property
        /// values from remote devices
        /// </summary>
        private UITimer _refreshTimer;

        /// <summary>
        /// Whether or not the panel is currently refreshing
        /// </summary>
        private bool _refreshing;
        
        /// <summary>
        /// The interval at which the panel should refresh its BACnet data
        /// </summary>
        public TimeSpan RefreshInterval
        {
            get { return _refreshInterval; }
            set
            {
                _refreshInterval = value;
                if (_refreshTimer != null)
                {
                    _refreshTimer.Interval = value.TotalSeconds;
                }
            }
        }
        private TimeSpan _refreshInterval;

        /// <summary>
        /// The BACnet host
        /// </summary>
        protected Host host;

        /// <summary>
        /// The BACnet client
        /// </summary>
        protected Client.Client client;

        /// <summary>
        /// The BACnet network database
        /// </summary>
        protected NetworkDatabase db;

        /// <summary>
        /// The editor bin
        /// </summary>
        private List<IEditorBinding> _editorBindings;

        /// <summary>
        /// Constructs a new BACnetPanel2 instance
        /// </summary>
        public BACnetPanel2()
        {
            RefreshInterval = TimeSpan.FromSeconds(Constants.DefaultRefreshInterval);
            _editorBindings = new List<IEditorBinding>();

            var session = BACnetSession.Current;
            host = session.GetProcess<Host>();
            client = new Client.Client(host);
            db = session.GetProcess<NetworkDatabase>();
        }

        /// <summary>
        /// Perform the initial refresh on load
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _refreshTimer = new UITimer();
            _refreshTimer.Interval = RefreshInterval.TotalSeconds;
            _refreshTimer.Elapsed += _refresh;
            _refreshTimer.Start();
            _refresh(null, null);
        }

        /// <summary>
        /// Disabled future refreshes
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUnLoad(EventArgs e)
        {
            base.OnUnLoad(e);

            if (_refreshTimer != null)
            {
                _refreshTimer.Stop();
                _refreshTimer = null;
            }
        }


        /// <summary>
        /// Called when the refreshTimer elapses, and triggers
        /// a refresh of BACnet data on this panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void _refresh(object sender, EventArgs e)
        {
            if (_refreshing)
                return;

            try
            {
                foreach(var binding in _editorBindings)
                {
                    binding.Refresh();
                }
                await client.ReadQueue.SendAsync();
            }
            catch (BACnetException)
            {
            }
            catch (AggregateException)
            {
            }
            catch (Exception)
            {

            }
            finally
            {
                _refreshing = false;
            }
        }

        /// <summary>
        /// Creates and registers a new editor
        /// </summary>
        /// <typeparam name="TObj">The type of object</typeparam>
        /// <typeparam name="TProp">The type of property</typeparam>
        /// <param name="obj">The object handle</param>
        /// <param name="expression">The property expression</param>
        /// <returns>The editor control</returns>
        protected Control bindEditor<TObj, TProp>(ObjectHandle<TObj> obj, Expression<Func<TObj, TProp>> expression, bool enabled = true)
        {
            var editor = ExtensionManager.CreateEditor<TProp>();
            editor.Enabled = enabled;

            var reference = ObjectHelpers.GetObjectPropertyReference(
                obj.ObjectIdentifier,
                expression);

            var binding = new PropertyEditorBinding<TProp>(
                obj.DeviceInstance,
                reference,
                obj.Client,
                editor);

            this._editorBindings.Add(binding);
            return editor.Control;
        }

        /// <summary>
        /// Creates a new label instance
        /// </summary>
        /// <param name="text">The text of the label</param>
        /// <returns>The label instance</returns>
        protected Label createLabel(string text)
        {
            return new Label() { Text = text };
        }

        /// <summary>
        /// Refreshes information on the panel
        /// </summary>
        /// <param name="client">The client to use for refreshing</param>
        public async virtual Task Refresh(BACnet.Client.Client client)
        {
            // get rid of warning
            await Task.Delay(0);
        }
    }
}
