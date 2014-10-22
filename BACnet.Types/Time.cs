using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public struct Time
    {
        /// <summary>
        /// The hour of the time
        /// </summary>
        public byte Hour { get; private set; }

        /// <summary>
        /// The minute of the time
        /// </summary>
        public byte Minute { get; private set; }

        /// <summary>
        /// The second of the time
        /// </summary>
        public byte Second { get; private set; }

        /// <summary>
        /// The hundredths of a second of the time
        /// </summary>
        public byte Hundredths { get; private set; }

        /// <summary>
        /// Constructs a new time instance
        /// </summary>
        /// <param name="hour">The hour component of the time</param>
        /// <param name="minute">The minute component of the time</param>
        /// <param name="second">The second component of the time</param>
        /// <param name="hundredths">The hundredths component of the time</param>
        public Time(byte hour, byte minute, byte second, byte hundredths) : this()
        {
            this.Hour = hour;
            this.Minute = minute;
            this.Second = second;
            this.Hundredths = hundredths;
        }
    }
}
