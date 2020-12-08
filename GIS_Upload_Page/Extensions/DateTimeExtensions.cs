using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIS_Upload_Page.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDatabaseDate(this DateTime value)
        {
            return value;
        }

        public static DateTime ToDisplayDate(this DateTime value)
        {
            return value;
        }

        public static DateTime? ToDatabaseDate(this DateTime? value)
        {
            return value;
        }

        public static DateTime? ToDisplayDate(this DateTime? value)
        {
            return value;
        }

        public static DateTime ToDatabaseDateTime(this DateTime value)
        {
            if (value != null)
            {
                value = value.ToUniversalTime();
            }
            return value;
        }

        public static DateTime ToDisplayDateTime(this DateTime value)
        {
            if (value != null)
            {
                value = value.ToLocalTime();
            }
            return value;
        }

        public static DateTime? ToDatabaseDateTime(this DateTime? value)
        {
            if (value.HasValue)
            {
                value = value.Value.ToUniversalTime();
            }
            return value;
        }

        public static DateTime? ToDisplayDateTime(this DateTime? value)
        {
            if (value.HasValue)
            {
                value = value.Value.ToLocalTime();
            }
            return value;
        }
    }
}
