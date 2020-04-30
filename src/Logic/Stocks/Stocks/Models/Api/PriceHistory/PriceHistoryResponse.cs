using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Models.Api.PriceHistory
{
    public class PriceHistoryResponse
    {
        public List<CandleItem> CandleItems { get; }
        public string Symbol { get; set; }
        public bool IsEmpty { get; set; }

        public PriceHistoryResponse(string symbol, bool isEmpty, List<CandleItem> candleItems)
        {
            Symbol = symbol;
            IsEmpty = isEmpty;
            CandleItems = candleItems;
        }        
    }
}
