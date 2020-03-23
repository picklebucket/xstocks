using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Common.Helpers
{
    public static class DateHelper
    {
        public static long? GetDateTimeUnixEpochMilliseconds(DateTime? date)
        {
            long? result = null;
            if (date.HasValue)
            {
                var dateTimeOffset = new DateTimeOffset(date.Value.ToUniversalTime());
                result = dateTimeOffset.ToUnixTimeMilliseconds();
            }
            return result;
        }

        public static DateTimeOffset? GetDateTimeOffsetFromUnixTimeMilliseconds(long? milliseconds)
        {
            if(milliseconds.HasValue)
            {
                return DateTimeOffset.FromUnixTimeMilliseconds(milliseconds.Value);
            }
            return null;
        }

        public static DateTimeOffset? GetDateTimeOffsetFromDateTime(DateTime? date)
        {
            if(date.HasValue)
            {
                return new DateTimeOffset(date.Value.ToUniversalTime());
            }
            return null;
        }
    }
}
