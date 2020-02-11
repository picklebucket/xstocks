using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Models
{
    public class ReportItem
    {
        public string Symbol { get; set; }
        public AtrItem AtrItem { get; set; }
        public CandleItem CandleItem { get; set; }
        public IchimokuItem IchimokuItem { get; set; }
        public IndicatorItem IndicatorItem { get; set; }
    }
}
