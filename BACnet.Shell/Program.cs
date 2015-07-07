using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using BACnet.Ashrae;
using BACnet.Ashrae.Objects;
using BACnet.Core;
using BACnet.Core.App;
using BACnet.Core.Datalink;
using BACnet.Core.Network;
using BACnet.Client;
using BACnet.Ethernet;
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
                BbmdHost = "142.33.64.30",
                BbmdPort = 47808,
                LocalHost = "0.0.0.0",
                LocalPort = 47808,
                RegistrationInterval = TimeSpan.FromSeconds(30)
            };

            PortManagerOptions portMgrOptions = new PortManagerOptions();
            RouterOptions routerOpts = new RouterOptions();
            routerOpts.PortNetworkMappings.Add(new KeyValuePair<byte, ushort>(1, 0));
            HostOptions hostOpts = new HostOptions();
            DeviceFinderOptions finderOpts = new DeviceFinderOptions();

            using (ForeignDevicePort port = new ForeignDevicePort(options))
            using (PortManager manager = new PortManager(portMgrOptions))
            using (Router router = new Router(routerOpts))
            using (Host host = new Host(hostOpts))
            using (DeviceFinder finder = new DeviceFinder(finderOpts))
            using (Session session = new Session(port, manager, router, host, finder))
            {
                var client = new BACnet.Client.Client(host);

                // as long as there is at least 1 new devices found every 10 seconds,
                // for each found device, read that devices name and print it to the console

                foreach(var device in finder.Timeout(TimeSpan.FromSeconds(10))
                    .Catch(Observable.Empty<DeviceTableEntry>())
                    .ToEnumerable())
                {
                    var name = client.With(device.Instance).ReadProperty(dev => dev.ObjectName);
                    Console.WriteLine(name);
                }
            }

        }
    }
}
