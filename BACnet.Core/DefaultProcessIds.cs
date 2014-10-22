using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core
{
    public static class DefaultProcessIds
    {
        internal const int Base = 0;
        public const int PortManager = Base + 1;
        public const int Router = Base + 2;
        public const int Host = Base + 3;
    }
}
