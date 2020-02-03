using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Models
{
    public class CandleModel
    {
        public double? Open { get; set; }
        public double? Close { get; set; }
        public double? High { get; set; }
        public double? Low { get; set; }
        public DateTimeOffset? Date { get; set; }
        public long? Volume { get; set; }

        public CandleModel()
        {
        }

        public CandleModel(DateTimeOffset date)
        {
            Date = date;
        }
    }
}
