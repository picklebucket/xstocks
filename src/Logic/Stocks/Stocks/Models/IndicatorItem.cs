using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Models
{
    public class IndicatorItem
    {
        public double? StopLoss { get; set; }
        public double? TakeProfit { get; set; }
        public PriceResult PriceResult { get; set; }
    }
}
