using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Ashrae.Objects;
using BACnet.Core.App;
using BACnet.Core.App.Transactions;
using BACnet.Tagging;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Client
{
    public class Client
    {
        /// <summary>
        /// The host used for sending and receiving appgrams
        /// </summary>
        public Host Host { get; private set; }

        /// <summary>
        /// The read queue for queueing property reads
        /// </summary>
        public ReadQueue ReadQueue { get; private set; }

        /// <summary>
        /// Constructs a new Client instance
        /// </summary>
        /// <param name="host">The host used for sending and receiving appgrams</param>
        public Client(Host host)
        {
            this.Host = host;
            this.ReadQueue = new ReadQueue(this);
        }

        /// <summary>
        /// Converts a generic value to a strongly typed value
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="schema">The schema of the value</param>
        /// <returns>The strongly typed vlaue</returns>
        private T _valueAs<T>(GenericValue value)
        {
            var schema = Value<T>.Schema;
            var stream = value.Provider.CreateStream(value, schema);
            return Value<T>.Loader(stream);
        }
        
        /// <summary>
        /// Sends a request and retrieves the response as a specific type
        /// </summary>
        /// <typeparam name="T">The response type</typeparam>
        /// <param name="deviceInstance">The device instance of the destination device</param>
        /// <param name="request">The request to send</param>
        /// <returns>The response value</returns>
        public Task<TAck> SendRequestAsync<TAck>(uint deviceInstance, IConfirmedRequest request)
            where TAck : IComplexAck
        {
            var handle = new ComplexAckHandle<TAck>();
            Host.SendConfirmedRequest(handle, deviceInstance, request);
            return handle.GetResponseAsync();
        }

        /// <summary>
        /// Sends a request, expecting a simple ack in response
        /// </summary>
        /// <param name="deviceInstance">The device instance of the destination device</param>
        /// <param name="request">The request to send</param>
        /// <returns>The task to wait for a simple ack in response</returns>
        public Task SendRequestAsync(uint deviceInstance, IConfirmedRequest request)
        {
            var handle = new SimpleAckHandle();
            Host.SendConfirmedRequest(handle, deviceInstance, request);
            return handle.WaitAsync();
        }

        /// <summary>
        /// Creates a read property multiple request for reading
        /// properties from a single object
        /// </summary>
        /// <param name="objectIdentifier">The object identifier of the object to read</param>
        /// <param name="properties">The properties to read</param>
        /// <returns>The read property multiple request</returns>
        private ReadPropertyMultipleRequest _createRPM(ObjectId objectIdentifier, params PropertyIdentifier[] properties)
        {
            PropertyReference[] references = new PropertyReference[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                references[i] = new PropertyReference(properties[i]);
            }

            return _createRPM(objectIdentifier, references);
        }

        /// <summary>
        /// Creates a read property multiple request
        /// for reading properties from a single object
        /// </summary>
        /// <param name="objectIdentifier">The object identifier of the object to read</param>
        /// <param name="propertyReferences">The property references to read</param>
        /// <returns>The read property multiple request</returns>
        private ReadPropertyMultipleRequest _createRPM(ObjectId objectIdentifier, params PropertyReference[] propertyReferences)
        {
            return new ReadPropertyMultipleRequest(
                new ReadOnlyArray<ReadAccessSpecification>(false,
                    new ReadAccessSpecification(
                        objectIdentifier,
                        new ReadOnlyArray<PropertyReference>(false,
                            propertyReferences))));
        }

        /// <summary>
        /// Extracts the array of generic values from a read property multiple ack
        /// </summary>
        /// <param name="ack">The read property multiple ack</param>
        /// <returns>The array of generic values</returns>
        private GenericValue[] _extractValues(ReadPropertyMultipleAck ack)
        {
            List<GenericValue> values = new List<GenericValue>();
            foreach(var results in ack.ListOfReadAccessResults)
            {
                if (!results.ListOfResults.HasValue)
                    continue;

                foreach(var result in results.ListOfResults.Value)
                {
                    var readResult = result.ReadResult;
                    if (readResult.IsPropertyValue)
                        values.Add(readResult.AsPropertyValue);
                }
            }
            return values.ToArray();
        }

        /// <summary>
        /// Extracts an array of read results from a read property multiple ack
        /// </summary>
        /// <param name="ack">The read propety multiple ack</param>
        /// <returns>The array of read results</returns>
        private ReadAccessResult.ReadResultType[] _extractReadResults(ReadPropertyMultipleAck ack)
        {
            List<ReadAccessResult.ReadResultType> values = new List<ReadAccessResult.ReadResultType>();

            foreach(var results in ack.ListOfReadAccessResults)
            {
                if (!results.ListOfResults.HasValue)
                    continue;

                foreach(var result in results.ListOfResults.Value)
                {
                    if(result.ReadResult.IsPropertyAccessError
                        && result.ReadResult.AsPropertyAccessError.ErrorCode == Error.ErrorCodeType.UnknownProperty)
                    {
                        // for optional properties, we handle a non-existing properties
                        // with a Option<T> instance with no value, rather than an error
                        // code
                        values.Add(ReadAccessResult.ReadResultType.NewPropertyValue(
                            TaggedGenericValue.Empty));
                    }
                    else
                        values.Add(result.ReadResult);
                }
            }

            return values.ToArray();
        }

        /// <summary>
        /// Sends a read property multiple request
        /// and retrieves the results as generic values
        /// </summary>
        /// <param name="deviceInstance">The device instance to send to</param>
        /// <param name="objectIdentifier">The object identifier of the object to read</param>
        /// <param name="references">The references of the properties to read</param>
        /// <returns>The generic values</returns>
        internal async Task<GenericValue[]> SendRPMAsync(uint deviceInstance, ObjectId objectIdentifier, params PropertyReference[] references)
        {
            var request = _createRPM(objectIdentifier, references);
            var ack = await SendRequestAsync<ReadPropertyMultipleAck>(deviceInstance, request);
            var values = _extractValues(ack);

            if (values.Length != references.Length)
                throw new Exception();

            return values;
        }

        /// <summary>
        /// Sends a read property multiple request
        /// and retrieves the results as ReadResult instances
        /// </summary>
        /// <param name="deviceInstance">The device instance to read from</param>
        /// <param name="references">The references to read</param>
        /// <returns>The read results</returns>
        internal async Task<ReadAccessResult.ReadResultType[]> SendRPMForReadResultsAsync(uint deviceInstance, params ObjectPropertyReference[] references)
        {
            List<ReadAccessSpecification> specs = new List<ReadAccessSpecification>();
            ReadPropertyMultipleRequest request = null;
            var groups = references.GroupBy(r => r.ObjectIdentifier);

            foreach(var group in groups)
            {
                specs.Add(new ReadAccessSpecification(
                    group.Key,
                    new ReadOnlyArray<PropertyReference>(group.Select(r => new PropertyReference(r.PropertyIdentifier, r.PropertyArrayIndex)))
                ));
            }

            request = new ReadPropertyMultipleRequest(new ReadOnlyArray<ReadAccessSpecification>(specs));
            var ack = await SendRequestAsync<ReadPropertyMultipleAck>(deviceInstance, request);
            var values = _extractReadResults(ack);

            if (values.Length != references.Length)
                throw new Exception();

            return values;
        }

        /// <summary>
        /// Sends a read property multiple request
        /// and retrieves the results as ReadResult instances
        /// </summary>
        /// <param name="deviceInstance">The device instance to read from</param>
        /// <param name="references">The references to read</param>
        /// <returns>The read results</returns>
        internal ReadAccessResult.ReadResultType[] SendRPMForReadResults(uint deviceInstance, params ObjectPropertyReference[] references)
        {
            return SendRPMForReadResultsAsync(deviceInstance, references).Result;
        }

        /// <summary>
        /// Transforms a read result into an ErrorOr value
        /// </summary>
        /// <typeparam name="T">The type of value</typeparam>
        /// <param name="result">The read result</param>
        /// <returns>The ErrorOr instance</returns>
        internal ErrorOr<T> FromReadResult<T>(ReadAccessResult.ReadResultType result)
        {
            if (result.IsPropertyAccessError)
                return new ErrorOr<T>(result.AsPropertyAccessError);
            else
                return new ErrorOr<T>(result.AsPropertyValue.As<T>());
        }
        
        /// <summary>
        /// Creates an object scope, which allows strongly
        /// typed operations to be performed on a single object
        /// </summary>
        /// <typeparam name="TObj">The type of object</typeparam>
        /// <param name="deviceInstance">The device instance of the device</param>
        /// <param name="objectIdentifier">The object identifier of the object</param>
        /// <returns>The object handle</returns>
        public ObjectHandle<TObj> With<TObj>(uint deviceInstance, ObjectId objectIdentifier)
        {
            return new ObjectHandle<TObj>(this, deviceInstance, objectIdentifier);
        }

        /// <summary>
        /// Creates an object scope for a device object, which allows
        /// strongly typed operations to be performed on the device
        /// </summary>
        /// <param name="deviceInstance">The device instance of the device</param>
        /// <returns>The object handle for the device</returns>
        public ObjectHandle<IDevice> With(uint deviceInstance)
        {
            return new ObjectHandle<IDevice>(this, deviceInstance,
                new ObjectId((ushort)ObjectType.Device, deviceInstance));
        }

        /// <summary>
        /// Creates a new object creator, which enables creation
        /// of a new object on a remote device
        /// </summary>
        /// <typeparam name="TObj">The type of object to create</typeparam>
        /// <param name="deviceInstance">The device instance to create the object on</param>
        /// <param name="objectIdentifier">The object identifier of the new object</param>
        /// <returns>The object creator instance</returns>
        public ObjectCreator<TObj> Create<TObj>(uint deviceInstance, ObjectId objectIdentifier)
        {
            return new ObjectCreator<TObj>(this, deviceInstance, objectIdentifier);
        }

        /// <summary>
        /// Creates a new object creator, which enables creation
        /// of a new object on a remote device
        /// </summary>
        /// <typeparam name="TObj">The type of object to create</typeparam>
        /// <param name="deviceInstance">The device instance to create the object on</param>
        /// <param name="objectIdentifier">The object type of the new object</param>
        /// <returns>The object creator instance</returns>
        public ObjectCreator<TObj> Create<TObj>(uint deviceInstance, ObjectType objectType)
        {
            return new ObjectCreator<TObj>(this, deviceInstance, objectType);
        }

        /// <summary>
        /// Creates an object updater, which can be used to update properties
        /// on a remote object
        /// </summary>
        /// <typeparam name="TObj">The type of object to update</typeparam>
        /// <param name="deviceInstance">The device that contains the object to update</param>
        /// <param name="objectIdentifier">The object identifier of the object to update</param>
        /// <returns>The object updater instance</returns>
        public ObjectUpdater<TObj> Update<TObj>(uint deviceInstance, ObjectId objectIdentifier)
        {
            return new ObjectUpdater<TObj>(this, deviceInstance, objectIdentifier);
        }
    }
}
