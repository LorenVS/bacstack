using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Core.Datalink;

namespace BACnet.Core.Network
{
    public class RoutingTable
    {
        /// <summary>
        /// The routes that are in the routing table
        /// </summary>
        private readonly List<Route> _routes;

        public RoutingTable()
        {
            _routes = new List<Route>();
        }

        /// <summary>
        /// Finds the route to a network
        /// </summary>
        /// <param name="network">The network to find</param>
        /// <returns>The index of the route in the list, or -1 if the route does not exist</returns>
        private int _findRoute(ushort network)
        {
            for(int i = 0; i < _routes.Count; i++)
            {
                if (_routes[i].Network == network)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Finds a route to a directly attached network by its port id
        /// </summary>
        /// <param name="portId">The port id</param>
        /// <returns>The index of the route in the list, or -1 if the route does not exist</returns>
        private int _findRouteByPortId(byte portId)
        {
            for (int i = 0; i < _routes.Count; i++)
            {
                if (_routes[i].PortId == portId && _routes[i].NextHop.IsBroadcast())
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Replaces a route in the routing table, or inserts a new
        /// one if there is no suitable route to replace
        /// </summary>
        /// <param name="index">The index of the route to replace, or -1 for insertion</param>
        /// <param name="route">The new route</param>
        private Route _upsertRoute(int index, Route route)
        {
            if (index == -1)
            {
                _routes.Add(route);
                return route;
            }
            else if (route.NextHop.IsBroadcast()) // give preference to local routes
            {
                _routes[index] = route;
                return route;
            }
            else if (!_routes[index].NextHop.IsBroadcast()) // don't overwrite local routes
            {
                _routes[index] = route;
                return route;
            }
            else
                return _routes[index];
        }

        /// <summary>
        /// Adds a new local route to the routing table
        /// </summary>
        /// <param name="network">The network number of the route</param>
        /// <param name="portId">The port id of the route</param>
        public Route AddLocalRoute(ushort network, byte portId)
        {
            int index = _findRoute(network);
            return _upsertRoute(index, new Route(network, portId, Mac.Broadcast));
        }

        /// <summary>
        /// Adds a new remote route to routing table
        /// </summary>
        /// <param name="network">The network number of the route</param>
        /// <param name="portId">The port id of the route</param>
        /// <param name="nextHop">The next hop mac address</param>
        public Route AddRemoteRoute(ushort network, byte portId, Mac nextHop)
        {
            int index = _findRoute(network);
            return _upsertRoute(index, new Route(network, portId, nextHop));
        }

        /// <summary>
        /// Gets the route to a network
        /// </summary>
        /// <param name="network">The network to get the route to</param>
        /// <returns>The route object</returns>
        public Route GetRoute(ushort network)
        {
            int index = _findRoute(network);
            return index == -1 ? null : _routes[index];
        }

        /// <summary>
        /// Gets the local route to a directly attached network
        /// </summary>
        /// <param name="portId">The port id of the port</param>
        /// <returns>The route, or null if no route exists</returns>
        public Route GetRouteByPortId(byte portId)
        {
            int index = _findRouteByPortId(portId);
            return index == -1 ? null : _routes[index];
        }
    }
}
