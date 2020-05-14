using System;

namespace Stocks.Data.Entities
{
    public class GoodPriceCompany : BaseEntity
    {
        public string Symbol { get; set; }

        public decimal LastTradePrice { get; set; }

        public DateTime LastTradeTime { get; set; }

        public decimal IntrinsicValue { get; set; }

        public DateTime DateTime { get; set; }
    }
}
