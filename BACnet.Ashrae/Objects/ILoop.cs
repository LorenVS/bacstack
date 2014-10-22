using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Ashrae.Objects
{
    public interface ILoop
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

        [PropertyId(PropertyIdentifier.OutputUnits)]
        EngineeringUnits OutputUnits { get; }

        [PropertyId(PropertyIdentifier.ManipulatedVariableReference)]
        ObjectPropertyReference ManipulatedVariableReference { get; }

        [PropertyId(PropertyIdentifier.ControlledVariableReference)]
        ObjectPropertyReference ControlledVariableReference { get; }

        [PropertyId(PropertyIdentifier.ControlledVariableValue)]
        float ControlledVariableValue { get; }

        [PropertyId(PropertyIdentifier.ControlledVariableUnits)]
        EngineeringUnits ControlledVariableUnits { get; }

        [PropertyId(PropertyIdentifier.SetpointReference)]
        SetpointReference SetpointReference { get; }

        [PropertyId(PropertyIdentifier.Setpoint)]
        float Setpoint { get; }

        [PropertyId(PropertyIdentifier.Action)]
        Action Action { get; }

        [PropertyId(PropertyIdentifier.ProportionalConstant)]
        Option<float> ProportionalConstant { get; }

        [PropertyId(PropertyIdentifier.ProportionalConstantUnits)]
        Option<EngineeringUnits> ProportionalConstantUnits { get; }

        [PropertyId(PropertyIdentifier.IntegralConstant)]
        Option<float> IntegralConstant { get; }

        [PropertyId(PropertyIdentifier.IntegralConstantUnits)]
        Option<EngineeringUnits> IntegralConstantUnits { get; }

        [PropertyId(PropertyIdentifier.DerivativeConstant)]
        Option<float> DerivativeConstant { get; }

        [PropertyId(PropertyIdentifier.DerivativeConstantUnits)]
        Option<EngineeringUnits> DerivativeConstantUnits { get; }

        [PropertyId(PropertyIdentifier.Bias)]
        Option<float> Bias { get; }

        [PropertyId(PropertyIdentifier.MaximumOutput)]
        Option<float> MaximumOutput { get; }

        [PropertyId(PropertyIdentifier.MinimumOutput)]
        Option<float> MinimumOutput { get; }

        [PropertyId(PropertyIdentifier.PriorityForWriting)]
        byte PriorityForWriting { get; }

        [PropertyId(PropertyIdentifier.CovIncrement)]
        Option<float> CovIncrement { get; }

        [PropertyId(PropertyIdentifier.TimeDelay)]
        Option<uint> TimeDelay { get; }

        [PropertyId(PropertyIdentifier.NotificationClass)]
        Option<uint> NotificationClass { get; }

        [PropertyId(PropertyIdentifier.ErrorLimit)]
        Option<float> ErrorLimit { get; }

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
