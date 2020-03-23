using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Stocks.Ameritrade.Models.PriceHistory
{
    public class PriceHistoryResponse
    {
        [JsonPropertyName("empty")]
        public bool Empty { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("candles")]
        public IEnumerable<CandleResponse> Candles { get; set; }
    }

}
