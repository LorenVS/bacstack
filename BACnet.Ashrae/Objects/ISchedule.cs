using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Ashrae.Objects
{
    public interface ISchedule
    {

        [PropertyId(PropertyIdentifier.ObjectIdentifier)]
        ObjectId ObjectIdentifier { get; }

        [PropertyId(PropertyIdentifier.ObjectName)]
        string ObjectName { get; }

        [PropertyId(PropertyIdentifier.ObjectType)]
        ObjectType ObjectType { get; }

        [PropertyId(PropertyIdentifier.PresentValue)]
        GenericValue PresentValue { get; }

        [PropertyId(PropertyIdentifier.Description)]
        Option<string> Description { get; }

        [PropertyId(PropertyIdentifier.EffectivePeriod)]
        DateRange EffectivePeriod { get; }

        [PropertyId(PropertyIdentifier.WeeklySchedule)]
        Option<ReadOnlyArray<DailySchedule>> WeeklySchedule { get; }

        [PropertyId(PropertyIdentifier.ScheduleDefault)]
        GenericValue ScheduleDefault { get; }

        [PropertyId(PropertyIdentifier.ExceptionSchedule)]
        Option<ReadOnlyArray<SpecialEvent>> ExceptionSchedule { get; }

        [PropertyId(PropertyIdentifier.ListOfObjectPropertyReferences)]
        ReadOnlyArray<DeviceObjectPropertyReference> ListOfObjectPropertyReferences { get; }

        [PropertyId(PropertyIdentifier.PriorityForWriting)]
        byte PriorityForWriting { get; }

        [PropertyId(PropertyIdentifier.StatusFlags)]
        StatusFlags StatusFlags { get; }

        [PropertyId(PropertyIdentifier.Reliability)]
        Reliability Reliability { get; }

        [PropertyId(PropertyIdentifier.OutOfService)]
        bool OutOfService { get; }

        [PropertyId(PropertyIdentifier.ProfileName)]
        Option<string> ProfileName { get; }
    }
}
