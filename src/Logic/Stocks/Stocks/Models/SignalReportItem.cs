using Newtonsoft.Json;
using System;

namespace Stocks.Models
{
    public class SignalReportItem
    {
        public decimal? Open { get; set; }
        public decimal? Close { get; set; }
        public DateTime? Date { get; set; }
        public decimal Indicator { get; set; }
        public long Ticks { get; set; }

        [JsonIgnore]
        public bool IsSignal { get; set; }
    }
}
