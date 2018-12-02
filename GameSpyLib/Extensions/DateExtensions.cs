using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSpyLib
{
    public static class DateExtensions
    {
        /// <summary>
        /// Our Epoch Time Start
        /// </summary>
        public static readonly DateTime UnixStartDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Returns the current Unix Timestamp
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int ToUnixTimestamp(this DateTime target)
        {
            DateTime Unix = target.ToUniversalTime();
            return (int)Unix.Subtract(UnixStartDate).TotalSeconds;
        }

        /// <summary>
        /// Converts a timestamp to a UTC DataTime
        /// </summary>
        /// <param name="timestamp">The number of seconds from Epoch</param>
        /// <returns></returns>
        public static DateTime FromUnixTimestamp(this DateTime target, int timestamp)
        {
            return UnixStartDate.AddSeconds(timestamp);
        }
    }
}
