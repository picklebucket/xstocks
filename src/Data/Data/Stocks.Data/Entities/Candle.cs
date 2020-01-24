using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Data.Entities
{
    public class Candle
    {
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public long DateTimeUnixEpochMilliseconds { get; set; }
        public long Volume { get; set; }
    }
}
