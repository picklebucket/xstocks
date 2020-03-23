using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Ameritrade.Models.PriceHistory
{
    public class AmeritradePriceHistoryRequest
    {
        public AmeritradePeriodType PeriodType { get; set; }
        public short? Period { get; set; }
        public AmeritradeFrequencyType FrequencyType { get; set; }
    }
}
