using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Models
{
    public enum IndicatorState
    {
        None,
        Win,
        Lose
    }

    public class IndicatorItem
    {
        public decimal? EntryPrice { get; set; }
        public decimal? StopLoss { get; set; }
        public decimal? TakeProfit { get; set; }
        public PriceResult PriceResult { get; set; }
        public Guid Id { get; set; }
        public DateTimeOffset? ReachedDate { get; set; }
        public IndicatorState State { get; set; }
    }
}
