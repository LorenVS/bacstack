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

            hostOpts.UnconfirmedRegistrar.Register<IAmRequest>(UnconfirmedServiceChoice.IAm);
            hostOpts.UnconfirmedRegistrar.Register<WhoIsRequest>(UnconfirmedServiceChoice.WhoIs);

            hostOpts.ConfirmedRegistrar.Register<ReadPropertyRequest>(ConfirmedServiceChoice.ReadProperty);
            hostOpts.ConfirmedRegistrar.Register<ReadPropertyMultipleRequest>(ConfirmedServiceChoice.ReadPropertyMultiple);
            hostOpts.ConfirmedRegistrar.Register<WritePropertyRequest>(ConfirmedServiceChoice.WriteProperty);
            hostOpts.ConfirmedRegistrar.Register<ReadRangeRequest>(ConfirmedServiceChoice.ReadRange);

            var dbOptions = new NetworkDatabaseOptions();

            using(ForeignDevicePort port = new ForeignDevicePort(options))
            using(PortManager manager = new PortManager(portMgrOptions))
            using(Router router = new Router(routerOpts))
            using (Host host = new Host(hostOpts))
            //using (NetworkDatabase db = new NetworkDatabase(dbOptions))
            {
                var processes = new IProcess[] { port, manager, router, host };//, db };

                foreach(var p in processes.OfType<IPort>())
                {
                    p.Open();
                }

                foreach(var process in processes)
                {
                    process.Resolve(processes);
                }


                var client = new BACnet.Client.Client(host);

                var name = client.With(300111)
                    .ReadProperty(dev => dev.ObjectName);

                var bufferSize = client.With<ITrendLog>(300100, new ObjectId(20, 1))
                    .ReadProperty(tl => tl.BufferSize);

                Console.WriteLine(bufferSize);

                var logRecords = client.With<ITrendLog>(300100, new ObjectId(20, 1))
                    .ReadRangeByPosition(tl => tl.LogBuffer, 1, 100);

                foreach(var record in logRecords)
                {
                    if(record.LogDatum.IsRealValue)
                    {
                        Console.WriteLine("{0}: {1}", record.Timestamp.ToDateTime(), record.LogDatum.AsRealValue);
                    }
                }
                
                System.Threading.Thread.Sleep(10000);
            }

        }
    }
}
