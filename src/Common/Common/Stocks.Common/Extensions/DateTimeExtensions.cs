using System;

namespace Stocks.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ConvertToDateTime(this string str, DateTime defaultValue)
        {
            DateTime result;
            if (!DateTime.TryParse(str, out result))
            {
                return defaultValue;
            }

            return result;
        }
    }
}
