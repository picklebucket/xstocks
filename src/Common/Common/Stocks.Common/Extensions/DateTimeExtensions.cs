using System;

namespace Stocks.Common.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime ConvertToDateTime(this string str, DateTime defaultValue)
        {
            DateTime result;
            if (!DateTime.TryParse(str, out result))
            {
                return defaultValue;
            }

            return result;
        }

        public static long ToUnix(this DateTime date)
        {
            return (long)(date - UnixEpoch).TotalMilliseconds;
        }
    }
}
