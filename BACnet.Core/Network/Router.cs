using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using BACnet.Core.Datalink;
using BACnet.Core.Network.Messages;

namespace BACnet.Core.Network
{
    public class Router : IProcess, IObservable<InboundAppgram>, IObserver<InboundNetgram>, ISearchHandler<ushort>, ISearchCallback<ushort, Route>
    {
        /// <summary>
        /// The timespan to leave between searches for a route to a network
        /// </summary>
        public static readonly TimeSpan NetworkSearchInterval = TimeSpan.FromSeconds(2);

        /// <summary>
        /// The maximum number of times a network should be searched for
        /// </summary>
        public const int NetworkSearchAttempts = 2;

        /// <summary>
        /// Lock instance to synchronize access to the router
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// The process id of the router
        /// </summary>
        public int ProcessId { get { return _options.ProcessId; } }

        /// <summary>
        /// The options instance for this router
        /// </summary>
        private readonly RouterOptions _options;

        /// <summary>
        /// The routing table for the router
        /// </summary>
        private readonly RoutingTable _table = new RoutingTable();

        /// <summary>
        /// The port manager instance
        /// </summary>
        private PortManager _portManager = null;

        /// <summary>
        /// The subscription to the port manager (for netgrams)
        /// </summary>
        private IDisposable _portManagerSubscription = null;

        /// <summary>
        /// List of observers that are subscribed for appgrams
        /// </summary>
        private readonly SubscriptionList<InboundAppgram> _appgramObservers = new SubscriptionList<InboundAppgram>();
        
        /// <summary>
        /// Queue of netgrams that are awaiting routing information to send
        /// </summary>
        private readonly LinkedList<NetgramContent> _netgramQueue = new LinkedList<NetgramContent>();

        /// <summary>
        /// THe list of all the routes that are being searched for
        /// </summary>
        private SearchList<ushort, Route> _routeSearches;

        /// <summary>
        /// Constructs a new Router instance
        /// </summary>
        public Router(RouterOptions options)
        {
            this._options = options.Clone();
            this._routeSearches = new SearchList<ushort, Route>(this);

            foreach (var mapping in _options.PortNetworkMappings)
            {
                _table.AddLocalRoute(mapping.Value, mapping.Key);
            }
        }

        /// <summary>
        /// Disposes an observer's subscription
        /// </summary>
        /// <param name="observer">The observer to dispose</param>
        private void _disposeSubscription(IObserver<InboundAppgram> observer)
        {
            _appgramObservers.Unregister(observer);
        }
        
        /// <summary>
        /// Disposes the current port manager subscription
        /// </summary>
        private void _disposePortManagerSubscription()
        {
            if(_portManagerSubscription != null)
            {
                _portManagerSubscription.Dispose();
                _portManagerSubscription = null;
            }

            _portManager = null;
        }

        /// <summary>
        /// Disposes all resources held by the router
        /// </summary>
        private void _disposeAll()
        {
            _disposePortManagerSubscription();

            if (_routeSearches != null)
            {
                _routeSearches.Dispose();
                _routeSearches = null;
            }

            _netgramQueue.Clear();

            _appgramObservers.Clear();

        }

        /// <summary>
        /// Retrieves the qualified source address of a netgram
        /// </summary>
        /// <param name="netgram">The received netgram</param>
        /// <param name="header">The netgram header</param>
        /// <returns>The qualified source address</returns>
        private Address _getSource(InboundNetgram netgram, NetgramHeader header)
        {
            if (header.Source != null)
                return header.Source;
            else
            {
                lock(_lock)
                {
                    Route route = _table.GetRouteByPortId(netgram.Port.PortId);
                    return new Address(route.Network, netgram.Source);
                }
            }
        }

        /// <summary>
        /// Creates a network message instance
        /// </summary>
        /// <param name="vendorId">The vendor id of the network message</param>
        /// <param name="messageType">The message type</param>
        private INetworkMessage _createNetworkMessage(ushort vendorId, byte messageType)
        {
            if(vendorId == 0)
            {
                switch((MessageType)messageType)
                {
                    case MessageType.IAmRouterToNetwork:
                        return new IAmRouterToNetworkMessage();
                    case MessageType.WhoIsRouterToNetwork:
                        return new WhoIsRouterToNetworkMessage();
                }
            }

            throw new Exception("Could not create network message for vendor '" + vendorId + "' and message type '" + messageType + "'");
        }

        /// <summary>
        /// Processes a received network message
        /// </summary>
        /// <param name="netgram">The received netgram</param>
        /// <param name="header">The header of the netgram</param>
        /// <param name="buffer">The buffer containing the appgram</param>
        /// <param name="offset">The offset of the appgram within the buffer</param>
        /// <param name="end">The end of the content in the buffer</param>
        private void _processNetworkMessage(InboundNetgram netgram, NetgramHeader header, byte[] buffer, int offset, int end)
        {
            try
            {
                INetworkMessage message = _createNetworkMessage(header.VendorId, header.MessageType);
                offset = message.Deserialize(buffer, offset, end);

                switch(message.Type)
                {
                    case MessageType.WhoIsRouterToNetwork:
                        _processWhoIsRouterToNetworkMessage(netgram, header, (WhoIsRouterToNetworkMessage)message);
                        break;
                    case MessageType.IAmRouterToNetwork:
                        _processIAmRouterToNetworkMessage(netgram, header, (IAmRouterToNetworkMessage)message);
                        break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Processes a received i am router to network message
        /// </summary>
        /// <param name="netgram">The received netgram</param>
        /// <param name="header">The netgram header</param>
        /// <param name="message">The i am router to network message</param>
        private void _processIAmRouterToNetworkMessage(InboundNetgram netgram, NetgramHeader header, IAmRouterToNetworkMessage message)
        {
            // we only use routes from directly connected devices
            if (header.Destination == null && message.Networks != null)
            {
                List<Route> routes = new List<Route>();

                lock(_lock)
                {
                    foreach (var network in message.Networks)
                    {
                        var route = _table.AddRemoteRoute(network, netgram.Port.PortId, netgram.Source);
                        routes.Add(route);
                    }
                }

                foreach(var route in routes)
                {
                    _routeSearches.ResultFound(route.Network, route);
                }
            }
        }

        /// <summary>
        /// Processes a received who is router to network message
        /// </summary>
        /// <param name="netgram">The received netgram</param>
        /// <param name="header">The netgram header</param>
        /// <param name="message">The who is router to network message</param>
        private void _processWhoIsRouterToNetworkMessage(InboundNetgram netgram, NetgramHeader header, WhoIsRouterToNetworkMessage message)
        {
        }


        /// <summary>
        /// Creates an inbound appgram
        /// </summary>
        /// <param name="netgram">The received netgram</param>
        /// <param name="header">The header of the netgram</param>
        /// <param name="buffer">The buffer containing the appgram</param>
        /// <param name="offset">The offset of the appgram within the buffer</param>
        /// <param name="end">The end of the content in the buffer</param>
        private InboundAppgram _createInboundAppgram(InboundNetgram netgram, NetgramHeader header, byte[] buffer, int offset, int end)
        {
            InboundAppgram appgram = new InboundAppgram();
            appgram.Source = _getSource(netgram, header);
            appgram.ExpectingReply = header.ExpectingReply;
            appgram.Priority = header.Priority;
            appgram.Segment = new BufferSegment(buffer, offset, end);
            return appgram;
        }

        /// <summary>
        /// Resolves the router's dependencies
        /// </summary>
        /// <param name="processes">The processes</param>
        public void Resolve(IEnumerable<IProcess> processes)
        {
            lock(_lock)
            {
                _disposePortManagerSubscription();
                var pm = processes.OfType<PortManager>().FirstOrDefault();
                if(pm != null)
                {
                    _portManager = pm;
                    _portManagerSubscription = pm.Subscribe(this);
                }
            }
        }

        /// <summary>
        /// Disposes all resources held by the router
        /// </summary>
        public void Dispose()
        {
            lock(_lock)
            {
                _disposeAll();
            }
        }

        /// <summary>
        /// Sends a netgram using a supplied route
        /// </summary>
        /// <param name="route">The route to send with</param>
        /// <param name="content">The content to of the netgram to send</param>
        private void _sendWithRoute(Route route, NetgramContent content)
        {
            var destination = content.Destination;
            byte portId;
            Mac mac;

            if (destination.IsGlobalBroadcast())
            {
                // global broadcast
                content.Header.Destination = Address.GlobalBroadcast;
                portId = 255;
                mac = Mac.Broadcast;
            }
            else if(destination.IsDirectedlyConnectedBroadcast())
            {
                // broadcast on all directly connected networks
                content.Header.Destination = null;
                portId = 255;
                mac = Mac.Broadcast;
            }
            else if (route.NextHop.IsBroadcast())
            {
                // directly attached network
                content.Header.Destination = null;
                portId = route.PortId;
                mac = destination.Mac;
            }
            else
            {
                // not directly attached
                content.Header.Destination = destination;
                portId = route.PortId;
                mac = route.NextHop;                
            }

            OutboundNetgram netgram = new OutboundNetgram();
            netgram.PortId = portId;
            netgram.Destination = mac;
            netgram.Content = content;

            PortManager pm = null;
            lock(_lock)
            {
                pm = _portManager;
            }
            if (pm != null)
                pm.SendNetgram(netgram);
        }

        /// <summary>
        /// Sends a netgram containing either a network message or an appgram
        /// </summary>
        /// <param name="content">The netgram content to send</param>
        private void _send(NetgramContent content)
        {
            var destination = content.Destination;
            Route route = null;

            if (destination.IsGlobalBroadcast() || destination.IsDirectedlyConnectedBroadcast())
            {
                // don't need a route, since we are globally broadcasting
                _sendWithRoute(null, content);
            }
            else
            {
                lock(_lock)
                {
                    route = _table.GetRoute(destination.Network);
                    if (route == null)
                        _netgramQueue.AddLast(content);
                }

                if(route != null)
                {
                    // we have a route to the destination network, which we can use
                    _sendWithRoute(route, content);
                }
                else
                {
                    _routeSearches.Search(destination.Network, this);
                }
            }
        }

        /// <summary>
        /// Sends an appgram
        /// </summary>
        /// <param name="appgram">The appgram to send</param>
        public void SendAppgram(OutboundAppgram appgram)
        {
            Contract.Requires(appgram != null);
            Contract.Requires(appgram.Destination != null);
            Contract.Requires(appgram.Content != null);

            var header = new NetgramHeader();
            header.Destination = null;
            header.Source = null;
            header.HopCount = 64;
            header.IsNetworkMessage = false;
            header.ExpectingReply = appgram.ExpectingReply;
            header.Priority = appgram.Priority;
            var content = new NetgramContent(appgram.Destination, header, appgram.Content);

            lock (_lock)
            {
                _send(content);
            }
        }

        /// <summary>
        /// Searches for a route to a single network
        /// </summary>
        /// <param name="network">The network to search for</param>
        private void _searchForRouteToNetwork(ushort network)
        {
            WhoIsRouterToNetworkMessage message = new WhoIsRouterToNetworkMessage();
            message.Network = network;
            _sendNetworkMessage(Address.DirectlyConnectedBroadcast, true, NetgramPriority.Normal, message);            
        }
        
        /// <summary>
        /// Sends a network message
        /// </summary>
        /// <param name="destination">The destination address</param>
        /// <param name="expectingReply">True if a reply is expected to this network message, false otherwise</param>
        /// <param name="priority">The netgram priority to send the network message with</param>
        /// <param name="message">The message to send</param>
        private void _sendNetworkMessage(Address destination, bool expectingReply, NetgramPriority priority, INetworkMessage message)
        {
            NetgramHeader header = new NetgramHeader();
            header.IsNetworkMessage = true;
            header.Destination = destination;
            header.HopCount = 64;
            header.ExpectingReply = expectingReply;
            header.Priority = priority;
            header.MessageType = (byte)message.Type;
            var content = new NetgramContent(destination, header, message);
            _send(content);
        }

        /// <summary>
        /// Subscribes to received appgrams
        /// </summary>
        /// <param name="observer">The observer that is subscribing</param>
        /// <returns>The disposable subscription object</returns>
        public IDisposable Subscribe(IObserver<InboundAppgram> observer)
        {
            var ret = new AppgramSubscription(this, observer);
            this._appgramObservers.Register(observer);
            return ret;
        }

        /// <summary>
        /// Performs a search for a route
        /// </summary>
        /// <param name="key">The network number to search for</param>
        void ISearchHandler<ushort>.DoSearch(ushort key)
        {
            _searchForRouteToNetwork(key);
        }

        /// <summary>
        /// Collects queued netgrams for a specific network
        /// </summary>
        /// <param name="network">The network to collect</param>
        /// <returns>The queued netgrams</returns>
        private List<NetgramContent> _collectQueuedNetgrams(ushort network)
        {
            List<NetgramContent> ret = null;

            lock(_lock)
            {
                for(var node = _netgramQueue.First; node != null;)
                {
                    if (node.Value.Destination.Network == network)
                    {
                        var temp = node.Next;
                        ret = ret ?? new List<NetgramContent>();
                        ret.Add(node.Value);
                        _netgramQueue.Remove(node);
                        node = temp;
                    }
                    else
                        node = node.Next;
                }
            }

            return ret;
        }

        /// <summary>
        /// Called when a route has been found for a queued netgram
        /// </summary>
        /// <param name="result">The route that was found</param>
        void ISearchCallback<ushort, Route>.OnFound(Route result)
        {
            var queued = _collectQueuedNetgrams(result.Network);
            foreach (var netgram in queued)
                _sendWithRoute(result, netgram);
        }

        /// <summary>
        /// Called when a route search has timed out for a network
        /// </summary>
        /// <param name="key">The timed out network number</param>
        void ISearchCallback<ushort, Route>.OnTimeout(ushort key)
        {
            // just remove those netgrams, we don't do anything
            // with them
            _collectQueuedNetgrams(key);
        }

        /// <summary>
        /// Called when the port manager receives the next
        /// netgram instance
        /// </summary>
        /// <param name="value">The received netgram</param>
        void IObserver<InboundNetgram>.OnNext(InboundNetgram value)
        {
            var buffer = value.Segment.Buffer;
            var offset = value.Segment.Offset;
            var end = value.Segment.End;

            NetgramHeader header = new NetgramHeader();
            offset = header.Deserialize(buffer, offset, end);
            InboundAppgram appgram = null;

            if (header.IsNetworkMessage)
            {
                _processNetworkMessage(value, header, buffer, offset, end);
            }
            else
            {
                appgram = _createInboundAppgram(value, header, buffer, offset, end);
                _appgramObservers.Next(appgram);
            }
        }

        /// <summary>
        /// Called whenever the port manager receives an error
        /// from an underlying port
        /// </summary>
        /// <param name="error">The received error</param>
        void IObserver<InboundNetgram>.OnError(Exception error)
        {
        }

        /// <summary>
        /// Called when the port manager terminates the
        /// subscription
        /// </summary>
        void IObserver<InboundNetgram>.OnCompleted()
        {
            lock(_lock)
            {
                _disposePortManagerSubscription();
            }
        }

        private class AppgramSubscription : IDisposable
        {
            private Router _router;
            private IObserver<InboundAppgram> _observer;

            public AppgramSubscription(Router router, IObserver<InboundAppgram> observer)
            {
                this._router = router;
                this._observer = observer;
            }

            public void Dispose()
            {
                _router._disposeSubscription(_observer);
            }
        }
        
        
        private class NetgramContent : IContent
        {
            public Address Destination { get; private set; }

            public NetgramHeader Header { get; private set; }
            
            public INetworkMessage NetworkMessage { get; private set; }

            public IContent Content { get; private set; }

            public NetgramContent(Address destination, NetgramHeader header, IContent content)
            {
                this.Destination = destination;
                this.Header = header;
                this.NetworkMessage = null;
                this.Content = content;
            }

            public NetgramContent(Address destination, NetgramHeader header, INetworkMessage networkMessage)
            {
                this.Destination = destination;
                this.Header = header;
                this.NetworkMessage = networkMessage;
            }

            public int Serialize(byte[] buffer, int offset)
            {
                offset = Header.Serialize(buffer, offset);
                if (Header.IsNetworkMessage)
                    offset = NetworkMessage.Serialize(buffer, offset);
                else
                    offset = Content.Serialize(buffer, offset);
                return offset;
            }
        }
    }
}
