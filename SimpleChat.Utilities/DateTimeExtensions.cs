using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleChat.Utilities
{
    public static class DateTimeExtensions
    {
        public static string ToShortDateEqualLengthString(this DateTime dt)
        {
            var rv = new StringBuilder();
            if (dt.Month.ToString().Length == 1)
            {
                rv.Append("0");
            }
            rv.Append(dt.Month.ToString());
            rv.Append("/");
            if (dt.Day.ToString().Length == 1)
            {
                rv.Append("0");
            }
            rv.Append(dt.Day.ToString());
            rv.Append("/");
            rv.Append(dt.Year.ToString());

            return rv.ToString();
        }

        public static string ToShortDateTimeString(this DateTime dt)
        {
            return dt.ToShortDateString() + " " + dt.ToShortTimeString();
        }

        public static string ToTimeSpanWords(this DateTime dt)
        {
            var diff = DateTime.Now - dt;
            string end = string.Empty;
            if (dt < DateTime.Now)
                end = "ago";
            if (dt > DateTime.Now)
                end = "from now";

            var sb = new StringBuilder();

            if (diff.Days > 0)
            {
                sb.Append(diff.Days);
                sb.Append(" day");
                if (diff.Days > 1)
                    sb.Append("s");
            }
            else if (diff.Hours > 0)
            {
                sb.Append(diff.Hours);
                sb.Append(" hour");
                if (diff.Hours > 1)
                    sb.Append("s");
            }
            else if (diff.Minutes > 0)
            {
                sb.Append(diff.Minutes);
                sb.Append(" minute");
                if (diff.Minutes > 1)
                    sb.Append("s");
            }
            else if (diff.Seconds > 0)
            {
                sb.Append(diff.Seconds);
                sb.Append(" second");
                if (diff.Seconds > 1)
                    sb.Append("s");
            }
            else
            {
                sb.Append("moments");
            }
            sb.Append(" ");
            sb.Append(end);
            return sb.ToString();
        }
    }
}