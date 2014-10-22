using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Ashrae.Objects
{
    public interface INotificationClass
    {

        [PropertyId(PropertyIdentifier.ObjectIdentifier)]
        ObjectId ObjectIdentifier { get; }

        [PropertyId(PropertyIdentifier.ObjectName)]
        string ObjectName { get; }

        [PropertyId(PropertyIdentifier.ObjectType)]
        ObjectType ObjectType { get; }

        [PropertyId(PropertyIdentifier.Description)]
        Option<string> Description { get; }

        [PropertyId(PropertyIdentifier.NotificationClass)]
        uint NotificationClass { get; }

        [PropertyId(PropertyIdentifier.Priority)]
        ReadOnlyArray<uint> Priority { get; }

        [PropertyId(PropertyIdentifier.AckRequired)]
        EventTransitionBits AckRequired { get; }

        [PropertyId(PropertyIdentifier.RecipientList)]
        ReadOnlyArray<Destination> RecipientList { get; }

        [PropertyId(PropertyIdentifier.ProfileName)]
        Option<string> ProfileName { get; }
    }
}
