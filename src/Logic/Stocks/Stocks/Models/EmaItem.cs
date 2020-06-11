using System;

namespace Stocks.Models
{
    public class SignalIndicatorItem
    {
        public DateTimeOffset? Date { get; set; }
        public decimal? Value { get; set; }

        public SignalIndicatorItem(DateTimeOffset? date)
        {
            Date = date;
        }

        public SignalIndicatorItem(DateTimeOffset? date, decimal? value)
            : this(date)
        {
            Value = value;
        }
    }
}
