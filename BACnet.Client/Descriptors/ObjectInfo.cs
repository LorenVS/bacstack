using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BACnet.Ashrae.Objects;
using BACnet.Types;

namespace BACnet.Client.Descriptors
{
    public class ObjectInfo : PropertyChangedBase, ICloneable, ISyncable, IStronglyKeyed<GlobalObjectId>
    {
        /// <summary>
        /// The global object id of the object info
        /// </summary>
        [JsonIgnore]
        public GlobalObjectId Key
        {
            get { return new GlobalObjectId(DeviceInstance, ObjectIdentifier); }
        }

        /// <summary>
        /// The vendor id of the object
        /// </summary>
        [JsonIgnore]
        public ushort VendorId { get; private set; }

        /// <summary>
        /// The device instance of the object
        /// </summary>
        [JsonIgnore]
        public uint DeviceInstance { get; private set; }

        /// <summary>
        /// The object identifier of the object
        /// </summary>
        [JsonIgnore]
        public ObjectId ObjectIdentifier { get; private set; }

        /// <summary>
        /// The name of the object
        /// </summary>
        [JsonIgnore]
        public string Name
        {
            get { return _name; }
            set { changeProperty(ref _name, value, "Name"); }
        }
        private string _name;

        /// <summary>
        /// Constructs a new object info instance
        /// </summary>
        /// <param name="vendorId">The vendor id of the object</param>
        /// <param name="deviceInstance">The device instance of the object</param>
        /// <param name="objectIdentifier">The object identifier of the object</param>
        public ObjectInfo(ushort vendorId, uint deviceInstance, ObjectId objectIdentifier)
        {
            this.VendorId = vendorId;
            this.DeviceInstance = deviceInstance;
            this.ObjectIdentifier = objectIdentifier;
        }
        
        /// <summary>
        /// Copies the properties of this object from another instance
        /// </summary>
        /// <param name="other">The other instance</param>
        public virtual void CopyFrom(object other)
        {
            var oi = other as ObjectInfo;
            if(oi != null)
            {
                this.Name = oi.Name;
            }
        }

        /// <summary>
        /// Clones this object
        /// </summary>
        /// <returns>The cloned instance</returns>
        public virtual object Clone()
        {
            var clone = new ObjectInfo(VendorId, DeviceInstance, ObjectIdentifier);
            clone.CopyFrom(this);
            return clone;
        }


        /// <summary>
        /// Syncs updates from another instance to this one
        /// </summary>
        /// <param name="other">The instance to sync from</param>
        /// <param name="name">The name of the property to sync</param>
        protected virtual void syncFrom(object other, string name)
        {
            var info = other as ObjectInfo;
            if (info != null)
            {
                switch (name)
                {
                    case "Name":
                        this.Name = info.Name;
                        break;
                }
            }
        }

        /// <summary>
        /// Syncs updates from another instance to this one
        /// </summary>
        /// <param name="other">The instance to sync from</param>
        /// <param name="name">The name of the property to sync</param>
        public void SyncFrom(object other, string name)
        {
            syncFrom(other, name);
        }

        /// <summary>
        /// Refresh the descriptor
        /// </summary>
        /// <param name="queue">The read queue</param>
        public virtual void Refresh(ReadQueue queue)
        {
            var handle = queue.With<INamedObject>(DeviceInstance, ObjectIdentifier);
            handle.Enqueue(obj => obj.ObjectName, name => this.Name = name);
        }

        /// <summary>
        /// Loads additional properties for the descriptor
        /// from a JSON string
        /// </summary>
        /// <param name="props">The JSON string</param>
        public virtual void LoadProperties(string props)
        {
            JsonConvert.PopulateObject(props, this);
        }

        /// <summary>
        /// Saves additional properties for the descriptor
        /// to a JSON string
        /// </summary>
        /// <returns>The JSON string</returns>
        public virtual string SaveProperties()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
