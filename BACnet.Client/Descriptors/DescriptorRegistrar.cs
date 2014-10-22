using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Client.Descriptors
{
    /// <summary>
    /// Registrar for various types of descriptors
    /// </summary>
    public class DescriptorRegistrar
    {
        /// <summary>
        /// The registrations that have been made with the registrar
        /// </summary>
        private Dictionary<uint, Registration> _registrations;

        /// <summary>
        /// The default registration used for unregistered object types
        /// </summary>
        private Registration _defaultRegistration;

        /// <summary>
        /// Constructs a new DescriptorRegistrar instance
        /// </summary>
        public DescriptorRegistrar()
        {
            _registrations = new Dictionary<uint, Registration>();

            _defaultRegistration = new Registration(
                0,
                0,
                (vi, di, oi) => new ObjectInfo(vi, di, oi));
        }

        /// <summary>
        /// Creates an identical copy of this registrar
        /// </summary>
        /// <returns>The cloned instance</returns>
        public DescriptorRegistrar Clone()
        {
            var clone = new DescriptorRegistrar();
            foreach(var kv in this._registrations)
            {
                clone._registrations.Add(kv.Key, kv.Value);
            }
            clone._defaultRegistration = this._defaultRegistration;
            return clone;
        }

        /// <summary>
        /// Registers a new descriptor type with the registrar
        /// </summary>
        /// <param name="objectType">The object type of the registration</param>
        /// <param name="vendorId">The vendor id of the registration</param>
        /// <param name="factory">The factory function for the descriptors</param>
        public void Register(ushort objectType, ushort vendorId, DescriptorFactory factory)
        {
            var registration = new Registration(objectType, vendorId, factory);
            _register(registration);
        }

        /// <summary>
        /// Registers a new default descriptor type with the registrar
        /// </summary>
        /// <param name="objectType">The object type of the registration</param>
        /// <param name="factory">The factory function for the descriptors</param>
        public void Register(ushort objectType, DescriptorFactory factory)
        {
            var registration = new Registration(objectType, 0, factory);
            _register(registration);
        }

        /// <summary>
        /// Creates a new descriptor for an object
        /// </summary>
        /// <param name="vendorId">The vendor id of the object</param>
        /// <param name="objectType">The object type of the object</param>
        /// <returns>The descriptor instance</returns>
        public ObjectInfo CreateDescriptor(ushort vendorId, uint deviceInstance, ObjectId objectIdentifier)
        {
            var registration = _getRegistration(vendorId, objectIdentifier.Type);
            return registration.Factory(vendorId, deviceInstance, objectIdentifier);
        }

        /// <summary>
        /// Registers a new registration instance
        /// </summary>
        /// <param name="registration">The registration</param>
        private void _register(Registration registration)
        {
            uint key = _getKey(registration.VendorId, registration.ObjectType);
            if (_registrations.ContainsKey(key))
                throw new Exception("Key already registered");
            _registrations.Add(key, registration);
        }

        /// <summary>
        /// Retrieves a registration key
        /// </summary>
        /// <param name="vendorId">The vendor id</param>
        /// <param name="objectType">The object type</param>
        /// <returns>The key</returns>
        private uint _getKey(ushort vendorId, ushort objectType)
        {
            uint key = vendorId;
            key <<= 16;
            key |= objectType;
            return key;
        }

        /// <summary>
        /// Retrieves a suitable registration for an object
        /// </summary>
        /// <param name="vendorId">The vendor id of the object</param>
        /// <param name="objectType">The object type of the object</param>
        /// <returns>The registration</returns>
        private Registration _getRegistration(ushort vendorId, ushort objectType)
        {
            Registration ret;
            uint key;

            key = _getKey(vendorId, objectType);
            if (_registrations.TryGetValue(key, out ret))
                return ret;

            key = _getKey(0, objectType);
            if (_registrations.TryGetValue(key, out ret))
                return ret;

            return _defaultRegistration;            
        }

        public class Registration
        {
            /// <summary>
            /// The object type that this descriptor type is used for
            /// </summary>
            public ushort ObjectType { get; private set; }

            /// <summary>
            /// The vendor id that this descriptor type is used for,
            /// 0 is used for ASHRAE, the default vendor, which all
            /// vendors will defer to
            /// </summary>
            public ushort VendorId { get; private set; }

            /// <summary>
            /// The factory function used to create descriptors
            /// </summary>
            public DescriptorFactory Factory { get; private set; }

            /// <summary>
            /// Constructs a new registration instance
            /// </summary>
            /// <param name="objectType">The object type of the registration</param>
            /// <param name="vendorId">The vendor id of the registration, or 0 for an ASHRAE registration</param>
            /// <param name="factory">Factory function for creating new descriptor instances</param>
            public Registration(ushort objectType, ushort vendorId, DescriptorFactory factory)
            {
                this.ObjectType = objectType;
                this.VendorId = vendorId;
                this.Factory = factory;
            }
        }
    }
}
