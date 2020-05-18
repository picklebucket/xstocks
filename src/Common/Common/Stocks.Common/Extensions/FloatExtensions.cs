using System;
using System.Globalization;

namespace Stocks.Common.Extensions
{
    public static class FloatExtensions
    {
        public static decimal? ToDecimal(this float? num)
        {
            if (num.HasValue)
            {
                return Convert.ToDecimal(num.Value, CultureInfo.InvariantCulture);
            }

            return null;
        }
    }
}
