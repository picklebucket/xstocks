using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Models
{
    public class CandleItem
    {
        public double? Open { get; set; }
        public double? Close { get; set; }
        public double? High { get; set; }
        public double? Low { get; set; }
        public DateTimeOffset? Date { get; set; }
        public long? Volume { get; set; }

        public CandleItem()
        {
        }

        public CandleItem(DateTimeOffset date)
        {
            Date = date;
        }
    }
}
