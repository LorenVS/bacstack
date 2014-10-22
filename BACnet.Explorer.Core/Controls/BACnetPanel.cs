using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto;
using Eto.Forms;
using BACnet.Core.App;
using BACnet.Core.Exceptions;
using BACnet.Client;

namespace BACnet.Explorer.Core.Controls
{
    public class BACnetPanel : Panel
    {
        /// <summary>
        /// The timer used to refresh
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
                if(_refreshTimer != null)
                {
                    _refreshTimer.Interval = value.TotalSeconds;
                }
            }
        }
        private TimeSpan _refreshInterval;

        
        /// <summary>
        /// Constructs a new BACnetPanel instance
        /// </summary>
        public BACnetPanel()
        {
            RefreshInterval = TimeSpan.FromSeconds(Constants.DefaultRefreshInterval);
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
            _refreshTimer.Stop();
            _refreshTimer = null;
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
                var session = BACnetSession.Current;
                var host = session.GetProcess<Host>();
                var client = new Client.Client(host);

                await this.Refresh(client);

            }
            catch(BACnetException)
            {
            }
            catch(AggregateException)
            {
            }
            catch(Exception)
            {

            }
            finally
            {
                _refreshing = false;
            }
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
