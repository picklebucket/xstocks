using Newtonsoft.Json;
using System;

namespace Stocks.Models
{
    public class Ema50ReportItem
    {
        public decimal? Open { get; set; }
        public decimal? Close { get; set; }
        public DateTime? Date { get; set; }
        public decimal Ema { get; set; }
        public long Ticks { get; set; }

        [JsonIgnore]
        public bool IsEma50 { get; set; }
    }
}
