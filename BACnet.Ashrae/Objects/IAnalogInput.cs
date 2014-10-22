using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Ashrae.Objects
{
    public interface IAnalogInput
    {
        [PropertyId(PropertyIdentifier.ObjectIdentifier)]
        ObjectId ObjectIdentifier { get; }

        [PropertyId(PropertyIdentifier.ObjectName)]
        string ObjectName { get; }

        [PropertyId(PropertyIdentifier.ObjectType)]
        ObjectType ObjectType { get; }

        [PropertyId(PropertyIdentifier.PresentValue)]
        float PresentValue { get; }

        [PropertyId(PropertyIdentifier.Description)]
        Option<string> Description { get; }

        [PropertyId(PropertyIdentifier.DeviceType)]
        Option<string> DeviceType { get; }

        [PropertyId(PropertyIdentifier.StatusFlags)]
        StatusFlags StatusFlags { get; }

        [PropertyId(PropertyIdentifier.EventState)]
        EventState EventState { get; }

        [PropertyId(PropertyIdentifier.Reliability)]
        Option<Reliability> Reliability { get; }

        [PropertyId(PropertyIdentifier.OutOfService)]
        bool OutOfService { get; }

        [PropertyId(PropertyIdentifier.UpdateInterval)]
        Option<uint> UpdateInterval { get; }

        [PropertyId(PropertyIdentifier.Units)]
        EngineeringUnits Units { get; }

        [PropertyId(PropertyIdentifier.MinPresValue)]
        Option<float> MinPresValue { get; }

        [PropertyId(PropertyIdentifier.MaxPresValue)]
        Option<float> MaxPresValue { get; }

        [PropertyId(PropertyIdentifier.Resolution)]
        Option<float> Resolution { get; }

        [PropertyId(PropertyIdentifier.CovIncrement)]
        Option<float> CovIncrement { get; }

        [PropertyId(PropertyIdentifier.TimeDelay)]
        Option<uint> TimeDelay { get; }

        [PropertyId(PropertyIdentifier.NotificationClass)]
        Option<uint> NotificationClass { get; }

        [PropertyId(PropertyIdentifier.HighLimit)]
        Option<float> HighLimit { get; }

        [PropertyId(PropertyIdentifier.LowLimit)]
        Option<float> LowLimit { get; }

        [PropertyId(PropertyIdentifier.Deadband)]
        Option<float> Deadband { get; }

        [PropertyId(PropertyIdentifier.LimitEnable)]
        Option<LimitEnable> LimitEnable { get; }

        [PropertyId(PropertyIdentifier.EventEnable)]
        Option<EventTransitionBits> EventEnable { get; }

        [PropertyId(PropertyIdentifier.AckedTransitions)]
        Option<EventTransitionBits> AckedTransitions { get; }

        [PropertyId(PropertyIdentifier.NotifyType)]
        Option<NotifyType> NotifyType { get; }

        [PropertyId(PropertyIdentifier.EventTimeStamps)]
        Option<ReadOnlyArray<TimeStamp>> EventTimeStamps { get; }

        [PropertyId(PropertyIdentifier.ProfileName)]
        Option<string> ProfileName { get; }
    }
}
