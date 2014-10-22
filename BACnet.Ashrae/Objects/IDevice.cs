using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Ashrae.Objects
{
    public interface IDevice
    {
        [PropertyId(PropertyIdentifier.ObjectIdentifier)]
        ObjectId ObjectIdentifier { get; }

        [PropertyId(PropertyIdentifier.ObjectName)]
        string ObjectName { get; }

        [PropertyId(PropertyIdentifier.ObjectType)]
        ObjectType ObjectType { get; }

        [PropertyId(PropertyIdentifier.SystemStatus)]
        DeviceStatus SystemStatus { get; }

        [PropertyId(PropertyIdentifier.VendorName)]
        string VendorName { get; }

        [PropertyId(PropertyIdentifier.VendorIdentifier)]
        ushort VendorIdentifier { get; }

        [PropertyId(PropertyIdentifier.ModelName)]
        string ModelName { get; }

        [PropertyId(PropertyIdentifier.FirmwareRevision)]
        string FirmwareRevision { get; }

        [PropertyId(PropertyIdentifier.ApplicationSoftwareVersion)]
        string ApplicationSoftwareVersion { get; }

        [PropertyId(PropertyIdentifier.Location)]
        Option<string> Location { get; }

        [PropertyId(PropertyIdentifier.Description)]
        Option<string> Description { get; }

        [PropertyId(PropertyIdentifier.ProtocolVersion)]
        uint ProtocolVersion { get; }

        [PropertyId(PropertyIdentifier.ProtocolRevision)]
        uint ProtocolRevision { get; }

        [PropertyId(PropertyIdentifier.ProtocolServicesSupported)]
        ServicesSupported ProtocolServicesSupported { get; }

        [PropertyId(PropertyIdentifier.ProtocolObjectTypesSupported)]
        ObjectTypesSupported ProtocolObjectTypesSupported { get; }

        [PropertyId(PropertyIdentifier.ObjectList)]
        ReadOnlyArray<ObjectId> ObjectList { get; }

        [PropertyId(PropertyIdentifier.MaxApduLengthAccepted)]
        uint MaxApduLengthAccepted { get; }

        [PropertyId(PropertyIdentifier.SegmentationSupported)]
        Segmentation SegmentationSupported { get; }

        [PropertyId(PropertyIdentifier.VtClassesSupported)]
        Option<ReadOnlyArray<VTClass>> VtClassesSupported { get; }

        [PropertyId(PropertyIdentifier.ActiveVtSessions)]
        Option<ReadOnlyArray<VTSession>> ActiveVtSessions { get; }

        [PropertyId(PropertyIdentifier.LocalTime)]
        Option<Time> LocalTime { get; }

        [PropertyId(PropertyIdentifier.LocalDate)]
        Option<Date> LocalDate { get; }

        [PropertyId(PropertyIdentifier.UtcOffset)]
        Option<int> UtcOffset { get; }

        [PropertyId(PropertyIdentifier.DaylightSavingsStatus)]
        Option<bool> DaylightSavingsStatus { get; }

        [PropertyId(PropertyIdentifier.ApduSegmentTimeout)]
        uint ApduSegmentTimeout { get; }

        [PropertyId(PropertyIdentifier.ApduTimeout)]
        uint ApduTimeout { get; }

        [PropertyId(PropertyIdentifier.NumberOfAPDURetries)]
        uint NumberOfAPDURetries { get; }

        [PropertyId(PropertyIdentifier.ListOfSessionKeys)]
        Option<ReadOnlyArray<SessionKey>> ListOfSessionKeys { get; }

        [PropertyId(PropertyIdentifier.TimeSynchronizationRecipients)]
        Option<ReadOnlyArray<Recipient>> TimeSynchronizationRecipients { get; }

        [PropertyId(PropertyIdentifier.MaxMaster)]
        Option<byte> MaxMaster { get; }

        [PropertyId(PropertyIdentifier.MaxInfoFrames)]
        Option<byte> MaxInfoFrames { get; }

        [PropertyId(PropertyIdentifier.DeviceAddressBinding)]
        ReadOnlyArray<AddressBinding> DeviceAddressBinding { get; }

        [PropertyId(PropertyIdentifier.DatabaseRevision)]
        uint DatabaseRevision { get; }

        [PropertyId(PropertyIdentifier.ConfigurationFiles)]
        ReadOnlyArray<ObjectId> ConfigurationFiles { get; }

        [PropertyId(PropertyIdentifier.LastRestoreTime)]
        TimeStamp LastRestoreTime { get; }

        [PropertyId(PropertyIdentifier.BackupFailureTimeout)]
        ushort BackupFailureTimeout { get; }

        [PropertyId(PropertyIdentifier.ActiveCovSubscriptions)]
        ReadOnlyArray<COVSubscription> ActiveCovSubscriptions { get; }

        [PropertyId(PropertyIdentifier.MaxSegmentsAccepted)]
        uint MaxSegmentsAccepted { get; }

        [PropertyId(PropertyIdentifier.AutoSlaveDiscovery)]
        Option<ReadOnlyArray<bool>> AutoSlaveDiscovery { get; }

        [PropertyId(PropertyIdentifier.SlaveAddressBinding)]
        Option<ReadOnlyArray<AddressBinding>> SlaveAddressBinding { get; }

        [PropertyId(PropertyIdentifier.ManualSlaveAddressBinding)]
        Option<ReadOnlyArray<AddressBinding>> ManualSlaveAddressBinding { get; }

        [PropertyId(PropertyIdentifier.ProfileName)]
        Option<string> ProfileName { get; }
    }
}
