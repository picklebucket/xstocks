using System;
using System.Collections.Generic;

namespace Stocks.Data.Entities
{
    public class CurrencyData : BaseEntity
    {
        public string PairCode { get; set; }
        
        public string BaseCurrency { get; set; }

        public string QuoteCurrency { get; set; }

        public List<CurrencyPrice> Prices { get; set; }

        public CurrencyData()
        {
            this.Prices = new List<CurrencyPrice>();
        }
    }

    public class CurrencyPrice
    {
        public decimal? OpenBid { get; set; }

        public decimal? HighBid { get; set; }

        public decimal? LowBid { get; set; }

        public decimal? CloseBid { get; set; }

        public decimal? OpenAsk { get; set; }

        public decimal? HighAsk { get; set; }

        public decimal? LowAsk { get; set; }

        public decimal? CloseAsk { get; set; }

        public decimal? TotalTicks { get; set; }

        public DateTime OccurredAt { get; set; }
    }
}
