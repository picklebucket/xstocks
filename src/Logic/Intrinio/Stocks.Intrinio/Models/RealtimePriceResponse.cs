using System;

namespace Stocks.Intrinio.Models
{
    public class RealtimePriceResponse
    {
        public decimal? LastPrice { get; set; }
        
        public DateTime? LastTime { get; set; }

        public decimal? BidPrice { get; set; }

        public decimal? BidSize { get; set; }

        public decimal? AskPrice { get; set; }

        public decimal? AskSize { get; set; }

        public decimal? OpenPrice { get; set; }

        public decimal? HighPrice { get; set; }

        public decimal? LowPrice { get; set; }

        public decimal? ExchangeVolume { get; set; }

        public decimal? MarketVolume { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string Source { get; set; }
    }
}
