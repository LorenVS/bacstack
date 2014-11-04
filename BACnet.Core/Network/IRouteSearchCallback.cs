using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.Network
{
    public interface IRouteSearchCallback
    {
        /// <summary>
        /// Called when a route search completes
        /// and a route has been found
        /// </summary>
        /// <param name="route">The route that was found</param>
        void RouteFound(Route route);

        /// <summary>
        /// Called when a route search times out
        /// </summary>
        /// <param name="network">The network that was not found</param>
        void RouteSearchTimedOut(ushort network);
    }
}
