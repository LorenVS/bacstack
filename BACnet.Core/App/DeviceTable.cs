using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Core.Network;

namespace BACnet.Core.App
{
    public class DeviceTable
    {
        /// <summary>
        /// The entries in the device table
        /// </summary>
        private readonly DeviceTableEntry[] _entries;
        
        /// <summary>
        /// The first entry in the table
        /// </summary>
        private int _start = 0;

        /// <summary>
        /// The number of entries in the table
        /// </summary>
        private int _count = 0;

        /// <summary>
        /// Constructs a new device table instance
        /// </summary>
        /// <param name="capacity">The capacity of the device table</param>
        public DeviceTable(int capacity = 256)
        {
            this._entries = new DeviceTableEntry[capacity];
            this._start = 0;
            this._count = 0;
        }

        /// <summary>
        /// Upserts a new entry into the device table
        /// </summary>
        /// <param name="entry">The entry to upsert</param>
        public void Upsert(DeviceTableEntry entry)
        {
            for(int i = 0; i < _count; i++)
            {
                int index = (_start + i) % _entries.Length;
                var oldEntry = _entries[index];
                if (oldEntry != null &&  oldEntry.Instance == entry.Instance)
                {
                    _entries[index] = entry;
                    return;
                }
            }

            if(_count < _entries.Length)
            {
                int insertIndex = (_start + _count) % _entries.Length;
                _entries[insertIndex] = entry;
                _count++;
            }
            else
            {
                _entries[_start] = entry;
                _start = (_start + 1) % _entries.Length;
            }
        }

        /// <summary>
        /// Gets a device table entry by instance
        /// </summary>
        /// <param name="instance">The device instance</param>
        /// <returns>The device table entry, or null if no entry exists</returns>
        public DeviceTableEntry Get(uint instance)
        {
            for (int i = 0; i < _count; i++)
            {
                int index = (_start + i) % _entries.Length;
                var entry = _entries[index];
                if (entry != null && entry.Instance == instance)
                    return entry;
            }
            return null;
        }

        /// <summary>
        /// Gets a device table entry by address
        /// </summary>
        /// <param name="address">The device's address</param>
        /// <returns>The device table entry, or null if no entry exists</returns>
        public DeviceTableEntry GetByAddress(Address address)
        {
            for(int i = 0; i < _count; i++)
            {
                int index = (_start + i) % _entries.Length;
                var entry = _entries[index];
                if (entry != null && entry.Address == address)
                    return entry;
            }
            return null;
        }

    }
}
