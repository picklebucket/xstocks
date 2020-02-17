using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Data.Entities
{
    public class PriceHistory : BaseEntity
    {
        public string Symbol { get; set; }
        public bool Empty { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
        public List<Candle> Candles { get; set; }

        public PriceHistory()
        {
            Candles = new List<Candle>();
        }
    }
}
