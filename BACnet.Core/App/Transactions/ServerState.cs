﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.App.Transactions
{
    public enum ServerState
    {
        Idle,
        SegmentedRequest,
        AwaitResponse,
        SegmentedResponse,

    }
}
