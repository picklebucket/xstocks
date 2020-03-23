using Stocks.Intrinio.Models.PriceHistory;
using Stocks.Models.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Extensions
{
    public static class FrequencyTypeExtensions
    {
        public static IntrinioFrequencyType ToIntrinioFrequencyType(this FrequencyType frequencyType)
        {
            return frequencyType switch
            {
                FrequencyType.Daily => IntrinioFrequencyType.Daily,
                _ => IntrinioFrequencyType.Unspecified
            };
        }
    }
}
