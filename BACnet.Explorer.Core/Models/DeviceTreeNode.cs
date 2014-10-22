using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto;
using Eto.Forms;
using Eto.Drawing;
using BACnet.Client;
using BACnet.Client.Descriptors;
using BACnet.Types;
using System.Collections.Specialized;

namespace BACnet.Explorer.Core.Models
{
    public class DeviceTreeNode : ObservableCollectionWithProperties<DeviceTreeNode>, ITreeGridItem, ITreeGridStore<ITreeGridItem>
    {
        /// <summary>
        /// The underlying device info instance
        /// </summary>
        private ObjectInfo _deviceInfo;
        public ObjectInfo DeviceInfo { get { return _deviceInfo; } }

        /// <summary>
        /// Retrieves the device instance
        /// </summary>
        public uint DeviceInstance { get { return _deviceInfo.DeviceInstance; } }

        /// <summary>
        /// Retrieves the object identifier
        /// </summary>
        public ObjectId ObjectIdentifier { get { return _deviceInfo.ObjectIdentifier; } }

        /// <summary>
        /// Retrieves the device name
        /// </summary>
        public string Name { get { return _deviceInfo.Name; } }
        
        /// <summary>
        /// True if the node is expanded, false otherwise
        /// </summary>
        public bool Expanded
        {
            get { return _expanded; }
            set { changeProperty(ref _expanded, value, "Expanded"); }
        }
        private bool _expanded;

        /// <summary>
        /// True if the node is expandable, false otherwise
        /// </summary>
        public bool Expandable
        {
            get
            {
                return this.Count > 0;
            }
            private set
            {
                changeProperty(ref _expandable, value, nameof(Expandable));
            }
        }
        private bool _expandable;

        /// <summary>
        /// Retrieves the parent of this tree item
        /// </summary>
        public ITreeGridItem Parent
        {
            get
            {
                return _parent;
            }

            set
            {
                changeProperty(ref _parent, value, "Parent");
            }
        }
        private ITreeGridItem _parent;

        public new ITreeGridItem this[int index]
        {
            get { return base[index]; }
        }

        /// <summary>
        /// Constructs a new device tree node instance
        /// </summary>
        /// <param name="_deviceInfo">The underlying device info</param>
        public DeviceTreeNode(ObjectInfo deviceInfo)
        {
            this._deviceInfo = deviceInfo;
            this._deviceInfo.PropertyChanged += _deviceInfoPropertyChanged;
            this._expanded = true;
        }

        /// <summary>
        /// Called when a property on the underlying device info changes
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The property changed event args</param>
        private void _deviceInfoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.onPropertyChanged(e.PropertyName);
        }

        /// <summary>
        /// Called whenever the collection changes
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            this.Expandable = this.Count > 0;
            base.OnCollectionChanged(e);
        }
    }
}
