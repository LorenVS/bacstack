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
    public class ObjectUpdater<TObj>
    {
        /// <summary>
        /// The client used to send confirmed requests
        /// </summary>
        public Client Client { get; private set; }

        /// <summary>
        /// The device instance of the device containing the object to update
        /// </summary>
        public uint DeviceInstance { get; private set; }

        /// <summary>
        /// The object identifier of the object to update
        /// </summary>
        public ObjectId ObjectIdentifier { get; private set; }

        /// <summary>
        /// The properties to update
        /// </summary>
        private List<PropertyValue> _properties;

        /// <summary>
        /// Creates a new object updater instance
        /// </summary>
        /// <param name="client">The client used to send confirmed requests</param>
        /// <param name="deviceInstance">The device instance of the device containing the object to update</param>
        /// <param name="objectIdentifier">The object identifier of the object to update</param>
        public ObjectUpdater(Client client, uint deviceInstance, ObjectId objectIdentifier)
        {
            this.Client = client;
            this.DeviceInstance = deviceInstance;
            this.ObjectIdentifier = objectIdentifier;
            this._properties = new List<PropertyValue>();
        }

        /// <summary>
        /// Sets a property to be updated
        /// </summary>
        /// <typeparam name="TProp">The type of the property</typeparam>
        /// <param name="propertyExpr">The property expression</param>
        /// <param name="value">The new value of the property</param>
        /// <param name="priority">The priority to write at</param>
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
        /// Updates the object
        /// </summary>
        public void Update()
        {
            var spec = new WriteAccessSpecification(ObjectIdentifier, new ReadOnlyArray<PropertyValue>(_properties));
            var request = new WritePropertyMultipleRequest(new ReadOnlyArray<WriteAccessSpecification>(false, spec));
            var handle = Client.Host.SendConfirmedRequest(DeviceInstance, request);
            if (handle.GetResponse() != null)
                throw new Exception();
        }

        /// <summary>
        /// Updates the object
        /// </summary>
        public Task UpdateAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                this.Update();
            });
        }
    }
}
