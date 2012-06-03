using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleChat.Utilities
{
    public static class TimeSpanExtensions
    {
        public static DateTime Ago(this TimeSpan ts)
        {
            return DateTime.Now.Add(ts.Negate());
        }

        public static DateTime FromNow(this TimeSpan ts)
        {
            return DateTime.Now.Add(ts);
        }
    }
}
