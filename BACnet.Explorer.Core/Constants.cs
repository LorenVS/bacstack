using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Explorer.Core
{
    public static class Constants
    {
        // ui elements
        public const string AddPortButtonText = "Add Port";
        public const string ChooseSessionTitle = "Choose Session";
        public const string CreateSessionButtonText = "Create Session";
        public const string BbmdHostLabel = "BBMD Host";
        public const string BbmdPortLabel = "BBMD Port";
        public const string DeviceNameLabel = "Device";
        public const string ExistingSessionText = "Existing Session";
        public const string LocalHostLabel = "Local Host";
        public const string LocalPortLabel = "Local Port";
        public const string MainFormDefaultTitle = "BACnet Explorer";
        public const string MainFormTitle = "BACnet Explorer = {0}";
        public const string NewSessionText = "New Session";
        public const string PickSessionButtonText = "Connect";
        public const string ProcessNameLabel = "Process Name";
        public const string ProcessIdLabel = "Process Id";
        public const string SessionNameLabel = "Name";

        // default process names
        public const string DeviceFinderDefaultName = "Device Finder";
        public const string ForeignDevicePortDefaultName = "Foreign Device Port";
        public const string EthernetPortDefaultName = "Ethernet Port";
        public const string HostDefaultName = "Host";
        public const string NetworkDatabaseDefaultName = "Network Database";
        public const string PortManagerDefaultName = "Port Manager";
        public const string RouterDefaultName = "Router";
        public const string SessionDefaultName = "New Session";

        // performance
        public const double DefaultRefreshInterval = 30;

        // storage
        public const string SessionsFolder = "sessions";
    }
}
