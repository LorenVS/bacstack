using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto;
using Eto.Forms;
using BACnet.Client;
using BACnet.Client.Descriptors;

namespace BACnet.Explorer.Core.Models
{
    public class DeviceTreeStore : ObservableCollection<DeviceTreeNode>, ITreeGridStore<DeviceTreeNode>, IDescriptorObserver<ObjectInfo, GlobalObjectId>
    {
        /// <summary>
        /// The eto application, which is used to invoke back onto the UI thread
        /// </summary>
        private Application _application;

        /// <summary>
        /// Constructs a new device tree store instance
        /// </summary>
        /// <param name="application">The eto application</param>
        public DeviceTreeStore(Application application)
        {
            this._application = application;
        }

        /// <summary>
        /// Determines whether a device is a parent of another
        /// in the tree
        /// </summary>
        /// <param name="parent">The parent</param>
        /// <param name="child">The child</param>
        /// <returns>True if <see cref="parent"/> is a parent of <see cref="child"/></returns>
        private bool _isParentOf(DeviceTreeNode parent, ObjectInfo child)
        {
            uint parentInstance = parent.DeviceInstance;
            uint childInstance = child.DeviceInstance;

            if (parent.DeviceInstance % 100 == 0 && child.DeviceInstance / 100 == parent.DeviceInstance / 100)
                return true;
            else if (parent.DeviceInstance % 10000 == 0 && child.DeviceInstance / 10000 == parent.DeviceInstance / 10000)
                return true;
            return false;
        }

        /// <summary>
        /// Merges a new device into the tree
        /// </summary>
        /// <param name="value">The device info to merge</param>
        private void _merge(ObjectInfo value)
        {
            // for now, we use the Delta Controls 10000/100 standard tree
            // structure, hopefully we can make this configurable in the future

            ObservableCollection<DeviceTreeNode> collection = this;
            var node = new DeviceTreeNode(value);
            bool merged = false;

        recurse:
            for(int i = 0; i < collection.Count; i++)
            {
                var parent = collection[i];
                if(_isParentOf(parent, value))
                {
                    collection = parent;
                    goto recurse;
                }
                else if(parent.DeviceInstance > value.DeviceInstance)
                {
                    collection.Insert(i, node);
                    for(int j = i + 1; j < collection.Count; j++)
                    {
                        if (!_isParentOf(node, collection[j].DeviceInfo))
                            break;
                        var child = collection[j];
                        collection.RemoveAt(j);
                        node.Add(child);
                        j--;
                    }
                    merged = true;
                    break;
                }
            }

            if(!merged)
            {
                collection.Add(node);
            }
        }

        /// <summary>
        /// Finds the device tree node for a corresponding object info instance
        /// </summary>
        /// <param name="info">The object info instance</param>
        /// <returns>The device tree node, or null if none was found</returns>
        private DeviceTreeNode _find(ObjectInfo info)
        {
            ObservableCollection<DeviceTreeNode> collection = this;
        recurse:
            for(int i = 0; i < collection.Count; i++)
            {
                var parent = collection[i];
                if (parent.DeviceInfo.Key == info.Key)
                    return parent;
                else if(_isParentOf(parent, info))
                {
                    collection = parent;
                    goto recurse;
                }
            }
            return null;
        }

#region IDescriptorObserver Implementation

        void IDescriptorObserver<ObjectInfo, GlobalObjectId>.Close()
        {
        }

        void IDescriptorObserver<ObjectInfo, GlobalObjectId>.Add(IEnumerable<ObjectInfo> values)
        {
            _application.AsyncInvoke(() =>
            {
                foreach (var value in values)
                {
                    _merge((ObjectInfo)value.Clone());
                }
            });
        }

        void IDescriptorObserver<ObjectInfo, GlobalObjectId>.Add(ObjectInfo value)
        {
            _application.AsyncInvoke(() =>
            {
                _merge((ObjectInfo)value.Clone());
            });
        }
        
        void IDescriptorObserver<ObjectInfo, GlobalObjectId>.Remove(IEnumerable<ObjectInfo> values)
        {
            _application.AsyncInvoke(() =>
            {

            });
        }

        void IDescriptorObserver<ObjectInfo, GlobalObjectId>.Remove(ObjectInfo value)
        {
            _application.AsyncInvoke(() =>
            {

            });
        }


        void IDescriptorObserver<ObjectInfo, GlobalObjectId>.Update(ObjectInfo value)
        {
            _application.AsyncInvoke(() =>
            {
                var node = _find(value);
                if (node != null)
                    node.DeviceInfo.CopyFrom(value);
            });
        }

        void IDescriptorObserver<ObjectInfo, GlobalObjectId>.Update(IEnumerable<ObjectInfo> values)
        {
            _application.AsyncInvoke(() =>
            {
                foreach (var value in values)
                {
                    var node = _find(value);
                    if (node != null)
                        node.DeviceInfo.CopyFrom(value);
                }
            });
        }


        #endregion

    }
}
