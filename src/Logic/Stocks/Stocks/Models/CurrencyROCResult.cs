using System;
using System.Collections.Generic;

namespace Stocks.Models
{
    public class CurrencyPriceROC
    {
        public decimal? CloseBid { get; set; }

        public decimal? CloseAsk { get; set; }

        public DateTime OccurredAt { get; set; }

        public decimal? Week1Rok { get; set; }

        public decimal? Week2Rok { get; set; }
    }

    public class CurrencyROCResult
    {
        public string PairCode { get; set; }

        public List<CurrencyPriceROC> Data { get; } = new List<CurrencyPriceROC>();
    }
}
