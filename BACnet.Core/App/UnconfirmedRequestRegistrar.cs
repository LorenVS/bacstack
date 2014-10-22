using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Types;

namespace BACnet.Core.App
{
    /// <summary>
    /// Registrar instance which can create instances of
    /// UnconfirmedRequests
    /// </summary>
    public class UnconfirmedRequestRegistrar
    {
        /// <summary>
        /// The unconfirmed service types which have been
        /// registered
        /// </summary>
        private readonly List<IRegistration> _registrations;

        /// <summary>
        /// Constructs a new UnconfirmedRequestRegistrar instance
        /// </summary>
        public UnconfirmedRequestRegistrar()
        {
            _registrations = new List<IRegistration>();
        }

        /// <summary>
        /// Registers an unconfirmed request type with the registrar
        /// </summary>
        /// <param name="serviceChoice">The service choice of the request type</param>
        public void Register<T>(UnconfirmedServiceChoice serviceChoice) where T : IUnconfirmedRequest
        {
            for(int i = 0; i < _registrations.Count; i++)
            {
                if (_registrations[i].ServiceChoice == serviceChoice)
                    throw new Exception("Service choice is already registered");
            }

            Registration<T> registration = new Registration<T>(serviceChoice);
            this._registrations.Add(registration);
        }


        /// <summary>
        /// Retrieves the registration instance for a specific unconfirmed request type
        /// </summary>
        /// <param name="serviceChoice">The service choice of the unconfirmed request type</param>
        /// <returns>The registration instance</returns>
        public IRegistration GetRegistration(UnconfirmedServiceChoice serviceChoice)
        {
            for(int i = 0; i < _registrations.Count; i++)
            {
                if(_registrations[i].ServiceChoice == serviceChoice)
                {
                    return _registrations[i];
                }
            }

            throw new Exception("Could not retrieve registration for unregistered service: " + serviceChoice);
        }

        /// <summary>
        /// Clones this registrar instance
        /// </summary>
        /// <returns>The cloned instance</returns>
        public UnconfirmedRequestRegistrar Clone()
        {
            var cloned = new UnconfirmedRequestRegistrar();
            cloned._registrations.AddRange(this._registrations);
            return cloned;
        }

        public interface IRegistration
        {
            /// <summary>
            /// The service choice for the registration
            /// </summary>
            UnconfirmedServiceChoice ServiceChoice { get; }

            /// <summary>
            /// The schema for the registration
            /// </summary>
            ISchema Schema { get; }

            /// <summary>
            /// Loads an unconfirmed request
            /// </summary>
            /// <param name="stream">The value stream to load from</param>
            /// <returns>The loaded unconfirmed request</returns>
            IUnconfirmedRequest Load(IValueStream stream);

            /// <summary>
            /// Saves an unconfirmed request
            /// </summary>
            /// <param name="sink">The sink to save to</param>
            /// <param name="request">The request to save</param>
            void Save(IValueSink sink, IUnconfirmedRequest request);
        }

        private class Registration<T> : IRegistration where T : IUnconfirmedRequest
        {
            /// <summary>
            /// The service choice for the registration
            /// </summary>
            public UnconfirmedServiceChoice ServiceChoice { get; private set; }

            /// <summary>
            /// The schema for the registration
            /// </summary>
            public ISchema Schema { get { return Value<T>.Schema; } }

            /// <summary>
            /// Loads an unconfirmed request
            /// </summary>
            /// <param name="stream">The value stream to load from</param>
            /// <returns>THe loaded unconfirmed request</returns>
            public IUnconfirmedRequest Load(IValueStream stream)
            {
                return Value<T>.Load(stream);
            }

            /// <summary>
            /// Saves an unconfirmed request
            /// </summary>
            /// <param name="sink">The sink to save to</param>
            /// <param name="request">The request to save</param>
            public void Save(IValueSink sink, IUnconfirmedRequest request)
            {
                Value<T>.Save(sink, (T)request);
            }

            /// <summary>
            /// Constructs a new registration instance
            /// </summary>
            /// <param name="serviceChoice">The service choice of the registration</param>
            public Registration(UnconfirmedServiceChoice serviceChoice)
            {
                this.ServiceChoice = serviceChoice;
            }
        }
    }
}
