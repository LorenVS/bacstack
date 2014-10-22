using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Ashrae.Objects
{
    public interface IEventEnrollment
    {

        [PropertyId(PropertyIdentifier.ObjectIdentifier)]
        ObjectId ObjectIdentifier { get; }

        [PropertyId(PropertyIdentifier.ObjectName)]
        string ObjectName { get; }

        [PropertyId(PropertyIdentifier.ObjectType)]
        ObjectType ObjectType { get; }

        [PropertyId(PropertyIdentifier.Description)]
        Option<string> Description { get; }

        [PropertyId(PropertyIdentifier.EventType)]
        EventType EventType { get; }

        [PropertyId(PropertyIdentifier.NotifyType)]
        NotifyType NotifyType { get; }

        [PropertyId(PropertyIdentifier.EventParameters)]
        EventParameter EventParameters { get; }

        [PropertyId(PropertyIdentifier.ObjectPropertyReference)]
        DeviceObjectPropertyReference ObjectPropertyReference { get; }

        [PropertyId(PropertyIdentifier.EventState)]
        EventState EventState { get; }

        [PropertyId(PropertyIdentifier.EventEnable)]
        EventTransitionBits EventEnable { get; }

        [PropertyId(PropertyIdentifier.AckedTransitions)]
        EventTransitionBits AckedTransitions { get; }

        [PropertyId(PropertyIdentifier.NotificationClass)]
        Option<uint> NotificationClass { get; }

        [PropertyId(PropertyIdentifier.EventTimeStamps)]
        ReadOnlyArray<TimeStamp> EventTimeStamps { get; }

        [PropertyId(PropertyIdentifier.ProfileName)]
        Option<string> ProfileName { get; }
    }
}
