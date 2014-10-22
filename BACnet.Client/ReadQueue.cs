using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Types;

namespace BACnet.Client
{
    public class ReadQueue
    {
        /// <summary>
        /// The client
        /// </summary>
        public Client Client { get; private set; }
        
        /// <summary>
        /// The read requests
        /// </summary>
        private List<IPropertyRequest> _requests;

        /// <summary>
        /// Constructs a new read queue instance
        /// </summary>
        /// <param name="client">The client</param>
        public ReadQueue(Client client)
        {
            this.Client = client;
            this._requests = new List<IPropertyRequest>();
        }

        /// <summary>
        /// Retreives a new object handle for queueing properties for a specific object
        /// </summary>
        /// <typeparam name="TObj">The type of object</typeparam>
        /// <param name="deviceInstance">The device instance of the object</param>
        /// <param name="objectIdentifier">The object identifier of the object</param>
        /// <returns>The object handle</returns>
        public ReadQueueObjectHandle<TObj> With<TObj>(uint deviceInstance, ObjectId objectIdentifier)
        {
            return new ReadQueueObjectHandle<TObj>(this, deviceInstance, objectIdentifier);
        }

        /// <summary>
        /// Enqueues a new property to be read
        /// </summary>
        /// <typeparam name="TProp">The type of the property</typeparam>
        /// <param name="deviceInstance">The device instance of the object</param>
        /// <param name="reference">The reference to the property</param>
        /// <param name="onValue">The value handler for when the property is read</param>
        /// <param name="onError">The error handler for when an error occurs</param>
        public void Enqueue<TProp>(uint deviceInstance, ObjectPropertyReference reference, Action<TProp> onValue, Action<Error> onError = null)
        {
            PropertyRequest<TProp> request = new PropertyRequest<TProp>(
                deviceInstance,
                reference,
                onValue,
                onError);
            _requests.Add(request);
        }

        /// <summary>
        /// Sends the appropriate read requests to read all queued properties
        /// </summary>
        public void Send()
        {
            var byDevice = _requests.GroupBy(req => req.DeviceInstance);
            foreach(var device in byDevice)
            {
                var requests = device.ToArray();
                var references = requests.Select(req => req.Reference).ToArray();
                var results = Client.SendRPMForReadResults(device.Key, references);
                for(int i = 0; i < requests.Length; i++)
                {
                    requests[i].Resolve(results[i]);
                }
            }
        }

        /// <summary>
        /// Sends a group of properties requests for a single device
        /// </summary>
        /// <param name="grouping">The grouping</param>
        /// <returns>The asynchronous task</returns>
        private async Task _sendGroup(IGrouping<uint, IPropertyRequest> grouping)
        {
            var requests = grouping.ToArray();
            var references = requests.Select(req => req.Reference).ToArray();
            var results = await Client.SendRPMForReadResultsAsync(grouping.Key, references);
            for (int i = 0; i < requests.Length; i++)
            {
                requests[i].Resolve(results[i]);
            }
        }

        /// <summary>
        /// Sends the appropriate read requests to read all queued properties
        /// and resolves the requests on a marshalled thread
        /// </summary>
        public Task SendAsync()
        {
            List<Task> tasks = new List<Task>();
            foreach(var group in _requests.GroupBy(req => req.DeviceInstance))
            {
                tasks.Add(_sendGroup(group));
            }
            _requests.Clear();
            return Task.WhenAll(tasks);
        }

        public struct ReadQueueObjectHandle<TObj>
        {
            /// <summary>
            /// The parent queue
            /// </summary>
            public ReadQueue Queue { get; private set; }

            /// <summary>
            /// The device instance of the object
            /// </summary>
            public uint DeviceInstance { get; private set; }

            /// <summary>
            /// The object that is being wrapped
            /// </summary>
            public ObjectId ObjectIdentifier { get; private set; }

            public ReadQueueObjectHandle(ReadQueue queue, uint deviceInstance, ObjectId objectIdentifier) : this()
            {
                this.Queue = queue;
                this.DeviceInstance = deviceInstance;
                this.ObjectIdentifier = objectIdentifier;
            }

            /// <summary>
            /// Enqueues a property for reading
            /// </summary>
            /// <typeparam name="TProp">The type of property to read</typeparam>
            /// <param name="expression">The expression of the property to read</param>
            /// <param name="onValue">The action to invoke when a value is read</param>
            /// <param name="onError">The action to invoke when an error results</param>
            public void Enqueue<TProp>(Expression<Func<TObj, TProp>> expression, Action<TProp> onValue, Action<Error> onError = null)
            {
                var reference = ObjectHelpers.GetObjectPropertyReference(ObjectIdentifier, expression);
                var request = new PropertyRequest<TProp>(
                    DeviceInstance,
                    reference,
                    onValue,
                    onError);
                Queue._requests.Add(request);
            }
        }

        private interface IPropertyRequest
        {
            /// <summary>
            /// The device instance of the property to read
            /// </summary>
            uint DeviceInstance { get; }

            /// <summary>
            /// The reference to the property to read
            /// </summary>
            ObjectPropertyReference Reference { get; }

            /// <summary>
            /// Resolves the property request with a read result
            /// </summary>
            /// <param name="readResult">The read result</param>
            void Resolve(ReadAccessResult.ReadResultType readResult);
        }

        private class PropertyRequest<T> : IPropertyRequest
        {
            /// <summary>
            /// The device instance of the property to read
            /// </summary>
            public uint DeviceInstance { get; private set; }

            /// <summary>
            /// The reference to the property to read
            /// </summary>
            public ObjectPropertyReference Reference { get; private set; }

            /// <summary>
            /// Action to invoke when the request is resolved with a propert value
            /// </summary>
            public Action<T> OnValue { get; private set; }

            /// <summary>
            /// Action to invoke when the request is resolved with an error
            /// </summary>
            public Action<Error> OnError { get; private set; }

            /// <summary>
            /// Constructs a new property request
            /// </summary>
            public PropertyRequest(uint deviceInstance, ObjectPropertyReference reference, Action<T> onValue, Action<Error> onError = null)
            {
                this.DeviceInstance = deviceInstance;
                this.Reference = reference;
                this.OnValue = onValue;
                this.OnError = onError;
            }

            /// <summary>
            /// Resolves the property request with a read result
            /// </summary>
            /// <param name="readResult">The read result</param>
            public void Resolve(ReadAccessResult.ReadResultType readResult)
            {
                if(readResult.IsPropertyAccessError)
                {
                    if (this.OnError != null)
                        this.OnError(readResult.AsPropertyAccessError);
                }
                else if(readResult.IsPropertyValue)
                {
                    if(this.OnValue != null)
                    {
                        try
                        {
                            var value = readResult.AsPropertyValue.As<T>();
                            this.OnValue(value);
                        }
                        catch(Exception)
                        {
                            var error = new Error(Error.ErrorClassType.Property, Error.ErrorCodeType.InvalidDataType);
                            if (this.OnError != null)
                                this.OnError(error);
                        }
                    }
                }
            }
        }
    }
}
