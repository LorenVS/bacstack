using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BACnet.Types.Schemas;

namespace BACnet.Types
{
    /// <summary>
    /// Handles registration of type and object schemas
    /// </summary>
    public class Registrar
    {
        private ReaderWriterLockSlim _lock;
        private Dictionary<Type, ISchema> _typeSchemasByType;
        private Dictionary<string, ISchema> _typeSchemasByName;
        private Dictionary<uint, ObjectSchema> _objectSchemasByVendorIdAndObjectType;
        private Dictionary<string, ObjectSchema> _objectSchemasByName;
        private Dictionary<Type, ObjectSchema> _objectSchemasByType;

        /// <summary>
        /// Constructs a new registrar instance
        /// </summary>
        public Registrar()
        {
            _lock = new ReaderWriterLockSlim();
            _typeSchemasByType = new Dictionary<Type, ISchema>();
            _typeSchemasByName = new Dictionary<string, ISchema>();
            _objectSchemasByVendorIdAndObjectType = new Dictionary<uint, ObjectSchema>();
            _objectSchemasByName = new Dictionary<string, ObjectSchema>();
            _objectSchemasByType = new Dictionary<Type, ObjectSchema>();
        }

        private uint _getObjectKey(ushort vendorId, ushort objectType)
        {
            uint key = vendorId;
            key <<= 16;
            key |= objectType;
            return key;
        }

        /// <summary>
        /// Registers a collection of types schemas with the registrar
        /// </summary>
        /// <param name="schemas">The schemas to register</param>
        public void RegisterSchemas(IEnumerable<Tuple<string, ISchema>> schemas)
        {
            _lock.EnterWriteLock();
            try
            {
                foreach(var tup in schemas)
                {
                    _typeSchemasByName.Add(tup.Item1, tup.Item2);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Retrieves a type schema by its name
        /// </summary>
        /// <param name="name">The name of the schema</param>
        /// <returns>The type schema, or null if none was found</returns>
        public ISchema GetSchema(string name)
        {
            _lock.EnterReadLock();
            try
            {
                ISchema ret = null;
                _typeSchemasByName.TryGetValue(name, out ret);
                return ret;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Retrieves a type schema by its CLR type
        /// </summary>
        /// <param name="type">The CLR type</param>
        /// <returns>The type schema, or null if none was found</returns>
        public ISchema GetSchema(Type type)
        {
            ISchema ret = null;

            _lock.EnterReadLock();
            try
            {
                _typeSchemasByType.TryGetValue(type, out ret);    
            }
            finally
            {
                _lock.ExitReadLock();
            }

            if(ret == null)
            {
                string name = type.Name;
                ret = GetSchema(name);

                if(ret != null)
                {
                    _lock.EnterWriteLock();
                    try
                    {
                        if(!_typeSchemasByType.ContainsKey(type))
                        {
                            _typeSchemasByType.Add(type, ret);
                        }
                    }
                    finally
                    {
                        _lock.ExitWriteLock();
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Registers a collection of object schemas with the registrar
        /// </summary>
        /// <param name="objects">The object schemas to register</param>
        public void RegisterObjects(IEnumerable<ObjectSchema> objects)
        {
            _lock.EnterWriteLock();
            try
            {
                foreach(var obj in objects)
                {
                    _objectSchemasByVendorIdAndObjectType.Add(
                        _getObjectKey(obj.VendorId, obj.ObjectType),
                        obj);

                    _objectSchemasByName.Add(
                        obj.Name,
                        obj);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Retrieves an object schema from the registrar
        /// </summary>
        /// <param name="vendorId">The vendor id of the object</param>
        /// <param name="objectType">The object type of the object</param>
        /// <param name="allowBase">True to allow an ASHRAE definition to substitute, false otherwise</param>
        /// <returns>The object schema, or null if none was found</returns>
        public ObjectSchema GetObjectSchema(ushort vendorId, ushort objectType, bool allowBase)
        {
            _lock.EnterReadLock();
            try
            {
                uint key = _getObjectKey(vendorId, objectType);
                ObjectSchema ret = null;
                _objectSchemasByVendorIdAndObjectType.TryGetValue(key, out ret);
                if(ret == null && allowBase)
                {
                    key = _getObjectKey(0, objectType);
                    _objectSchemasByVendorIdAndObjectType.TryGetValue(key, out ret);
                }
                return ret;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Retrieves an object schema by name
        /// </summary>
        /// <param name="name">The name of the object</param>
        /// <returns>The object schema, or null if none was found</returns>
        public ObjectSchema GetObjectSchema(string name)
        {
            _lock.EnterReadLock();
            try
            {
                ObjectSchema ret = null;
                _objectSchemasByName.TryGetValue(name, out ret);
                return ret;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Retrieves an object schema by its interface type
        /// </summary>
        /// <param name="type">The interface type</param>
        /// <returns>The object schema instance</returns>
        public ObjectSchema GetObjectSchema(Type type)
        {
            ObjectSchema ret = null;

            _lock.EnterReadLock();
            try
            {
                _objectSchemasByType.TryGetValue(type, out ret);    
            }
            finally
            {
                _lock.ExitReadLock();
            }

            if(ret == null)
            {
                string name = type.Name;
                if(name.StartsWith("I"))
                {
                    // an interface type
                    name = name.Substring(1);
                }
                ret = GetObjectSchema(name);

                if(name != null)
                {
                    _lock.EnterWriteLock();
                    try
                    {
                        if(!_objectSchemasByType.ContainsKey(type))
                        {
                            _objectSchemasByType.Add(type, ret);
                        }
                    }
                    finally
                    {
                        _lock.ExitWriteLock();
                    }
                }
            }

            return ret;
        }
    }
}
