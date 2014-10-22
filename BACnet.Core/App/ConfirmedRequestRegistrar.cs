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
    /// ConfirmedRequests
    /// </summary>
    public class ConfirmedRequestRegistrar
    {
        /// <summary>
        /// The confirmed service types which have been
        /// registered
        /// </summary>
        private List<Registration> _registrations;

        /// <summary>
        /// Constructs a new ConfirmedRequestRegistrar instance
        /// </summary>
        public ConfirmedRequestRegistrar()
        {
            _registrations = new List<Registration>();
        }

        /// <summary>
        /// Registers an confirmed request type with the registrar
        /// </summary>
        /// <param name="serviceChoice">The service choice of the request type</param>
        /// <param name="schema">The schema for the confirmed request type</param>
        /// <param name="loader">The loader function for the request type</param>
        /// <param name="saver">The saver function for the request type</param>
        public void Register(ConfirmedServiceChoice serviceChoice, ISchema schema, Func<IValueStream, IConfirmedRequest> loader, Action<IValueSink, IConfirmedRequest> saver)
        {
            for(int i = 0; i < _registrations.Count; i++)
            {
                if (_registrations[i].ServiceChoice == serviceChoice
                    && _registrations[i].Loader != loader)
                {
                    throw new Exception("Service choice is already registered");
                }
                else if (_registrations[i].ServiceChoice == serviceChoice)
                    return;
            }

            Registration registration = new Registration(serviceChoice, schema, loader, saver);
            this._registrations.Add(registration);
        }

        /// <summary>
        /// Registers an confirmed request type with the registrar
        /// </summary>
        /// <typeparam name="T">The confirmed service choice type</typeparam>
        /// <param name="serviceChoice">The service choice of the request type</param>
        public void Register<T>(ConfirmedServiceChoice serviceChoice)
            where T : class, IConfirmedRequest
        {
            for (int i = 0; i < _registrations.Count; i++)
            {
                if (_registrations[i].ServiceChoice == serviceChoice)
                    throw new Exception("Service choice is already registered");
            }

            Registration registration = new Registration(serviceChoice, Value<T>.Schema, Value<T>.Loader,
                (sink, req) => Value<T>.Saver(sink, (T)req));
            this._registrations.Add(registration);
        }

        /// <summary>
        /// Creates a new confirmed request from a value stream
        /// </summary>
        /// <param name="serviceChoice">The service choice of the request to create</param>
        /// <param name="stream">The value stream instance</param>
        /// <returns>The confirmed request</returns>
        public IConfirmedRequest Create(ConfirmedServiceChoice serviceChoice, IValueStream stream)
        {
            for(int i = 0; i < _registrations.Count; i++)
            {
                if(_registrations[i].ServiceChoice == serviceChoice)
                {
                    return _registrations[i].Loader(stream);
                }
            }

            throw new Exception("Could not create request for unregistered service: " + serviceChoice);
        }

        /// <summary>
        /// Retrieves the registration instance for a specific confirmed request type
        /// </summary>
        /// <param name="serviceChoice">The service choice of the confirmed request type</param>
        /// <returns>The registration instance</returns>
        public Registration GetRegistration(ConfirmedServiceChoice serviceChoice)
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
        public ConfirmedRequestRegistrar Clone()
        {
            var cloned = new ConfirmedRequestRegistrar();
            for(int i = 0; i < _registrations.Count; i++)
            {
                var reg = _registrations[i];
                cloned.Register(reg.ServiceChoice, reg.Schema, reg.Loader, reg.Saver);
            }
            return cloned;
        }


        public struct Registration
        {
            /// <summary>
            /// The service choice for this type of request
            /// </summary>
            public ConfirmedServiceChoice ServiceChoice { get; private set; }

            /// <summary>
            /// The schema for the confirmed request type
            /// </summary>
            public ISchema Schema { get; private set; }

            /// <summary>
            /// Loading function, which creates the service choice
            /// from a value stream
            /// </summary>
            public Func<IValueStream, IConfirmedRequest> Loader { get; private set; }

            /// <summary>
            /// Saving function, which saves the service choice
            /// to a value sink
            /// </summary>
            public Action<IValueSink, IConfirmedRequest> Saver { get; private set; }

            /// <summary>
            /// Constructs a new Registration instance
            /// </summary>
            /// <param name="serviceChoice">The service choice of the confirmed service</param>
            /// <param name="schema">The schema for the confirmed request type</param>
            /// <param name="loader">The loader function, which creates a confirmed request from a value stream</param>
            /// <param name="saver">The saving function, which saves a confirmed request to a value sink</param>
            public Registration(ConfirmedServiceChoice serviceChoice, ISchema schema, Func<IValueStream, IConfirmedRequest> loader, Action<IValueSink, IConfirmedRequest> saver) : this()
            {
                this.ServiceChoice = serviceChoice;
                this.Schema = schema;
                this.Loader = loader;
                this.Saver = saver;
            }
        }
    }
}
