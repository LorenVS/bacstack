using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Ashrae
{
    public partial class DateAndTime
    {
        /// <summary>
        /// Creates a DateAndTime instance from a DateTime
        /// </summary>
        /// <param name="dt">The datetime instance</param>
        /// <returns>The DateAndTime instance</returns>
        public static DateAndTime FromDateTime(DateTime dt)
        {
            return new DateAndTime(
                new Date(
                    (byte)(dt.Year - 1900),
                    (byte)(dt.Month),
                    (byte)(dt.Day),
                    (byte)(dt.DayOfWeek)),
                new Time(
                    (byte)(dt.Hour),
                    (byte)(dt.Minute),
                    (byte)(dt.Second),
                    (byte)(dt.Millisecond / 10)));
        }

        public DateTime ToDateTime()
        {
            return new DateTime(
                this.Date.Year + 1900,
                this.Date.Month,
                this.Date.Day,
                this.Time.Hour,
                this.Time.Minute,
                this.Time.Second,
                this.Time.Hundredths * 10);
        }
    }
}
