using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Stocks.Ameritrade.Models.PriceHistory
{
    public class CandleResponse
    {
        [JsonPropertyName("open")]
        public decimal Open { get; set; }

        [JsonPropertyName("close")]
        public decimal Close { get; set; }

        [JsonPropertyName("high")]
        public decimal High { get; set; }

        [JsonPropertyName("low")]
        public decimal Low { get; set; }

        [JsonPropertyName("datetime")]
        public long DateTimeUnixEpochMilliseconds { get; set; }

        [JsonPropertyName("volume")]
        public long Volume { get; set; }
    }
}
