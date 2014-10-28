using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Ashrae.Objects;
using BACnet.Types;

namespace BACnet.Client
{
    public static class Extensions
    {
        /// <summary>
        /// Retrieves active alarms for the device
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <param name="scope">The object scope</param>
        /// <returns>The active alarms</returns>
        public static async Task<ReadOnlyArray<GetAlarmSummaryAck.Element>> GetAlarmsAsync<T>(this ObjectHandle<T> scope) where T : IDevice
        {
            var request = new GetAlarmSummaryRequest();
            var ack = await scope.Client.SendRequestAsync<GetAlarmSummaryAck>(
                scope.DeviceInstance,
                request);
            return ack.Item;
        }

        /// <summary>
        /// Retrieves active alarms for the device
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <param name="scope">The object scope</param>
        /// <returns>The active alarms</returns>
        public static ReadOnlyArray<GetAlarmSummaryAck.Element> GetAlarms<T>(this ObjectHandle<T> scope) where T : IDevice
        {
            return scope.GetAlarmsAsync().Result;
        }
    }
}
