using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Data.Entities
{
    public class Candle
    {
        public decimal? Open { get; set; }
        public decimal? Close { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public long? DateTimeUnixEpochMilliseconds { get; set; }
        public decimal? Volume { get; set; }
    }
}
