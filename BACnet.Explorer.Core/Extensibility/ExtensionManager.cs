using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Explorer.Core.Controls;

namespace BACnet.Explorer.Core.Extensibility
{
    public static class ExtensionManager
    {
        /// <summary>
        /// Lock synchronizing access to the manager
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// The interfaces that the manager is currently watching for
        /// </summary>
        private static readonly List<Type> _interfaces = new List<Type>();

        /// <summary>
        /// The known types that implement any of the watched interfaces
        /// </summary>
        private static readonly HashSet<Type> _types = new HashSet<Type>();

        /// <summary>
        /// Instances of the watched types
        /// </summary>
        private static readonly List<object> _instances = new List<object>();
        
        /// <summary>
        /// Registers the assembly listener
        /// </summary>
        static ExtensionManager()
        {
            AppDomain.CurrentDomain.AssemblyLoad += _assemblyLoad;
        }

        /// <summary>
        /// Called whenever a new assembly is loaded
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="args">The assembly load event args</param>
        private static void _assemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            var newTypes = args.LoadedAssembly.GetTypes()
                .Where(t => _interfaces.Any(i => i.IsAssignableFrom(t)))
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .ToArray();

            foreach(var type in newTypes)
            {
                var instance = Activator.CreateInstance(type);
                _instances.Add(type);
                _types.Add(type);
            }
        }


        /// <summary>
        /// Retrieves all of the extensions that conform to a certain interface
        /// </summary>
        /// <typeparam name="T">The interface type</typeparam>
        /// <returns>The extension instances</returns>
        public static T[] GetExtensions<T>() where T : class
        {
            Contract.Requires(typeof(T).IsInterface);

            lock(_lock)
            {
                if (_interfaces.Contains(typeof(T)))
                {
                    return _instances.OfType<T>().ToArray();
                }
                else
                {
                    return _addInterface<T>();
                }
            }
        }

        /// <summary>
        /// Retrieves a generic extension that conforms to a certain interface
        /// </summary>
        /// <typeparam name="TBase">The base extension interface type</typeparam>
        /// <typeparam name="TGen">The generic extension interface type</typeparam>
        /// <returns>The generic extension instance</returns>
        public static TGen GetGenericExtension<TBase, TGen>() where TBase : class where TGen : class, TBase
        {
            Contract.Requires(typeof(TBase).IsInterface);
            Contract.Requires(typeof(TGen).IsInterface);

            lock(_lock)
            {
                if(_interfaces.Contains(typeof(TBase)))
                {
                    return _instances.OfType<TGen>().FirstOrDefault();
                }
                else
                {
                    var arr = _addInterface<TBase>();
                    return arr.OfType<TGen>().FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// Creates an editor from a registered
        /// editor provider
        /// </summary>
        /// <typeparam name="T">The type of editor to create</typeparam>
        /// <returns>The editor instance</returns>
        public static IEditor<T> CreateEditor<T>()
        {
            var provider = GetExtensions<IEditorProvider>()
                .FirstOrDefault(ep => ep.ProvidesEditorFor<T>());

            if (provider == null)
                throw new Exception("Can't create a new editor for " + typeof(T).Name);

            return provider.CreateEditor<T>();
        }

        /// <summary>
        /// Adds an interface to the watched interfaces
        /// </summary>
        /// <param name="type">The type to add</param>
        /// <returns></returns>
        private static T[] _addInterface<T>()
        {
            var newTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => !_types.Contains(t))
                .Where(t => typeof(T).IsAssignableFrom(t))
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Where(t => !t.IsGenericTypeDefinition)
                .ToArray();
            
            foreach(var type in newTypes)
            {
                var instance = Activator.CreateInstance(type);
                _instances.Add(instance);
                _types.Add(type);
            }

            return _instances.OfType<T>().ToArray();
        }
    }
}
