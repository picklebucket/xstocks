using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Data.Entities
{
    public class PriceHistory : BaseEntity
    {
        public string Symbol { get; set; }
        public bool Empty { get; set; }
        public List<Candle> Candles { get; set; }
    }
}
