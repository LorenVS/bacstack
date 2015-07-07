using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types.Schemas
{
    public class ObjectSchema
    {
        /// <summary>
        /// The vendor id of the vendor that defined the object, 0 for ashrae
        /// </summary>
        public ushort VendorId { get; private set; }

        /// <summary>
        /// The object type of the object
        /// </summary>
        public ushort ObjectType { get; private set; }

        /// <summary>
        /// The name of the object type
        /// </summary>
        public string Name { get; private set; }

        private Dictionary<ushort, PropertySchema> _propertiesById;
        private Dictionary<string, PropertySchema> _propertiesByName;

        /// <summary>
        /// Constructs a new ObjectSchema instance
        /// </summary>
        /// <param name="vendorId">The id of the vendor that defined the object</param>
        /// <param name="objectType">The object type of the object</param>
        /// <param name="name">The name of the object</param>
        /// <param name="properties">The properties defined for the object</param>
        public ObjectSchema(ushort vendorId, ushort objectType, string name, IEnumerable<PropertySchema> properties)
        {
            this.VendorId = vendorId;
            this.ObjectType = objectType;
            this.Name = name;
            this._propertiesById = properties.ToDictionary(prop => prop.PropertyId);
            this._propertiesByName = properties.ToDictionary(prop => prop.Name);
        }

        /// <summary>
        /// Retrieves a property for this object schema
        /// by the property id
        /// </summary>
        /// <param name="id">The property id</param>
        /// <returns>The property schema, or null if no matching property was found</returns>
        public PropertySchema GetPropertyById(ushort id)
        {
            PropertySchema ret = null;
            _propertiesById.TryGetValue(id, out ret);
            return ret;
        }

        /// <summary>
        /// Retrieves a property for this object schema
        /// by the property name
        /// </summary>
        /// <param name="name">The name of the property</param>
        /// <returns>The property schema, or null if no matching property was found</returns>
        public PropertySchema GetPropertyByName(string name)
        {
            PropertySchema ret = null;
            _propertiesByName.TryGetValue(name, out ret);
            return ret;
        }
    }
}
