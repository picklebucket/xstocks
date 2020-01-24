using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Ameritrade.Models.PriceHistory
{
    public class PriceHistoryRequest
    {
        public PeriodType PeriodType { get; set; }
        public short? Period { get; set; }
        public FrequencyType FrequencyType { get; set; }
    }
}
