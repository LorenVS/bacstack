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
    public class ObjectCreator<TObj>
    {
        /// <summary>
        /// The client used to send confirmed requests
        /// </summary>
        public Client Client { get; private set; }

        /// <summary>
        /// The device instance of the device to create the object on
        /// </summary>
        public uint DeviceInstance { get; private set; }

        /// <summary>
        /// The object specifier of the object to create
        /// </summary>
        public CreateObjectRequest.ObjectSpecifierType ObjectSpecifier { get; private set; }

        /// <summary>
        /// The property values to create the object with
        /// </summary>
        private List<PropertyValue> _properties;

        /// <summary>
        /// Creates a new object creator instance
        /// </summary>
        /// <param name="client">The client used to send confirmed requests</param>
        /// <param name="deviceInstance">The device instance of the device to create the object on</param>
        /// <param name="objectIdentifier">The object identifier of the object to create</param>
        public ObjectCreator(Client client, uint deviceInstance, ObjectId objectIdentifier)
        {
            this.Client = client;
            this.DeviceInstance = deviceInstance;
            this.ObjectSpecifier = CreateObjectRequest.ObjectSpecifierType.NewObjectIdentifier(objectIdentifier);
            this._properties = new List<PropertyValue>();
        }

        /// <summary>
        /// Creates a new object creator instance
        /// </summary>
        /// <param name="client">The client used to send confirmed requests</param>
        /// <param name="deviceInstance">The device instance of the device to create the object on</param>
        /// <param name="objectType">The type of object to create</param>
        public ObjectCreator(Client client, uint deviceInstance, ObjectType objectType)
        {
            this.Client = client;
            this.DeviceInstance = deviceInstance;
            this.ObjectSpecifier = CreateObjectRequest.ObjectSpecifierType.NewObjectType(objectType);
            this._properties = new List<PropertyValue>();
        }

        /// <summary>
        /// Sets a property value to create the object with
        /// </summary>
        /// <typeparam name="TProp">The type of the property</typeparam>
        /// <param name="propertyExpr">The property expression</param>
        /// <param name="value">The value of the propertys</param>
        public void Set<TProp>(Expression<Func<TObj, TProp>> propertyExpr, TProp value, Option<uint> priority = default(Option<uint>))
        {
            var reference = ObjectHelpers.GetPropertyReference(propertyExpr);
            var propertyValue = new PropertyValue(
                reference.PropertyIdentifier,
                reference.PropertyArrayIndex,
                Tagging.Tags.Encode(value),
                priority);
            this._properties.Add(propertyValue);
        }

        /// <summary>
        /// Creates the object
        /// </summary>
        public void Create()
        {
            var request = new CreateObjectRequest(ObjectSpecifier, new ReadOnlyArray<PropertyValue>(_properties));
            var handle = Client.Host.SendConfirmedRequest(DeviceInstance, request);
            if (handle.GetResponse() != null)
                throw new Exception();
        }

        /// <summary>
        /// Creates the object
        /// </summary>
        public Task CreateAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                this.Create();
            });
        }
    }
}
