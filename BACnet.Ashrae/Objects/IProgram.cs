using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Ashrae.Objects
{
    public interface IProgram
    {

        [PropertyId(PropertyIdentifier.ObjectIdentifier)]
        ObjectId ObjectIdentifier { get; }

        [PropertyId(PropertyIdentifier.ObjectName)]
        string ObjectName { get; }

        [PropertyId(PropertyIdentifier.ObjectType)]
        ObjectType ObjectType { get; }

        [PropertyId(PropertyIdentifier.ProgramState)]
        ProgramState ProgramState { get; }

        [PropertyId(PropertyIdentifier.ProgramChange)]
        ProgramRequest ProgramChange { get; }

        [PropertyId(PropertyIdentifier.ReasonForHalt)]
        Option<ProgramError> ReasonForHalt { get; }

        [PropertyId(PropertyIdentifier.DescriptionOfHalt)]
        Option<string> DescriptionOfHalt { get; }

        [PropertyId(PropertyIdentifier.ProgramLocation)]
        Option<string> ProgramLocation { get; }

        [PropertyId(PropertyIdentifier.Description)]
        Option<string> Description { get; }

        [PropertyId(PropertyIdentifier.InstanceOf)]
        Option<string> InstanceOf { get; }

        [PropertyId(PropertyIdentifier.StatusFlags)]
        StatusFlags StatusFlags { get; }

        [PropertyId(PropertyIdentifier.Reliability)]
        Option<Reliability> Reliability { get; }

        [PropertyId(PropertyIdentifier.OutOfService)]
        bool OutOfService { get; }

        [PropertyId(PropertyIdentifier.ProfileName)]
        Option<string> ProfileName { get; }
    }
}
