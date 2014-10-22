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
        public static ReadOnlyArray<GetAlarmSummaryAck.Element> GetAlarms<T>(this ObjectHandle<T> scope) where T : IDevice
        {
            var request = new GetAlarmSummaryRequest();
            var handle = scope.Client.Host.SendConfirmedRequest(scope.DeviceInstance, request);
            var ack = scope.Client.ResponseAs<GetAlarmSummaryAck>(handle);
            return ack.Item;
        }

        /// <summary>
        /// Retrieves active alarms for the device
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <param name="scope">The object scope</param>
        /// <returns>The active alarms</returns>
        public static Task<ReadOnlyArray<GetAlarmSummaryAck.Element>> GetAlarmsAsync<T>(this ObjectHandle<T> scope) where T : IDevice
        {
            return Task.Factory.StartNew(() =>
            {
                return scope.GetAlarms();
            });
        }
    }
}
