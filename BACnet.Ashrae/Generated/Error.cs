using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class Error
	{
		public ErrorClassType ErrorClass { get; private set; }

		public ErrorCodeType ErrorCode { get; private set; }

		public Error(ErrorClassType errorClass, ErrorCodeType errorCode)
		{
			this.ErrorClass = errorClass;
			this.ErrorCode = errorCode;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ErrorClass", 255, Value<ErrorClassType>.Schema),
			new FieldSchema("ErrorCode", 255, Value<ErrorCodeType>.Schema));

		public static Error Load(IValueStream stream)
		{
			stream.EnterSequence();
			var errorClass = Value<ErrorClassType>.Load(stream);
			var errorCode = Value<ErrorCodeType>.Load(stream);
			stream.LeaveSequence();
			return new Error(errorClass, errorCode);
		}

		public static void Save(IValueSink sink, Error value)
		{
			sink.EnterSequence();
			Value<ErrorClassType>.Save(sink, value.ErrorClass);
			Value<ErrorCodeType>.Save(sink, value.ErrorCode);
			sink.LeaveSequence();
		}
		public enum ErrorClassType : uint
		{
			Device = 0,
			Object = 1,
			Property = 2,
			Resources = 3,
			Security = 4,
			Services = 5,
			Vt = 6
		}
		public enum ErrorCodeType : uint
		{
			Other = 0,
			AuthenticationFailed = 1,
			CharacterSetNotSupported = 41,
			ConfigurationInProgress = 2,
			DatatypeNotSupported = 47,
			DeviceBusy = 3,
			DuplicateName = 48,
			DplicateObjectId = 49,
			DynamicCreationNotSupported = 4,
			FileAccessDenied = 5,
			IncompatibleSecurityLevels = 6,
			InconsistentParameters = 7,
			InconsistentSelectionCriterion = 8,
			InvalidArrayIndex = 42,
			InvalidConfigurationData = 46,
			InvalidDataType = 9,
			InvalidFileAccessMethod = 10,
			InvalidFileStartPosition = 11,
			InvalidOperatorName = 12,
			InvalidParameterDataType = 13,
			InvalidTimeStamp = 14,
			KeyGenerationError = 15,
			MissingRequiredParameter = 16,
			NoObjectsOfSpecifiedType = 17,
			NoSpaceForObject = 18,
			NoSpaceToAddListElement = 19,
			NoSpaceToWriteProperty = 20,
			NoVtSessionsAvailable = 21,
			ObjectDeletionNotPermitted = 23,
			ObjectIdentifierAlreadyExists = 24,
			OperationalProblem = 25,
			OptionalFunctionalityNotSupported = 45,
			PasswordFailure = 26,
			PropertyIsNotAList = 22,
			PropertyIsNotAnArray = 50,
			ReadAccessDenied = 27,
			SecurityNotSupported = 28,
			ServiceRequestDenied = 29,
			Timeout = 30,
			UnknownObject = 31,
			UnknownProperty = 32,
			UnknownVtClass = 34,
			UnknownVtSession = 35,
			UnsupportedObjectType = 36,
			ValueOutOfRange = 37,
			VtSessionAlreadyClosed = 38,
			VtSessionTerminationFailure = 39,
			WriteAccessDenied = 40,
			CovSubscriptionFailed = 43,
			NotCovProperty = 44
		}
	}
}
