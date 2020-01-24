using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Stocks.Ameritrade.Models.PriceHistory
{
    public class Candle
    {
        [JsonPropertyName("open")]
        public double Open { get; set; }

        [JsonPropertyName("close")]
        public double Close { get; set; }

        [JsonPropertyName("high")]
        public double High { get; set; }

        [JsonPropertyName("low")]
        public double Low { get; set; }

        [JsonPropertyName("datetime")]
        public long DateTimeUnixEpochMilliseconds { get; set; }

        [JsonPropertyName("volume")]
        public long Volume { get; set; }
    }
}
