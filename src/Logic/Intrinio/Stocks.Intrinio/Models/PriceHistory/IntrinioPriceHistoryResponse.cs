using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Intrinio.Models.PriceHistory
{
    public class PriceSummary
    {
        public decimal? Open { get; set; }
        public decimal? Close { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? Volume { get; set; }
        public DateTime? Date { get; set; }
    }

    public class IntrinioPriceHistoryResponse
    {
        public string Ticker { get; set; }
        public IEnumerable<PriceSummary> StockPrices { get; set; }
        public string NextPage { get; set; }
    }
}
