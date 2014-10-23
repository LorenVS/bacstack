using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Ashrae.Objects;
using BACnet.Core;
using BACnet.Core.App;
using BACnet.Core.Datalink;
using BACnet.Core.Network;
using BACnet.Client;
using BACnet.IP;
using BACnet.Types;

namespace BACnet.Shell
{

    public class Program
    {
        public static void Main(string[] args)
        {
            ForeignDevicePortOptions options = new ForeignDevicePortOptions()
            {
                PortId = 1,
                BbmdHost = "<bbmd-ip-here>",
                BbmdPort = 47808,
                LocalHost = "0.0.0.0",
                LocalPort = 47808,
                RegistrationInterval = TimeSpan.FromSeconds(30)
            };

            PortManagerOptions portMgrOptions = new PortManagerOptions();
            RouterOptions routerOpts = new RouterOptions();
            routerOpts.PortNetworkMappings.Add(new KeyValuePair<byte, ushort>(1, 0));
            HostOptions hostOpts = new HostOptions();


            using (ForeignDevicePort port = new ForeignDevicePort(options))
            using (PortManager manager = new PortManager(portMgrOptions))
            using (Router router = new Router(routerOpts))
            using (Host host = new Host(hostOpts))
            using (Session session = new Session(port, manager, router, host))
            {
                var client = new BACnet.Client.Client(host);

                var name = client.With(300111)
                    .ReadProperty(dev => dev.ObjectName);

                Console.WriteLine(name);

                var bufferSize = client.With<ITrendLog>(300100, new ObjectId(20, 1))
                    .ReadProperty(tl => tl.BufferSize);

                Console.WriteLine(bufferSize);
            }

        }
    }
}
