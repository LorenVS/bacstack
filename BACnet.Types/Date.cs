using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public struct Date
    {
        /// <summary>
        /// The year of the date, as a single byte since 1900
        /// </summary>
        public byte Year { get; private set; }

        /// <summary>
        /// The month of the date
        /// </summary>
        public byte Month { get; private set; }

        /// <summary>
        /// The day of month
        /// </summary>
        public byte Day { get; private set; }

        /// <summary>
        /// The day of week
        /// </summary>
        public byte DayOfWeek { get; private set; }

        /// <summary>
        /// Constructs a new DateValue instance
        /// </summary>
        /// <param name="year">The year of the date value</param>
        /// <param name="month">The month of the date value</param>
        /// <param name="day">The day of the date value</param>
        /// <param name="dayOfWeek">The day of week of the date value</param>
        public Date(byte year, byte month, byte day, byte dayOfWeek) : this()
        {
            this.Year = year;
            this.Month = month;
            this.Day = day;
            this.DayOfWeek = dayOfWeek;
        }
    }
}
