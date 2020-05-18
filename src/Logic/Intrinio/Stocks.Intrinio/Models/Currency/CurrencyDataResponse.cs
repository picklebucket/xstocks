using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Intrinio.Models.Currency
{
    public class PriceResponseData
    {
        public DateTime OccurredAt { get; set; }

        public decimal? OpenBid { get; set; }

        public decimal? HighBid { get; set; }

        public decimal? LowBid { get; set; }

        public decimal? CloseBid { get; set; }

        public decimal? OpenAsk { get; set; }

        public decimal? HighAsk { get; set; }

        public decimal? LowAsk { get; set; }

        public decimal? CloseAsk { get; set; }

        public int? TotalTicks { get; set; }
    }

    public class CurrencyDataResponse
    {
        public string CurrencyPairCode { get; set; }

        public string BaseCurrency { get; set; }

        public string QuoteCurrency { get; set; }

        public List<PriceResponseData> Prices { get; set; }

        public CurrencyDataResponse()
        {
            this.Prices = new List<PriceResponseData>();
        }
    }
}
