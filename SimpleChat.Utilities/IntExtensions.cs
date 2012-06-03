using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleChat.Utilities
{
    public static class IntExtensions
    {
        public static TimeSpan Days(this int i)
        {
            return new TimeSpan(i, 0, 0, 0);
        }
        public static TimeSpan Hours(this int i)
        {
            return new TimeSpan(i, 0, 0);
        }
        public static TimeSpan Minutes(this int i)
        {
            return new TimeSpan(0, i, 0);
        }
        public static TimeSpan Seconds(this int i)
        {
            return new TimeSpan(0, 0, i);
        }
        public static TimeSpan Weeks(this int i)
        {
            return new TimeSpan(7 * i, 0, 0, 0);
        }
        public static TimeSpan Years(this int i)
        {
            return new TimeSpan(365 * i, 0, 0, 0);
        }
    }

}
